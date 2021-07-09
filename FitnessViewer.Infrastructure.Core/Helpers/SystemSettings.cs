using FitnessViewer.Infrastructure.Core.Data;
using FitnessViewer.Infrastructure.Core.enums;
using FitnessViewer.Infrastructure.Core.Interfaces;
using FitnessViewer.Infrastructure.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitnessViewer.Infrastructure.Core.Models;

namespace FitnessViewer.Infrastructure.Core.Helpers
{
    public class SystemSettings
    {
        private readonly IUnitOfWork _unitOfWork;

        public string UserId { get; set; }
        public int? Ftp { get; set; }

        public bool ValueAdded { get; private set; }
        public bool ZoneAdded { get; private set; }

        public static SystemSettings CheckSettings(string userId, int? ftp)
        {
           


            SystemSettings settings = new SystemSettings();
            settings.UserId = userId;
            settings.Ftp = ftp;

            settings.CheckValues();
            settings.CheckZones();
            settings.CheckSettings();

            return settings;
        }

        private void CheckSettings()
        {
            AthleteSetting setting = _unitOfWork.CRUDRepository.GetByUserId<AthleteSetting>(UserId).FirstOrDefault();

            if (setting != null)
                return;

            Athlete fvAthlete = _unitOfWork.CRUDRepository.GetByUserId<Athlete>(UserId).FirstOrDefault();

            if (fvAthlete == null)
                return;

            setting = AthleteSetting.DefaultSettings(fvAthlete.Id, UserId);

            _unitOfWork.CRUDRepository.Add<AthleteSetting>(setting);
            _unitOfWork.Complete();
        }

        public SystemSettings()
        {
            _unitOfWork = new UnitOfWork();
        }

        private void CheckZones()
        {
            var currentZones = _unitOfWork.Settings.GetUserZoneRanges(UserId);

            CheckZones(currentZones, ZoneType.BikePower);
            CheckZones(currentZones, ZoneType.BikeHeartRate);
            CheckZones(currentZones, ZoneType.RunHeartRate);
            CheckZones(currentZones, ZoneType.RunPace);
            CheckZones(currentZones, ZoneType.SwimPace);
        }

        private void CheckZones(List<ZoneRange> currentZones, ZoneType zone)
        {
            if (currentZones.Any(a => a.ZoneType == zone))
                return;

            foreach (ZoneRange z in UserZones.ZoneTypeDefaultZones[zone])
            {
                ZoneRange newZoneRange = new ZoneRange();
                newZoneRange.UserId = UserId;
                newZoneRange.ZoneName = z.ZoneName;
                newZoneRange.ZoneStart = z.ZoneStart;
                newZoneRange.ZoneType = zone;
                _unitOfWork.CRUDRepository.Add<ZoneRange>(newZoneRange);

            }
            _unitOfWork.Complete();
            ZoneAdded = true;
        }

        private void CheckValues()
        {
            var currentValues = _unitOfWork.Settings.GetUserZones(UserId);

            CheckValues(currentValues, ZoneType.BikePower);
            CheckValues(currentValues, ZoneType.BikeHeartRate);
            CheckValues(currentValues, ZoneType.RunHeartRate);
            CheckValues(currentValues, ZoneType.RunPace);
            CheckValues(currentValues, ZoneType.SwimPace);
        }

        private void CheckValues(IEnumerable<Zone> currentValues, ZoneType zone)
        {
            if (currentValues.Any(a => a.ZoneType == zone))
                return;

            Zone newZone = new Zone();
            newZone.ZoneType = zone;
            newZone.UserId = UserId;
            newZone.StartDate = new DateTime(2000, 1, 1);
            newZone.Value = UserZones.ZoneTypeDefaultValues[zone];

            _unitOfWork.CRUDRepository.Add<Zone>(newZone);
            _unitOfWork.Complete();

            ValueAdded = true;
        }
    }
}
