using System.Collections.Generic;
using FitnessViewer.Infrastructure.Models.Dto;
using System;

namespace FitnessViewer.Infrastructure.Interfaces
{
    public interface IRunningTimesDtoRepository
    {
        IEnumerable<RunningTimesDto> GetBestTimes(string userId);
        IEnumerable<RunningTimesDto> GetBestTimes(string userId, DateTime start, DateTime end);
    }
}