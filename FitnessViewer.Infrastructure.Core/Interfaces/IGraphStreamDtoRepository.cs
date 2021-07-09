using FitnessViewer.Infrastructure.Core.Models.Dto;

namespace FitnessViewer.Infrastructure.Core.Interfaces
{
    public interface IGraphStreamDtoRepository
    {
        GraphStreamDto GetActivityStreams(long activityId);
        GraphStreamDto GetActivityStreams(long activityId, bool ignoreStep);
    }
}