﻿using Fitbit.Api.Portable;
using Fitbit.Api.Portable.OAuth2;
using Fitbit.Models;
using FitnessViewer.Infrastructure.Data;
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

        public FitbitHelper(string userId)
        {
            _unitOfWork = new UnitOfWork();
            _client = CreateFitbitClient(userId);
            _userId = userId;
        }

        /// <summary>
        /// Add (or update) fitbit user/access token details.
        /// </summary>
        /// <param name="userId">ASP.NET identity userId</param>
        /// <param name="accessToken">Fitbit access token.</param>
        public static void AddOrUpdateUser(string userId, OAuth2AccessToken accessToken)
        {
            UnitOfWork uow = new UnitOfWork();

            FitbitUser fitbitUser = uow.Metrics.GetFitbitUser(userId);

            if (fitbitUser == null)
            {
                // user doesn't exist in Fitbit table so create
                FitbitUser u = FitbitUser.Create(userId, accessToken);
                uow.Metrics.AddFitbitUser(u);
                uow.Complete();
            }
            else
            {
                FitbitHelper helper = new FitbitHelper(userId);
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
        public async void RefreshToken()
        {
            OAuth2AccessToken refreshedToken = await _client.RefreshOAuth2TokenAsync();

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
            fitbitUser.RefreshToken = accessToken.UserId;
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
        public  void Download()
        {

  //          RefreshToken();

            DownloadMetric(TimeSeriesResourceType.Weight, true);

            DownloadMetric(TimeSeriesResourceType.Fat, true);

            return ;
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

        private async void DownloadMetric(TimeSeriesResourceType type, DateTime dateStart)
        {
            TimeSeriesDataList fitbitData = null;
        
            try
            {

       

                fitbitData = await _client.GetTimeSeriesAsync(type, dateStart, DateRangePeriod.OneYear);
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
                var existingMetric = currentlyStoredMetrics.Where(m => m.Recorded == item.DateTime).FirstOrDefault();

                if (existingMetric == null)
                {         
                    _unitOfWork.Metrics.AddMetric(Metric.CreateMetric(_userId, metricType, item.DateTime, Convert.ToDecimal(item.Value)));
                }
                else
                    existingMetric.Value = Convert.ToDecimal(item.Value);

            }
            _unitOfWork.Complete();
        }
    }
}
