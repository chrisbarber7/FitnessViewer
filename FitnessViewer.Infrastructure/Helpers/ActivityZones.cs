using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.enums;
using FitnessViewer.Infrastructure.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Helpers
{
    public class ActivityZones
    {

        private ActivityDetailDto _activity;
        private UnitOfWork _unitOfWork;
        private UserZones _userZones;

        public ActivityZones(ActivityDetailDto activity)
            {
            _activity = activity;
            _unitOfWork = new UnitOfWork();
            _userZones = new UserZones(activity.Athlete.UserId);
        }


        public IEnumerable<ZoneValueDto> GetZoneValues( ZoneType zoneType)
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
                stream = _activity.ActivityStream.GetSecondsPerMileFromVelocity();

            var zoneValues = _userZones.GetUserZoneValues( zoneType, _activity.Start).Where(z=>z.ZoneType == zoneType);

            // calculate number of seconds in each zone.
            foreach (ZoneValueDto z in zoneValues)
                z.DurationInSeconds = stream.Where(w => w.Value >= z.StartValue && w.Value <= z.EndValue).Count();

            return zoneValues;
        }

    }
}
