using System.Collections.Generic;
using FitnessViewer.Infrastructure.Core.Models.Dto;
using System;

namespace FitnessViewer.Infrastructure.Core.Interfaces
{
    public interface IRunningTimesDtoRepository
    {
        IEnumerable<RunningTimesDto> GetBestTimes(string userId);
        IEnumerable<RunningTimesDto> GetBestTimes(string userId, DateTime start, DateTime end);
    }
}