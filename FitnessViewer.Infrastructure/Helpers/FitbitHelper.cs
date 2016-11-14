using Fitbit.Api.Portable;
using Fitbit.Api.Portable.OAuth2;
using Fitbit.Models;
using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.enums;
using FitnessViewer.Infrastructure.Helpers.Conversions;
using FitnessViewer.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Helpers
{
    public class FitbitHelper
    {
        private FitbitClient _client;
        private UnitOfWork _unitOfWork;
        private string _userId;

        public FitbitHelper(UnitOfWork uow, string userId)
        {
            _unitOfWork = uow;
            _client = CreateFitbitClient(userId);
            _userId = userId;
        }

        /// <summary>
        /// Add (or update) fitbit user/access token details.
        /// </summary>
        /// <param name="userId">ASP.NET identity userId</param>
        /// <param name="accessToken">Fitbit access token.</param>
        public static void AddOrUpdateUser(UnitOfWork uow, string userId, OAuth2AccessToken accessToken)
        {
            FitbitUser fitbitUser = uow.Metrics.GetFitbitUser(userId);

            if (fitbitUser == null)
            {
                // user doesn't exist in Fitbit table so create
                FitbitUser u = FitbitUser.Create(userId, accessToken);
                uow.Metrics.AddFitbitUser(u);

                DownloadQueue.CreateQueueJob(userId, enums.DownloadType.Fitbit).Save();
            }
            else
            {
                FitbitHelper helper = new FitbitHelper(uow, userId);
                helper.StoreFitbitToken(accessToken);
            }
        }

        /// <summary>
        /// Return application credentials (created at https://dev.fitbit.com/apps/new) and stored in web.config
        /// </summary>
        /// <returns></returns>
        public static FitbitAppCredentials GetFitbitAppCredentials()
        {
            return new FitbitAppCredentials()
            {
                ClientId = ConfigurationManager.AppSettings["FitbitClientId"],
                ClientSecret = ConfigurationManager.AppSettings["FitbitClientSecret"]
            };
        }

        /// <summary>
        /// Refresh expired token 
        /// </summary>
        public  void RefreshToken()
        {
            OAuth2AccessToken refreshedToken =  _client.RefreshOAuth2TokenAsync().Result;

            StoreFitbitToken(refreshedToken);
        }

        /// <summary>
        /// Store Fitbit token details against user
        /// </summary>
        /// <param name="accessToken"></param>
        private void StoreFitbitToken(OAuth2AccessToken accessToken)
        {
            FitbitUser fitbitUser = _unitOfWork.Metrics.GetFitbitUser(_userId);

            fitbitUser.FitbitUserId = accessToken.UserId;
            fitbitUser.RefreshToken = accessToken.RefreshToken;
            fitbitUser.Token = accessToken.Token;
            fitbitUser.TokenType = accessToken.TokenType;
            _unitOfWork.Complete();
        } 

        /// <summary>
        /// Create Fitbit access client.
        /// </summary>
        /// <param name="userId">ASP.NET Identify userId</param>
        /// <returns></returns>
        private FitbitClient CreateFitbitClient(string userId)
        {
            OAuth2AccessToken token = _unitOfWork.Metrics.GetFitbitAccessToken(userId);
            return new FitbitClient(FitbitHelper.GetFitbitAppCredentials(), token);
        }

        /// <summary>
        /// Dummy method to test downloads.
        /// </summary>
        /// <returns></returns>
        public void Download(bool fullDownload)
        {

            RefreshToken();

            // which metrics are we interested in?
            List<TimeSeriesResourceType> series = new List<TimeSeriesResourceType>()
            {
                 TimeSeriesResourceType.TimeEnteredBed,
                TimeSeriesResourceType.MinutesAsleep,
                TimeSeriesResourceType.TimeInBed,
                TimeSeriesResourceType.CaloriesIn,
                TimeSeriesResourceType.Weight,
                TimeSeriesResourceType.Fat
            };
        
            foreach (TimeSeriesResourceType r in series)
                DownloadMetric(r, true);

            // as weight details have changed we need to refresh weights recorded against activities
            ActivityWeight w = new ActivityWeight(_userId);
            w.UpdateActivityWeight();

            _unitOfWork.Complete();
            return;
        }

        private void DownloadMetric(TimeSeriesResourceType type, bool fullDownload)
        {
            if (!fullDownload)
                DownloadMetric(type, DateTime.UtcNow);
            else
            {
                // if initial download or a forced refresh then attemt to download last 5 years worth of data.
                for(int year=0; year>=-5; year--)               
                    DownloadMetric(type, DateTime.UtcNow.AddYears(year));
            }

        }

        private void DownloadMetric(TimeSeriesResourceType type, DateTime dateStart)
        {
            TimeSeriesDataList fitbitData = null;

            try
            {
                fitbitData = _client.GetTimeSeriesAsync(type, dateStart, DateRangePeriod.OneYear).Result;
                SaveSeries(type, fitbitData);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return;
            }
        }

        private void SaveSeries(TimeSeriesResourceType type, TimeSeriesDataList fitbitData)
        {
            var metricType = MetricTypeConversion.FromFitBitType(type);
            List<Metric> currentlyStoredMetrics = _unitOfWork.Metrics.GetMetrics(_userId, metricType);

            foreach (Fitbit.Models.TimeSeriesDataList.Data item in fitbitData.DataList)
            {
                // no value so ignore
                if (string.IsNullOrEmpty(item.Value))
                    continue;

                decimal convertedValue = FitbitConversion.ConvertFitbitValue(type, item.Value);

                // 0 = no value for the type/date so ignore
                if (convertedValue == FitbitConversion.InvalidValue)
                    continue;
                
                // check if we already have a metric for the given date.
                var existingMetric = currentlyStoredMetrics.Where(m => m.Recorded == item.DateTime).FirstOrDefault();

                if (existingMetric == null)
                {
                    // add metric
                    _unitOfWork.Metrics.AddMetric(Metric.CreateMetric(_userId, metricType, item.DateTime, convertedValue, false));
                }
                else
                {
                    // don't overwrite any manually entered metrics.
                    if (!existingMetric.IsManual)
                    {
                        existingMetric.Value = convertedValue;
                        _unitOfWork.Metrics.UpdateMetric(existingMetric);
                    }
                }

            }
            _unitOfWork.Complete();
        }

        
    }
}

