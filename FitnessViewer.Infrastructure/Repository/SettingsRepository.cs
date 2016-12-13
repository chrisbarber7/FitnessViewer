using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.enums;
using FitnessViewer.Infrastructure.Interfaces;
using FitnessViewer.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FitnessViewer.Infrastructure.Repository
{
    public class SettingsRepository : ISettingsRepository
    {
        private ApplicationDb _context;

        public SettingsRepository(ApplicationDb context)
        {
            _context = context;
        }

        public SettingsRepository()
        {
            _context = new ApplicationDb();

        }

        /// <summary>
        /// List of zones for given user/sport
        /// </summary>
        /// <param name="userId">ASP.NET Identity Id</param>
        /// <param name="zone">Zone Type (Bike, Run, Swim - HeartRate/Pace/etc)</param>
        /// <returns>List of Zones</returns>
        public IEnumerable<Zone> GetUserZones(string userId, ZoneType zone)
        {
            return _context.Zone
                        .Where(z => z.UserId == userId && z.ZoneType == zone)
                        .OrderBy(z => z.StartDate)
                        .ToList();
        }

        /// <summary>
        /// List of zones for given user/sport
        /// </summary>
        /// <param name="userId">ASP.NET Identity Id</param>
        /// <returns>List of Zones</returns>
        public IEnumerable<Zone> GetUserZones(string userId)
        {
            return _context.Zone
                        .Where(z => z.UserId == userId)
                        .OrderBy(z => z.StartDate)
                        .ToList();
        }


        /// <summary>
        /// List of Zone ranges for a given user/sport
        /// </summary>
        /// <param name="userId">ASP.NET Identity Id</param>
        /// <param name="zone">Zone Type (Bike, Run, Swim - HeartRate/Pace/etc)</param>
        /// <returns>List of ZoneRanges</returns>
        public List<ZoneRange> GetUserZoneRanges(string userId)
        {
            var zones = _context.ZoneRange
                  .Where(z => z.UserId == userId)
                  .OrderBy(z => z.ZoneType)
                  .ThenBy(z=>z.ZoneStart)
                  .ToList();

            if (zones.Count > 0)
                return zones;
            else
                return new List<ZoneRange>() { ZoneRange.CreateDefault() };
        }
    }
}
