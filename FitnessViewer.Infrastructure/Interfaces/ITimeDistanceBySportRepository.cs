using System;
using System.Collections.Generic;
using FitnessViewer.Infrastructure.Models.Dto;

namespace FitnessViewer.Infrastructure.Interfaces
{
    public interface ITimeDistanceBySportRepository
    {
        IEnumerable<TimeDistanceBySportDto> GetTimeDistanceBySport(string userId, DateTime start, DateTime end);
    }
}