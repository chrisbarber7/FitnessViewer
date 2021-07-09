using FitnessViewer.Infrastructure.Core.Data;
using FitnessViewer.Infrastructure.Core.Models;
using System.Linq;
//using Fitbit.Api.Portable.OAuth2;
//using System.Data.Entity;
using Microsoft.EntityFrameworkCore;



namespace FitnessViewer.Infrastructure.Core.Repository
{
    public class MetricsRepository
    {
        private ApplicationDb _context;

        public MetricsRepository(ApplicationDb context)
        {
            _context = context;
        }

       // internal void AddFitbitUser(FitbitUser u)
       // {
       //     _context.FitbitUser.Add(u);
       // }

       // public FitbitUser GetFitbitUser(string userId)
       // {
       //     return _context.FitbitUser.Where(u => u.UserId == userId).FirstOrDefault();
       // }

        //internal OAuth2AccessToken GetFitbitAccessToken(string userId)
        //{
        //    return _context.FitbitUser
        //        .Where(u => u.UserId == userId)
        //        .Select(t => new OAuth2AccessToken()
        //        {
        //            RefreshToken = t.RefreshToken,
        //            Token = t.Token,
        //            TokenType = t.TokenType,
        //            UserId = t.FitbitUserId

        //        }
        //        )
        //        .FirstOrDefault();
        //}

        /// <summary>
        /// Add or Update a metric record
        /// </summary>
        /// <param name="metric">Metric details</param>
        public void AddOrUpdateMetric(Metric metric)
        {
            AddOrUpdateMetric(metric, false);

        }

        /// <summary>
        /// Add a new metric
        /// </summary>
        /// <param name="metric">Metric details</param>
        public void AddMetric(Metric metric)
        {
            AddOrUpdateMetric(metric, true);
        }

        /// <summary>
        /// Add or Update a metric record
        /// </summary>
        /// <param name="metric">Metric details</param>
        /// <param name="isNew"></param>
        private void AddOrUpdateMetric(Metric metric, bool isNew)
        {

            if (!isNew)
            {
                // check if metric already exists?
                Metric existing = _context.Metric
                                            .Where(m => m.UserId == metric.UserId &&
                                                        m.Recorded == metric.Recorded &&
                                                        m.MetricType == metric.MetricType)
                                            .FirstOrDefault();

                // if it doesn't exist then add it, otherwise update
                if (existing == null)
                    isNew = true;
                else
                {
                    existing.Value = metric.Value;
                    UpdateMetric(existing);
                }
            }

            if (isNew)
                _context.Metric.Add(metric);
        }

        /// <summary>
        /// Update existing metric record
        /// </summary>
        /// <param name="amended">Amended Metric details</param>
        internal void UpdateMetric(Metric amended)
        {
            _context.Metric.Attach(amended);
            _context.Entry(amended).State = EntityState.Modified;
        }

        /// <summary>
        /// Update weight on an activity
        /// </summary>
        /// <param name="activityId">Strava activity Id</param>
        /// <param name="userId">ASP.NET Identity id</param>
        /// <param name="weightOnActivityDay">Weight on/both activity</param>
        internal void UpdateActivityWeight(long activityId, string userId, decimal weightOnActivityDay)
        {
            var activity = new Activity() { Id = activityId, Weight = weightOnActivityDay };

            _context.Activity.Attach(activity);
            _context.Entry(activity).Property(w => w.Weight).IsModified = true;
        }
    }
}