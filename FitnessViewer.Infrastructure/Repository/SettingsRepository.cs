using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.enums;
using FitnessViewer.Infrastructure.Models;
using System.Collections.Generic;
using System.Linq;

namespace FitnessViewer.Infrastructure.Repository
{
    public class SettingsRepository
    {
        private ApplicationDb _context;

        public SettingsRepository(ApplicationDb context)
        {
            _context = context;
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

    
        ///// <summary>
        ///// Calculate time spent in each zone.
        ///// </summary>
        ///// <param name="fvActivity">FitnessViewer Activity details</param>
        ///// <param name="zoneType">Zone type to return</param>
        ///// <returns></returns>
        //public IEnumerable<ZoneValueDto> GetZoneValues(Activity fvActivity, ZoneType zoneType)
        //{
        //    int?[] stream = null;

        //    if (zoneType == ZoneType.BikePower)
        //        stream = _context.Stream
        //                    .Where(s => s.ActivityId == fvActivity.Id && s.Watts.HasValue)
        //                    .Select(s => s.Watts)
        //                    .ToArray();
        //    else if (zoneType == ZoneType.RunHeartRate || zoneType == ZoneType.BikeHeartRate)
        //        stream = _context.Stream
        //                    .Where(s => s.ActivityId == fvActivity.Id && s.HeartRate.HasValue)
        //                    .Select(s => s.HeartRate)
        //                    .ToArray();
        //    else if (zoneType == ZoneType.RunPace)
        //        stream = GetSecondsPerMileFromVelocity(
        //            _context.Stream
        //                    .Where(s => s.ActivityId == fvActivity.Id && s.Velocity.HasValue)
        //                    .Select(s => s.Velocity).ToArray());



          

        //    return GetZoneValues(fvActivity.Athlete.UserId, fvActivity.Start, stream, zoneType);

        //    //var zoneValues = GetUserZoneValues(fvActivity.Athlete.UserId, zoneType, fvActivity.Start);

        //    //// calculate number of seconds in each zone.
        //    //foreach(ZoneValueDto z in zoneValues)
        //    //    z.DurationInSeconds = stream.Where(w => w.Value >= z.StartValue && w.Value <= z.EndValue).Count();

        //    //return zoneValues;
        //}


        //internal int?[] GetSecondsPerMileFromVelocity(double?[] metrePerSecondStream)
        //{
        //    List<int> secsPerMile = new List<int>();

        //    foreach (double? s in metrePerSecondStream)
        //    {
        //        if (s != null && s > 0)
        //            secsPerMile.Add(Distance.MetrePerSecondToSecondPerMile(s.Value));
        //    }

        //    return secsPerMile.Select(s => (int?)s).ToArray();
        //}
    }
}
