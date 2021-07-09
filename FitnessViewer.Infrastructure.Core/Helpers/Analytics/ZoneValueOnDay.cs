using FitnessViewer.Infrastructure.Core.enums;
using FitnessViewer.Infrastructure.Core.Interfaces;
using FitnessViewer.Infrastructure.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Core.Helpers.Analytics
{
    public class ZoneValueOnDay
    {
        private ISettingsRepository _repo;

        public ZoneValueOnDay(ISettingsRepository repo)
        {
            _repo = repo;
        }

        public ZoneValueOnDay()
        {
            _repo = new SettingsRepository(new Data.ApplicationDb());
        }

        /// <summary>
        /// Get a zone value (eg FTP, Pace, Heart Rate) on a given date.
        /// </summary>
        /// <param name="userId">ASP.NET Identity Id</param>
        /// <param name="zone">Required Zone Type</param>
        /// <param name="activityDate">Activity Date</param>
        /// <returns></returns>
        public int? GetUserZoneValueOnGivenDate(string userId, ZoneType zone, DateTime activityDate)
        {
            // remove time portion.
            activityDate = activityDate.Date;

            // get all values for the user/zone.
            var allValues = _repo.GetUserZones(userId, zone).ToList();

            if (allValues.Count == 0)
                return null;

            // file to before the activity date.
            var list = allValues
                         .OrderByDescending(z => z.StartDate)
                         .Where(z => z.StartDate <= activityDate)
                         .ToList();

            if (list.Count == 0)
                return null;

            // return the first before the activity date.
               return list
                 .Select(z => z.Value)
                 .FirstOrDefault();
        }
    }
}
