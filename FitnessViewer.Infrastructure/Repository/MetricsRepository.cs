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

        public void AddMeasurement(Measurement m)
        {
            _context.Measurement.Add(m);
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

        internal void AddMetric(Metric m)
        {
            _context.Metric.Add(m);
        }

        public List<WeightByDayDto> GetWeightDetails(string userId, int days)
        {
            days = days - 1;

            return GetWeightDetails(userId, DateTime.Now.Date.AddDays(days * -1), DateTime.Now);
        }

        public List<WeightByDayDto> GetWeightDetails(string userId, DateTime from, DateTime to)

        {
            DateTime dataFrom = from.AddDays(-30);
            // grab a copy of the needed data from database once then we'll work out figures from local list.
            var metrics = _context.Metric
                 .Where(m => m.UserId == userId && 
                            m.MetricType == enums.MetricType.Weight && 
                            m.Recorded >= dataFrom && 
                            m.Recorded <= to)
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
                WeightByDayDto w = new WeightByDayDto(day);

                // work out 30 day average weight for the current date
                var Day30Data = metrics.Where(m => m.Recorded >= day.AddDays(-30) && m.Recorded <= day).ToList();

                if (Day30Data.Count != 0)
                {
                    w.Low30Day = Day30Data.Min(d => d.Value);
                    w.Average30Day = Math.Round(Day30Data.Sum(d => d.Value) / Day30Data.Count, 2);
                    w.High30Day = Day30Data.Max(d => d.Value);
                }
                // work out 7 day average weight for the current date
                var Day7Data = metrics.Where(m => m.Recorded >= day.AddDays(-7) && m.Recorded <= day).ToList();

                if (Day7Data.Count != 0)
                {
                    w.Low7Day = Day7Data.Min(d => d.Value);
                    w.Average7Day = Math.Round(Day7Data.Sum(d => d.Value) / Day7Data.Count, 2);
                    w.High7Day = Day7Data.Max(d => d.Value);
                }

                var currentQuery = metrics
                        .Where(m => m.Recorded <= day)
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

    }
}