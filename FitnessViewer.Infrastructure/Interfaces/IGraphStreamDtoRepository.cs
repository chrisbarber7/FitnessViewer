using FitnessViewer.Infrastructure.Models.Dto;

namespace FitnessViewer.Infrastructure.Interfaces
{
    public interface IGraphStreamDtoRepository
    {
        GraphStreamDto GetActivityStreams(long activityId);
        GraphStreamDto GetActivityStreams(long activityId, bool ignoreStep);
    }
}