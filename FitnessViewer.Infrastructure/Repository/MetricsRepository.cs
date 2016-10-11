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

        internal FitbitUser GetFitbitUser(string userId)
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

        public List<WeightByDay> GetWeightDetails(string userId, int days)
        {

            // we'll need to get date for the required number of days + 30 so we've got the data for the 30 day rolling average.
            DateTime lowerDate = DateTime.Now.Date.AddDays((days + 30) * -1);

            // grab a copy of the needed data from database once then we'll work out figures from local list.
            var metrics = _context.Metric
                 .Where(m => m.UserId == userId && m.MetricType == enums.MetricType.Weight && m.Recorded >= lowerDate)
                 .Select(m => new
                 {
                     Recorded = m.Recorded,
                     Value = m.Value
                 })
                 .ToList();

            List<WeightByDay> results = new List<WeightByDay>();

            DateTime day = DateTime.Now.Date;


            if (metrics.Count == 0)
            {
                results.Add(new WeightByDay(day));
                return results;
            }

            while (day >= DateTime.Now.Date.AddDays(days * -1))
            {
                WeightByDay w = new WeightByDay(day);

                // work out 30 day average weight for the current date
                var Day30Data = metrics.Where(m => m.Recorded >= day.AddDays(-30) && m.Recorded <= day).ToList();

                if (Day30Data.Count != 0)
                    w.Average30Day = Day30Data.Sum(d => d.Value) / Day30Data.Count;

                // work out 7 day average weight for the current date
                var Day7Data = metrics.Where(m => m.Recorded >= day.AddDays(-7) && m.Recorded <= day).ToList();

                if (Day7Data.Count != 0)
                    w.Average7Day = Day7Data.Sum(d => d.Value) / Day7Data.Count;

                w.Current = metrics
                      .Where(m => m.Recorded <= day)
                      .OrderByDescending(m => m.Recorded).First().Value;

                results.Add(w);

                day = day.AddDays(-1);
            }
            return results;
        }

    }
}