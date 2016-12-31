using System;
using System.Collections.Generic;
using FitnessViewer.Infrastructure.Models.Dto;
using FitnessViewer.Infrastructure.enums;

namespace FitnessViewer.Infrastructure.Interfaces
{
    public interface IPeriodDtoRepository
    {
        IEnumerable<PeriodDto> ActivityByWeek(string userId, SportType sport, DateTime start, DateTime end);
        IEnumerable<ActivityPeaksPeriodDto> PeaksByMonth(string userId, DateTime start, DateTime end);
    }
}