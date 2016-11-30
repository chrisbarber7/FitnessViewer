using System.Collections.Generic;
using FitnessViewer.Infrastructure.Models.Dto;

namespace FitnessViewer.Infrastructure.Interfaces
{
    public interface IRunningTimesDtoRepository
    {
        IEnumerable<RunningTimesDto> GetBestTimes(string userId);
    }
}