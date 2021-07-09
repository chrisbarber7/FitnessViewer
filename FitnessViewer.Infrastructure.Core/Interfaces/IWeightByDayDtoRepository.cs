using System;
using System.Collections.Generic;
using FitnessViewer.Infrastructure.Core.enums;
using FitnessViewer.Infrastructure.Core.Models.Dto;

namespace FitnessViewer.Infrastructure.Core.Interfaces
{
    public interface IWeightByDayDtoRepository
    {
        List<WeightByDayDto> GetMetricDetails(string userId, MetricType type, int days);
        List<WeightByDayDto> GetMetricDetails(string userId, MetricType type, DateTime from, DateTime to);
    }
}