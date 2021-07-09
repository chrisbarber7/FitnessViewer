using FitnessViewer.Infrastructure.Core.Data;
using FitnessViewer.Infrastructure.Core.enums;
using FitnessViewer.Infrastructure.Core.Helpers.Analytics;
using FitnessViewer.Infrastructure.Core.Interfaces;
using FitnessViewer.Infrastructure.Core.Models;
using FitnessViewer.Infrastructure.Core.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Core.Helpers
{
    public class UserZones
    {
        private string _userId;
        private IUnitOfWork _UnitOfWork;
        IEnumerable<ZoneRange> _userZones;

        // default value for each zone!
        public static readonly Dictionary<ZoneType, int> ZoneTypeDefaultValues = new Dictionary<ZoneType, int>()
        {
            { ZoneType.BikeHeartRate, 170 },
            { ZoneType.BikePower, 200 },
            { ZoneType.RunHeartRate, 170 },
            { ZoneType.RunPace, 480 },
            { ZoneType.SwimPace,120 }
        };

        // default value for each zone!
        public static readonly Dictionary<ZoneType, List<ZoneRange>> ZoneTypeDefaultZones = new Dictionary<ZoneType, List<ZoneRange>>()
        {
            {
                ZoneType.BikePower, new List<ZoneRange>()
                {
                    new ZoneRange() {ZoneType= ZoneType.BikePower, ZoneName="1 - Recovery", ZoneStart=0 },
                    new ZoneRange() {ZoneType= ZoneType.BikePower, ZoneName="2 - Endurance", ZoneStart=55},
                    new ZoneRange() {ZoneType= ZoneType.BikePower, ZoneName="3 - Tempo", ZoneStart=75},
                    new ZoneRange() {ZoneType= ZoneType.BikePower, ZoneName="4 - Threshold", ZoneStart=90},
                    new ZoneRange() {ZoneType= ZoneType.BikePower, ZoneName="5 - VO2 Max", ZoneStart=105},
                    new ZoneRange() {ZoneType= ZoneType.BikePower, ZoneName="6 - Anaerobic", ZoneStart=120},
                    new ZoneRange() {ZoneType= ZoneType.BikePower, ZoneName="7 - Neuromuscualar", ZoneStart=200}
                }
            },
                {
            ZoneType.BikeHeartRate, new List<ZoneRange>()
                {
                    new ZoneRange() {ZoneType= ZoneType.BikeHeartRate, ZoneName="Zone 1", ZoneStart=0 },
                    new ZoneRange() {ZoneType= ZoneType.BikeHeartRate, ZoneName="Zone 2", ZoneStart=81},
                    new ZoneRange() {ZoneType= ZoneType.BikeHeartRate, ZoneName="Zone 3", ZoneStart=90},
                    new ZoneRange() {ZoneType= ZoneType.BikeHeartRate, ZoneName="Zone 4", ZoneStart=94},
                    new ZoneRange() {ZoneType= ZoneType.BikeHeartRate, ZoneName="Zone 5", ZoneStart=100},
                    new ZoneRange() {ZoneType= ZoneType.BikeHeartRate, ZoneName="Zone 5b", ZoneStart=103},
                    new ZoneRange() {ZoneType= ZoneType.BikeHeartRate, ZoneName="Zone 5c", ZoneStart=106}
                }
            },
         {
            ZoneType.RunPace, new List<ZoneRange>()
            {
                    new ZoneRange() {ZoneType= ZoneType.RunPace, ZoneName="Zone 7", ZoneStart=  1},
                    new ZoneRange() {ZoneType= ZoneType.RunPace, ZoneName="Zone ", ZoneStart=  87},
                    new ZoneRange() {ZoneType= ZoneType.RunPace, ZoneName="Zone 5", ZoneStart=  94},
                    new ZoneRange() {ZoneType= ZoneType.RunPace, ZoneName="Zone 4", ZoneStart=  99},
                    new ZoneRange() {ZoneType= ZoneType.RunPace, ZoneName="Zone 3", ZoneStart=  103},
                    new ZoneRange() {ZoneType= ZoneType.RunPace, ZoneName="Zone 2", ZoneStart=  112},
                    new ZoneRange() {ZoneType= ZoneType.RunPace, ZoneName="Zone 1", ZoneStart=  126}
            }
            },
         {
            ZoneType.RunHeartRate, new List<ZoneRange>()
            {
                    new ZoneRange() {ZoneType= ZoneType.RunHeartRate, ZoneName="Zone 1", ZoneStart=   0},
                    new ZoneRange() {ZoneType= ZoneType.RunHeartRate, ZoneName="Zone 2", ZoneStart=   85},
                    new ZoneRange() {ZoneType= ZoneType.RunHeartRate, ZoneName="Zone 3", ZoneStart=   90},
                    new ZoneRange() {ZoneType= ZoneType.RunHeartRate, ZoneName="Zone 4", ZoneStart=   95},
                    new ZoneRange() {ZoneType= ZoneType.RunHeartRate, ZoneName="Zone 5", ZoneStart=   100},
                    new ZoneRange() {ZoneType= ZoneType.RunHeartRate, ZoneName="Zone 5b", ZoneStart=  103},
                    new ZoneRange() {ZoneType= ZoneType.RunHeartRate, ZoneName="Zone 5c", ZoneStart=  106}
                }
            },


            {
         ZoneType.SwimPace, new List<ZoneRange>()
         
         }
        };

        public UserZones(string userId)
        {
            _userId = userId;
            _UnitOfWork = new UnitOfWork();
            _userZones = _UnitOfWork.Settings.GetUserZoneRanges(userId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="zone">Zone Type (Bike, Run, Swim - HeartRate/Pace/etc)</param>
        /// <param name="date">Activity date for which to return zones</param>
        /// <returns>Zones for a given user/type on a given date</returns>
        public List<ZoneValueDto> GetUserZoneValues(ZoneType zone, DateTime date)
        {
            // remove the time.
            date = date.Date;

            ZoneValueOnDay zoneValueDate = new ZoneValueOnDay();
            int? ValueOnDate = zoneValueDate.GetUserZoneValueOnGivenDate(_userId, zone, date);

            // if no value value found for the given user/zone/date then return a single zone.
            if (ValueOnDate == null)
                return new List<ZoneValueDto>()
                {
                    ZoneValueDto.CreateDefault(zone)
                };

            // populate zones with the start value.
            var zoneValues = _userZones.Where(z => z.ZoneType == zone)
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
    }
}
