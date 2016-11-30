using System;
using System.Collections.Generic;
using FitnessViewer.Infrastructure.Models.Dto;

namespace FitnessViewer.Infrastructure.Interfaces
{
    public interface ISportSummaryDtoRepository
    {
        SportSummaryDto GetSportSummary(string userId, string sport, DateTime start, DateTime end, List<ActivityDto> fullActivityList);
    }
}