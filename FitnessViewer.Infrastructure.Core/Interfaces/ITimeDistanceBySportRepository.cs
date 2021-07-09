using System;
using System.Collections.Generic;
using FitnessViewer.Infrastructure.Core.Models.Dto;

namespace FitnessViewer.Infrastructure.Core.Interfaces
{
    public interface ITimeDistanceBySportRepository
    {
        IEnumerable<TimeDistanceBySportDto> GetTimeDistanceBySport(string userId, DateTime start, DateTime end);
    }
}