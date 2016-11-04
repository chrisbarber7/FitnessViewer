using FitnessViewer.Infrastructure.enums;
using FitnessViewer.Infrastructure.Models;
using FitnessViewer.Infrastructure.Models.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Helpers
{
    /// <summary>
    /// Find peak values for a duration in a stream (ie 30 second peak power)
    /// </summary>
    public class PeakSeeker
    {
        private int[] _data;
        private int[] _durations;
        private PeakStreamType _streamType;
        private long _activityId;
  
  
        public static PeakSeeker Create(List<int> stream, PeakStreamType type, long activityId)
        {
            return new PeakSeeker(stream, type, activityId, false);
        }

        public static PeakSeeker Create(ActivityStreams stream, StreamType type)
        {
            var individualStream = stream.GetIndividualStream<int?>(type)
                .Select(s => s.Value)
                .ToList();

            PeakStreamType peakStream = type == StreamType.Watts ? PeakStreamType.Power :
                                        type == StreamType.Cadence ? PeakStreamType.Cadence : PeakStreamType.HeartRate;

            return new PeakSeeker(individualStream, peakStream, stream.ActivityId, false);
        }


        public static PeakSeeker CreatePowerCurve(ActivityStreams stream)
        {
            var individualStream = stream.GetIndividualStream<int?>( StreamType.Watts)
                .Select(s => s.Value)
                .ToList();

            return new PeakSeeker(individualStream, PeakStreamType.Power, stream.ActivityId, true);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream">Stream to be analysed for peaks.</param>
        /// <param name="type">Type of stream (power, HR, cadence)</param>
        /// <param name="activityId">Activity Id.</param>
        private PeakSeeker(List<int> stream, PeakStreamType type, long activityId, bool usePowerCurveDurations)
        {
            _streamType = type;
            _activityId = activityId;

            // for cadence peaks we ignore 0 values to exclude times when stationary so strip out now.
            _data = stream.Where(d => _streamType == PeakStreamType.Cadence ? d > 0 : true).ToArray();
            
            if (usePowerCurveDurations)
                _durations = PeakDuration.CreatePowerCurveDurations(_data.Count()).Durations;
            else
                _durations = PeakDuration.Create(_streamType).Durations;
        }

        public List<ActivityPeakDetail> FindPeaks()
        {
            return FindPeaks(null);
        }

        /// <summary>
        /// Find peaks in stream for all standard durations
        /// </summary>
        /// <returns></returns>
        public List<ActivityPeakDetail> FindPeaks(int? singleDuration)
        {
            List<ActivityPeakDetailCalculator> peaks = new List<ActivityPeakDetailCalculator>();

            // if running for a single duration override the standard durations.
            if (singleDuration != null)
                _durations =  new int[] { singleDuration.Value };
   
            foreach (int duration in _durations)
            {
                // default value to -1 so first value found will be higher and therefore used.
                peaks.Add(new ActivityPeakDetailCalculator(_activityId, _streamType, duration)
                {
                    Value = -1
                });
            }


            //   //  foreach (int l in _standardDurations)
            //   Parallel.ForEach(peaks, p =>
            //   {
            //       var xs = from n in Enumerable.Range(0, data.Length)
            //                let subseq = data.Skip(n).Take(p.Duration).Average()
            //                orderby subseq descending
            //                select new
            //                {
            //                    Start = n,
            //                    Duration =p.Duration,
            //                    Average = subseq
            //                };

            //       var results = xs.First();
            //       Console.WriteLine(results.Duration);
            //       Console.WriteLine(results.Average);
            //   });




            //// loop over the data for each possible starting point 
            //for (int startDataPoint = 0; startDataPoint <= dataCount; startDataPoint++)
            //{
            //    int remainingDataCount = dataCount - startDataPoint;

            //    // loop around each duration for which we have enough data left.
            //    Parallel.ForEach(peaks.Where(d => d.Duration <= remainingDataCount), p =>

            //      {
            //          double loopPeak = _data.Skip(startDataPoint).Take(p.Duration).Average();

            //          if (loopPeak > p.Value)
            //          {
            //              p.Value = (int)loopPeak;
            //              p.StartIndex = startDataPoint;
            //          }

            //      });
            //}

            int dataCount = _data.Length;

            // loop over the data for each possible starting point 
            for (int startDataPoint = 0; startDataPoint <= dataCount - 1; startDataPoint++)
            {
                Parallel.ForEach(peaks, c =>
               {
                   c.rollingValues.Enqueue(_data[startDataPoint]);

                   if (c.rollingValues.Count() < c.Duration)
                       return;

                   double loopPeak = c.rollingValues.Average();

                   if (loopPeak > c.Value)
                   {
                       c.Value = (int)loopPeak;
                       // we've moved past the start by the duration so need to adjust accordingly.
                       c.StartIndex = startDataPoint - c.Duration + 1;
                   }

                   c.rollingValues.Dequeue();
               });
            }

            // if we're running for a single duration then we don't need to include the 
            // peaks for the full full activity duration.
            if (singleDuration == null)
            {
                ActivityPeakDetailCalculator fullDuration = new ActivityPeakDetailCalculator(_activityId, _streamType, _data.Length)
                {
                    StartIndex = 0,
                    Value = (int)_data.Average()
                };

                // reset to int.maxValue now we've updated the startIndex so it's easy to find the full duration peaks.
                fullDuration.Duration = int.MaxValue;

                peaks.Add(fullDuration);
            }

            // where no peak found and value remains at the starting value of -1 reset to null
            // to indicate that no value was found.
            foreach (var peak in peaks.Where(p => p.Value == -1))
                peak.Value = null;

            return peaks.Select(p => AutoMapper.Mapper.Map<ActivityPeakDetail>(p)).ToList();
        }
    }
}