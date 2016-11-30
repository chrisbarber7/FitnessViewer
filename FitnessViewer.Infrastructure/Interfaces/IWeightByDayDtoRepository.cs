using System;
using System.Collections.Generic;
using FitnessViewer.Infrastructure.enums;
using FitnessViewer.Infrastructure.Models.Dto;

namespace FitnessViewer.Infrastructure.Interfaces
{
    public interface IWeightByDayDtoRepository
    {
        List<WeightByDayDto> GetMetricDetails(string userId, MetricType type, int days);
        List<WeightByDayDto> GetMetricDetails(string userId, MetricType type, DateTime from, DateTime to);
    }
}