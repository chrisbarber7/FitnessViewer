using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.enums;
using FitnessViewer.Infrastructure.Interfaces;
using FitnessViewer.Infrastructure.Models;
using FitnessViewer.Infrastructure.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Helpers
{
    public class UserZones
    {
        private string _userId;
        private IUnitOfWork _UnitOfWork;
        IEnumerable<ZoneRange> _userZones;

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

            // get a list of the zones for the user/type.
            //   var zoneRanges = GetUserZoneRanges(_userId, zone);

            // find the value of the zone (ftp/pace/hr/etc) on the given date
            int? ValueOnDate = _UnitOfWork.Settings.GetUserZones(_userId, zone)
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
            var zoneValues = _userZones.Where(z=>z.ZoneType == zone)
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
