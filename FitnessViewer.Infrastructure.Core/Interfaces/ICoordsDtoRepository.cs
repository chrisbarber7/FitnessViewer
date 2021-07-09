using System.Collections.Generic;
using FitnessViewer.Infrastructure.Core.Models.Dto;

namespace FitnessViewer.Infrastructure.Core.Interfaces
{
    public interface ICoordsDtoRepository
    {
        IEnumerable<CoordsDto> GetActivityCoords(long activityId);
    }
}