using AutoMapper;
using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Models.Dto
{
    public class ActivityLapsDto : ActivityDto
    {

        public static ActivityLapsDto CreateFromActivity(UnitOfWork uow, Activity fvActivity)
        {
            ActivityLapsDto m = Mapper.Map<ActivityLapsDto>(ActivityDto.CreateFromActivity(fvActivity));

            m.Laps = uow.Activity.GetLaps(fvActivity.Id);
            m.Power = uow.Activity.GetLapStream(fvActivity.Id, PeakStreamType.Power);
            m.HeartRate = uow.Activity.GetLapStream(fvActivity.Id, PeakStreamType.HeartRate);
            m.Cadence = uow.Activity.GetLapStream(fvActivity.Id, PeakStreamType.Cadence);
            m.SummaryInfo = uow.Activity.BuildSummaryInformation(fvActivity.Id, 0, int.MaxValue);
            return m;
        }


        public MinMaxDto SummaryInfo { get; set; }
        public IEnumerable<LapDto> Laps { get; set; }
        public IEnumerable<LapDto> Power { get; set; }
        public IEnumerable<LapDto> HeartRate { get; set; }
        public IEnumerable<LapDto> Cadence { get; set; }


    }
}
