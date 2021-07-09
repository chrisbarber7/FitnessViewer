using System.Collections.Generic;
using FitnessViewer.Infrastructure.Core.enums;
using FitnessViewer.Infrastructure.Core.Models.Dto;

namespace FitnessViewer.Infrastructure.Core.Interfaces
{
    public interface ILapDtoRepository
    {
        IEnumerable<LapDto> GetLaps(long activityId);
        IEnumerable<LapDto> GetLapStream(long activityId, PeakStreamType streamType);
    }
}