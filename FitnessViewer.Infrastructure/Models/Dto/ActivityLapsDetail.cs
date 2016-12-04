using AutoMapper;
using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.enums;
using FitnessViewer.Infrastructure.Helpers;
using FitnessViewer.Infrastructure.Helpers.Analytics;
using FitnessViewer.Infrastructure.Models.Collections;
using FitnessViewer.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Models.Dto
{
    public class ActivityDetailDto : ActivityDto
    {

        public static new ActivityDetailDto CreateFromActivity(Activity fvActivity)
        {
            LapDtoRepository repo = new LapDtoRepository();
            ActivityDetailDto m = Mapper.Map<ActivityDetailDto>(ActivityDto.CreateFromActivity(fvActivity));

            m.ActivityStream = ActivityStreams.CreateFromExistingActivityStream(fvActivity.Id);// uow.Activity.GetActivityStream(fvActivity.Id);
            
            m.Laps = repo.GetLaps(fvActivity.Id);
            m.HeartRate = repo.GetLapStream(fvActivity.Id, PeakStreamType.HeartRate);
            m.Cadence = repo.GetLapStream(fvActivity.Id, PeakStreamType.Cadence);

            ActivityMinMaxDto mma = new ActivityMinMaxDto(m.ActivityStream);

            mma.Populate();
            m.SummaryInfo = mma;

            m.Analytics = m.SummaryInfo.Analytics;

            ActivityZones zones = new ActivityZones(m);

            if (m.IsRun)
            {
                m.HeartRateZones = zones.GetZoneValues(ZoneType.RunHeartRate);
                m.RunPaceZones = zones.GetZoneValues(ZoneType.RunPace);
                m.PaceByDistance = repo.GetBestEffort(fvActivity.Id);

            }
            else if (m.IsRide)
            {

                m.HeartRateZones = zones.GetZoneValues(ZoneType.BikeHeartRate);

                if (m.HasPowerMeter)
                {
                    m.Power = repo.GetLapStream(fvActivity.Id, PeakStreamType.Power);
                    m.PowerZones = zones.GetZoneValues(ZoneType.BikePower);  // uow.Settings.GetZoneValues(m, ZoneType.BikePower);
                }
            }
            else if (m.IsSwim)
            {

            }


            return m;
        }

        public static ActivityDetailDto CreateForActivityWithNoDetails(Activity fvActivity)
        {
            ActivityDetailDto dto = ActivityDetailDto.CreateFromActivity(fvActivity);
            dto.Name = fvActivity.Name;
           

            dto.Analytics = ActivityAnalytics.EmptyStream();

            return dto;

        }

        public ActivityMinMaxDto SummaryInfo { get; set; }
        public IEnumerable<LapDto> Laps { get; set; }
        public IEnumerable<LapDto> Power { get; set; }
        public IEnumerable<LapDto> HeartRate { get; set; }
        public IEnumerable<LapDto> Cadence { get; set; }
        public IEnumerable<LapDto> PaceByDistance { get; set; }
            
   

        public IEnumerable<ZoneValueDto> PowerZones { get; set; }
        public ActivityStreams ActivityStream { get; set; }

        public IEnumerable<ZoneValueDto> HeartRateZones { get; set; }

        public IEnumerable<ZoneValueDto> RunPaceZones { get; set; }
        public ActivityAnalytics Analytics { get; set; }


    }
}
