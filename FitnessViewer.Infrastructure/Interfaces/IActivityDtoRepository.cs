using System;
using System.Collections.Generic;
using System.Linq;
using FitnessViewer.Infrastructure.Models.Dto;

namespace FitnessViewer.Infrastructure.Intefaces
{
    public interface IActivityDtoRepository
    {
        IEnumerable<ActivityDto> GetActivityDto(string userId);
        IEnumerable<ActivityDto> GetRecentActivity(string userId, int? returnedRows);
        IQueryable<ActivityDto> GetSportSummaryQuery(string userId, string sport, DateTime start, DateTime end);

         List<KeyValuePair<DateTime, int>> GetDailyTSS(string userId, string sport, DateTime start, DateTime end);
    }
}