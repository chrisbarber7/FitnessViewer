using System.Collections.Generic;
using FitnessViewer.Infrastructure.enums;
using FitnessViewer.Infrastructure.Models.Dto;

namespace FitnessViewer.Infrastructure.Interfaces
{
    public interface ILapDtoRepository
    {
        IEnumerable<LapDto> GetLaps(long activityId);
        IEnumerable<LapDto> GetLapStream(long activityId, PeakStreamType streamType);
    }
}