using System;
using System.Collections.Generic;
using FitnessViewer.Infrastructure.Models.Dto;

namespace FitnessViewer.Infrastructure.Interfaces
{
    public interface IPeriodDtoRepository
    {
        IEnumerable<PeriodDto> ActivityByWeek(string userId, string activityType, DateTime start, DateTime end);
    }
}