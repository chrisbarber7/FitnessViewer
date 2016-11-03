using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.enums;
using FitnessViewer.Infrastructure.Helpers;
using FitnessViewer.Infrastructure.Models;
using FitnessViewer.Infrastructure.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public List<Zone> GetUserZones(string userId, ZoneType zone)
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
        public List<ZoneRange> GetUserZoneRanges(string userId, ZoneType zone)
        {
            var zones = _context.ZoneRange
                  .Where(z => z.UserId == userId && z.ZoneType == zone)
                  .OrderBy(z => z.ZoneStart)
                  .ToList();

            if (zones.Count > 0)
                return zones;
            else
                return new List<ZoneRange>() { ZoneRange.CreateDefault() };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">ASP.NET Identity id</param>
        /// <param name="zone">Zone Type (Bike, Run, Swim - HeartRate/Pace/etc)</param>
        /// <param name="date">Activity date for which to return zones</param>
        /// <returns>Zones for a given user/type on a given date</returns>
        public List<ZoneValueDto> GetUserZoneValues(string userId, ZoneType zone, DateTime date)
        {
            // remove the time.
            date = date.Date;

            // get a list of the zones for the user/type.
            var zoneRanges = GetUserZoneRanges(userId, zone);

            // find the value of the zone (ftp/pace/hr/etc) on the given date
            int? ValueOnDate = GetUserZones(userId, zone)
                .Where(z => z.StartDate <= date)
                .Select(z => z.Value)
                .FirstOrDefault();

            // if no value value found for the given user/zone/date then return a single zone.
            if (ValueOnDate == null)
                return new List<ZoneValueDto>()
                {
                    ZoneValueDto.CreateDefault(zone)
                };

            // populate zones with the start value.
            var zoneValues = zoneRanges
                .Select(r => new ZoneValueDto
                {
                    ZoneType = zone,
                    ZoneName = r.ZoneName,
                    StartValue = r.ZoneStart * ValueOnDate.Value / 100

                })
                .OrderBy(z => z.StartValue)
                .ToList();


            if (zoneValues.Count > 1)
            {
                // calculate the EndValue for the zone based on the start value of the next zone up.
                for (int z = 0; z <= zoneValues.Count - 2; z++)
                    zoneValues[z].EndValue = zoneValues[z + 1].StartValue - 1;
            }

            else if (zoneValues.Count == 1)
            {
                // for the last zone the max value will have no upper limit.
                zoneValues[zoneValues.Count - 1].EndValue = int.MaxValue;
            }

            return zoneValues;
        }

        /// <summary>
        /// Calculate time spent in each zone.
        /// </summary>
        /// <param name="fvActivity">FitnessViewer Activity details</param>
        /// <param name="zoneType">Zone type to return</param>
        /// <returns></returns>
        public IEnumerable<ZoneValueDto> GetZoneValues(Activity fvActivity, ZoneType zoneType)
        {
            int?[] stream = null;

            if (zoneType == ZoneType.BikePower)
                stream = _context.Stream
                            .Where(s => s.ActivityId == fvActivity.Id && s.Watts.HasValue)
                            .Select(s => s.Watts)
                            .ToArray();
            else if (zoneType == ZoneType.RunHeartRate || zoneType == ZoneType.BikeHeartRate)
                stream = _context.Stream
                            .Where(s => s.ActivityId == fvActivity.Id && s.HeartRate.HasValue)
                            .Select(s => s.HeartRate)
                            .ToArray();
            else if (zoneType == ZoneType.RunPace)
                stream = GetSecondsPerMileFromVelocity(
                    _context.Stream
                            .Where(s => s.ActivityId == fvActivity.Id && s.Velocity.HasValue)
                            .Select(s => s.Velocity).ToArray());



          

            return GetZoneValues(fvActivity.Athlete.UserId, fvActivity.Start, stream, zoneType);

            //var zoneValues = GetUserZoneValues(fvActivity.Athlete.UserId, zoneType, fvActivity.Start);

            //// calculate number of seconds in each zone.
            //foreach(ZoneValueDto z in zoneValues)
            //    z.DurationInSeconds = stream.Where(w => w.Value >= z.StartValue && w.Value <= z.EndValue).Count();

            //return zoneValues;
        }

        public IEnumerable<ZoneValueDto> GetZoneValues(ActivityDetailDto activity, ZoneType zoneType)
        {
            int?[] stream = null;

            if (zoneType == ZoneType.BikePower)
                stream = activity.Stream
                            .Where(s => s.Watts.HasValue)
                            .Select(s => s.Watts)
                            .ToArray();
            else if (zoneType == ZoneType.RunHeartRate || zoneType == ZoneType.BikeHeartRate)
                stream = activity.Stream
                            .Where(s => s.HeartRate.HasValue)
                            .Select(s => s.HeartRate)
                            .ToArray();
            else if (zoneType == ZoneType.RunPace)
                stream = GetSecondsPerMileFromVelocity(activity.Stream);

            return GetZoneValues(activity.Athlete.UserId, activity.Start, stream, zoneType);
        }


        internal IEnumerable<ZoneValueDto> GetZoneValues(string userId, DateTime activityDate, int?[] stream, ZoneType zone)
        {
            var zoneValues = GetUserZoneValues(userId, zone, activityDate);

            // calculate number of seconds in each zone.
            foreach (ZoneValueDto z in zoneValues)
                z.DurationInSeconds = stream.Where(w => w.Value >= z.StartValue && w.Value <= z.EndValue).Count();

            return zoneValues;
        }

        internal int?[] GetSecondsPerMileFromVelocity(IEnumerable<Stream> stream)
        {
            List<int> secsPerMile = new List<int>();

            foreach (Stream s in stream)
            {
                if (s.Velocity.HasValue && s.Velocity.Value > 0)
                    secsPerMile.Add(MetreDistance.MetrePerSecondToSecondPerMile(s.Velocity.Value));
            }

            return secsPerMile.Select(s => (int?)s).ToArray();
        }

        internal int?[] GetSecondsPerMileFromVelocity(double?[] metrePerSecondStream)
        {
            List<int> secsPerMile = new List<int>();

            foreach (double? s in metrePerSecondStream)
            {
                if (s != null && s > 0)
                    secsPerMile.Add(MetreDistance.MetrePerSecondToSecondPerMile(s.Value));
            }

            return secsPerMile.Select(s => (int?)s).ToArray();
        }
    }
}
