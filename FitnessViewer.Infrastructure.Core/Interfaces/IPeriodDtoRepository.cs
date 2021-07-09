using System;
using System.Collections.Generic;
using FitnessViewer.Infrastructure.Core.Models.Dto;
using FitnessViewer.Infrastructure.Core.enums;

namespace FitnessViewer.Infrastructure.Core.Interfaces
{
    public interface IPeriodDtoRepository
    {
        IEnumerable<PeriodDto> ActivityByWeek(string userId, SportType sport, DateTime start, DateTime end);
        IEnumerable<ActivityPeaksPeriodDto> PeaksByMonth(string userId, DateTime start, DateTime end);
        IEnumerable<PowerCurveDto> PowerCurve(string userId, DateTime start, DateTime end);
    }
}