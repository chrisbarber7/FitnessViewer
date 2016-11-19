using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.Models.Dto;
using System.Linq;
using System.Data.Entity;

namespace FitnessViewer.Infrastructure.Repository
{
    public class GraphStreamDtoRepository : DtoRepository
    {
        public GraphStreamDtoRepository() : base()
        {
        }

        public GraphStreamDto GetActivityStreams(long activityId)
        {
            // by default unless told otherwise by parameter we'll use the steps set-up with the activity (so pass false).
            return GetActivityStreams(activityId, false);
        }


        public GraphStreamDto GetActivityStreams(long activityId, bool ignoreStep)
        {



            var activityStream = _context.Stream
                 .Include(a => a.Activity)
                 .Where(s => s.ActivityId == activityId && (ignoreStep ? true : s.Time % s.Activity.StreamStep == 0))
                 .Select(s => new
                 {
                     Time = s.Time,
                     Altitude = s.Altitude,
                     HeartRate = s.HeartRate,
                     Cadence = s.Cadence,
                     Watts = s.Watts
                 })
                  .OrderBy(s => s.Time)
                 .ToList();

            GraphStreamDto result = new GraphStreamDto();

            foreach (var s in activityStream)
            {
                result.Time.Add(s.Time);
                //    result.Distance.Add(s.Distance);
                result.Altitude.Add(s.Altitude);
                //      result.Velocity.Add(s.Velocity);
                result.HeartRate.Add(s.HeartRate);
                result.Cadence.Add(s.Cadence);
                result.Watts.Add(s.Watts);
            }

            return result;
        }
    }
}
