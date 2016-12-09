using System;
using System.Collections.Generic;
using System.Linq;
using FitnessViewer.Infrastructure.Models.Dto;
using FitnessViewer.Infrastructure.Helpers;
using FitnessViewer.Infrastructure.enums;

namespace FitnessViewer.Infrastructure.Intefaces
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