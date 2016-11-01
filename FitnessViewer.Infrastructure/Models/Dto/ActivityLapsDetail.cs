﻿using AutoMapper;
using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.enums;
using FitnessViewer.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Models.Dto
{
    public class ActivityDetailDto : ActivityDto
    {

        public static ActivityDetailDto CreateFromActivity(UnitOfWork uow, Activity fvActivity)
        {
            ActivityDetailDto m = Mapper.Map<ActivityDetailDto>(ActivityDto.CreateFromActivity(fvActivity));

                m.Stream = uow.Activity.GetActivityStream(fvActivity.Id);
            
            m.Laps = uow.Activity.GetLaps(fvActivity.Id);
            m.HeartRate = uow.Activity.GetLapStream(fvActivity.Id, PeakStreamType.HeartRate);
            m.Cadence = uow.Activity.GetLapStream(fvActivity.Id, PeakStreamType.Cadence);
            m.SummaryInfo = uow.Activity.BuildSummaryInformation(fvActivity, m.Stream, 0, int.MaxValue);
            m.Analytics = m.SummaryInfo.Analytics;

            if (m.HasPowerMeter)
            {
                m.Power = uow.Activity.GetLapStream(fvActivity.Id, PeakStreamType.Power);
                m.PowerZones = uow.Settings.GetZoneValues(m, ZoneType.BikePower);
            }
            return m;
        }

        public ActivityMinMaxDto SummaryInfo { get; set; }
        public IEnumerable<LapDto> Laps { get; set; }
        public IEnumerable<LapDto> Power { get; set; }
        public IEnumerable<LapDto> HeartRate { get; set; }
        public IEnumerable<LapDto> Cadence { get; set; }

        public IEnumerable<ZoneValueDto> PowerZones { get; set; }
        public List<Stream> Stream { get; set; }

        public ActivityAnalyticsDto Analytics { get; set; }


    }
}