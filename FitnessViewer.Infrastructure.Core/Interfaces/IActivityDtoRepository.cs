using System;
using System.Collections.Generic;
using System.Linq;
using FitnessViewer.Infrastructure.Core.Models.Dto;
using FitnessViewer.Infrastructure.Core.Helpers;
using FitnessViewer.Infrastructure.Core.enums;

namespace FitnessViewer.Infrastructure.Core.Interfaces
{
    public interface IActivityDtoRepository
    {
        IEnumerable<ActivityDto> GetActivityDto(string userId);
        IEnumerable<ActivityDto> GetRecentActivity(string userId, int? returnedRows);
        IQueryable<ActivityDto> GetSportSummaryQuery(string userId, SportType sport, DateTime start, DateTime end);

        List<KeyValuePair<DateTime, decimal>> GetDailyTSS(string userId, SportType sport, DateTime start, DateTime end);

        List<YearlyDetailsDayInfo> GetYearToDateInfo(string userId, int? year=null);
    }
}