using System;
using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.Models;
using System.Linq;
using Fitbit.Api.Portable.OAuth2;

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
    }
}