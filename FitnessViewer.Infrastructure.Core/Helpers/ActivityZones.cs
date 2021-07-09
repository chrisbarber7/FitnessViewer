using FitnessViewer.Infrastructure.Core.Data;
using FitnessViewer.Infrastructure.Core.enums;
using FitnessViewer.Infrastructure.Core.Interfaces;
using FitnessViewer.Infrastructure.Core.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FitnessViewer.Infrastructure.Core.Helpers
{
    public class ActivityZones
    {

        private ActivityDetailDto _activity;
        private IUnitOfWork _unitOfWork;
        private UserZones _userZones;

        public ActivityZones(ActivityDetailDto activity)
        {
            _activity = activity;
            _unitOfWork = new UnitOfWork();
            _userZones = new UserZones(activity.Athlete.UserId);
        }


        public IEnumerable<ZoneValueDto> GetZoneValues(ZoneType zoneType)
        {
            int?[] stream = null;

            if (zoneType == ZoneType.BikePower)
                stream = _activity.ActivityStream.GetIndividualStream<int?>(StreamType.Watts)
                            // .Where(s => s.Watts.HasValue)
                            //  .Select(s => s.Watts)
                            .ToArray();
            else if (zoneType == ZoneType.RunHeartRate || zoneType == ZoneType.BikeHeartRate)
                stream = _activity.ActivityStream.GetIndividualStream<int?>(StreamType.Heartrate)
                            // .Where(s => s.HeartRate.HasValue)
                            // .Select(s => s.HeartRate)
                            .ToArray();

            
            else if (zoneType == ZoneType.RunPace)
                stream = _activity.ActivityStream.GetSecondsPerMile();

            if (stream.Contains(null))
                return null;

            var zoneValues = _userZones.GetUserZoneValues(zoneType, _activity.Start).Where(z => z.ZoneType == zoneType);

            decimal totalDuration = stream.Count();
            decimal highestPercentage = 0;

            // calculate number of seconds in each zone,keep running total and store highest percentage
            foreach (ZoneValueDto z in zoneValues)
            {
                z.DurationInSeconds = stream.Where(w => w.Value >= z.StartValue && w.Value <= z.EndValue).Count();
                z.PercentageInZone = Math.Round(totalDuration == 0 ? 0.00M : Convert.ToDecimal(z.DurationInSeconds / totalDuration) * 100, 2);
                highestPercentage = z.PercentageInZone > highestPercentage ? z.PercentageInZone : highestPercentage;
            }

            // scale each percentage so that highest takes up 100% and other scale in proportion
            foreach (ZoneValueDto z in zoneValues)
                if (highestPercentage > 0)
                     z.DisplayPercentage = z.PercentageInZone / highestPercentage * 100;


            // for run pacing we need to reverse the results so we get the higher/slower times at the top of the list.
            if (zoneType == ZoneType.RunPace)
                return zoneValues.OrderByDescending(z => z.StartValue);

            return zoneValues;
        }
    }
}
