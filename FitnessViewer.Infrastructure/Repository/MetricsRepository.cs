using System;
using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.Models;
using System.Linq;
using Fitbit.Api.Portable.OAuth2;
using FitnessViewer.Infrastructure.enums;
using System.Collections.Generic;
using FitnessViewer.Infrastructure.Models.Dto;

namespace FitnessViewer.Infrastructure.Repository
{
    public class MetricsRepository
    {
        private ApplicationDb _context;

        public MetricsRepository(ApplicationDb context)
        {
            _context = context;
        }

        public void AddFitbitUser(FitbitUser u)
        {
            _context.FitbitUser.Add(u);
        }

        public FitbitUser GetFitbitUser(string userId)
        {
            return _context.FitbitUser.Where(u => u.UserId == userId).FirstOrDefault();
        }

        internal OAuth2AccessToken GetFitbitAccessToken(string userId)
        {
            return _context.FitbitUser
                .Where(u => u.UserId == userId)
                .Select(t => new OAuth2AccessToken()
                {
                    RefreshToken = t.RefreshToken,
                    Token = t.Token,
                    TokenType = t.TokenType,
                    UserId = t.FitbitUserId

                }
                )
                .FirstOrDefault();
        }


        internal List<Metric> GetMetrics(string userId, MetricType metricType)
        {
            return _context.Metric
                .Where(m => m.UserId == userId && m.MetricType == metricType)
                .ToList();
        }


        public void AddOrUpdateMetric(Metric metric)
        {
            // check if metric already exists?
            Metric existing = _context.Metric
                                        .Where(m => m.UserId == metric.UserId &&
                                                    m.Recorded == metric.Recorded &&
                                                    m.MetricType == metric.MetricType)
                                        .FirstOrDefault();

            if (existing == null)
                _context.Metric.Add(metric);
            else
                existing.Value = metric.Value;
        }

        public List<WeightByDayDto> GetMetricDetails(string userId, MetricType type, int days)
        {
            // to return requested number of days we need to subtract one (i.e. if days is 30 we need to go back 29 days to return 30 days values).
            DateTime fromDate = DateTime.Now.Date.AddDays((days - 1) * -1);

            return GetMetricDetails(userId, type, fromDate, DateTime.Now);
        }

        public List<WeightByDayDto> GetMetricDetails(string userId, MetricType type, DateTime from, DateTime to)
        {
            DateTime upperDateTime = to.AddHours(23).AddMinutes(59).AddSeconds(59);

            DateTime dataFrom = from.AddDays(-30);
            // grab a copy of the needed data from database once then we'll work out figures from local list.
            var metrics = _context.Metric
                 .Where(m => m.UserId == userId &&
                            m.MetricType == type &&
                            m.Recorded >= dataFrom &&
                            m.Recorded <= upperDateTime)
                 .Select(m => new
                 {
                     Recorded = m.Recorded,
                     Value = m.Value
                 })
                 .ToList();

            List<WeightByDayDto> results = new List<WeightByDayDto>();

            if (metrics.Count == 0)
            {
                results.Add(new WeightByDayDto(DateTime.Now));
                return results;
            }

            DateTime day = from;

            while (day <= to)
            {
                // need to ensure that we include records which also have a time element.
                DateTime toCompareAgainst = day.AddHours(23).AddMinutes(59).AddSeconds(59);


                WeightByDayDto w = new WeightByDayDto(day);

                // work out 30 day average weight for the current date
                var Day30Data = metrics.Where(m => m.Recorded >= day.AddDays(-30) && m.Recorded <= toCompareAgainst).ToList();

                if (Day30Data.Count != 0)
                {
                    w.Low30Day = Day30Data.Min(d => d.Value);
                    w.Average30Day = Math.Round(Day30Data.Sum(d => d.Value) / Day30Data.Count, 2);
                    w.High30Day = Day30Data.Max(d => d.Value);
                }
                // work out 7 day average weight for the current date
                var Day7Data = metrics.Where(m => m.Recorded >= day.AddDays(-7) && m.Recorded <= toCompareAgainst).ToList();

                if (Day7Data.Count != 0)
                {
                    w.Low7Day = Day7Data.Min(d => d.Value);
                    w.Average7Day = Math.Round(Day7Data.Sum(d => d.Value) / Day7Data.Count, 2);
                    w.High7Day = Day7Data.Max(d => d.Value);
                }

                var currentQuery = metrics
                        .Where(m => m.Recorded <= toCompareAgainst)
                        .OrderByDescending(m => m.Recorded)
                        .FirstOrDefault();


                if (currentQuery == null)
                    w.Current = null;
                else
                    w.Current = currentQuery.Value;

                var weight7daysAgoQuery = metrics
                       .Where(m => m.Recorded <= day.AddDays(-7))
                       .OrderByDescending(m => m.Recorded)
                       .FirstOrDefault();

                decimal? weight7daysAgo = null;
                if (weight7daysAgoQuery != null)
                    weight7daysAgo = weight7daysAgoQuery.Value;


                var weight30daysAgoQuery = metrics
                      .Where(m => m.Recorded <= day.AddDays(-30))
                      .OrderByDescending(m => m.Recorded)
                      .FirstOrDefault();


                decimal? weight30daysAgo = null;
                if (weight30daysAgoQuery != null)
                    weight30daysAgo = weight30daysAgoQuery.Value;

                if ((weight7daysAgo != null) && (w.Current != null))
                    w.Change7Day = w.Current.Value - weight7daysAgo.Value;

                if ((weight30daysAgo != null) && (w.Current != null))
                    w.Change30Day = w.Current.Value - weight30daysAgo.Value;


                results.Add(w);

                day = day.AddDays(1);
            }
            return results;
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