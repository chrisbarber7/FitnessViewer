using FitnessViewer.Infrastructure.enums;
using FitnessViewer.Infrastructure.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Helpers
{
    /// <summary>
    /// Find peak values for a duration in a stream (ie 30 second peak power)
    /// </summary>
    public class PeakValueFinder
    {
        private List<int> _data;
        private int[] _standardDurations;
        private PeakStreamType _streamType;
        private long _activityId;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream">Stream to be analysed for peaks.</param>
        /// <param name="type">Type of stream (power, HR, cadence)</param>
        /// <param name="activityId">Activity Id.</param>
        public PeakValueFinder(List<int> stream, PeakStreamType type, long activityId)
        {
            _streamType = type;
            _activityId = activityId;
            
            // for cadence peaks we ignore 0 values to exclude times when stationary so strip out now.
            _data = stream.Where(d => _streamType == PeakStreamType.Cadence ? d > 0 : true).ToList();
           
            // set standard reporting durations (in seconds)
            switch (type)
            {
                case PeakStreamType.Power:
                    {
                 _standardDurations = new int[] { 5, 10, 30, 60, 120, 300, 360, 600, 720, 1200, 1800, 3600, int.MaxValue };
                        break;
                    }
                case PeakStreamType.HeartRate:
                    {
                        _standardDurations = new int[] { 60, 120, 300, 360, 600, 720, 1200, 1800, 3600, int.MaxValue };
                        break;
                    }
                case PeakStreamType.Cadence:
                    {
                        _standardDurations = new int[] { 5, 10, 30, 60, 120, 300, 360, 600, 720, 1200, 1800, 3600, int.MaxValue };
                        break;
                    };
            }
        }

        public void SetupPowerCurveDurations()
        {
            List<int> durations = new List<int>();

            // under 3 minutes = every second
            for (int x = 1; x <= 179; x++)
                durations.Add(x);

            // 3 mins to 5 mins = every 2 seconds
            for (int x = 180; x <= 299; x = x + 2)
                durations.Add(x);

            // 5 mins to 30 mins = every 5 seconds
            for (int x = 300; x <= (30 * 60) - 1; x = x + 5)
                durations.Add(x);

            // 30 mins to 1 hour = every 30 seconds.
            for (int x = (30 * 60); x <= (60 * 60) - 1; x = x + 30)
                durations.Add(x);

            // one hour to two hours = every 60 seconds.
            for (int x = (60 * 60); x <= (2 * 60 * 60) - 1; x = x + 60)
                durations.Add(x);

            // anything over every 5 mins
            for (int x = (2 * 60 * 60); x <= (3 * 60 * 60) - 1; x = x + 300)
                durations.Add(x);
            
            durations.Add(int.MaxValue);

            // include any duration less than event time (and int.MaxValue for full event).
            _standardDurations = durations.Where(d=>d <= _data.Count() || d == int.MaxValue).ToArray();
        }


        public static List<ActivityPeakDetail> ExtractPeaksFromStream(long activityId, List<int?> stream, PeakStreamType type)
        {
            if (stream.Contains(null))
                return null;


            PeakValueFinder finder = new PeakValueFinder(
                stream.Select(s => s.Value).ToList(),
                type,
                activityId);

            return finder.FindPeaks();
        }


        /// <summary>
        /// Find peaks in stream for all standard durations
        /// </summary>
        /// <returns></returns>
        public List<ActivityPeakDetail> FindPeaks()
        {
            List<ActivityPeakDetail> peaks = new List<ActivityPeakDetail>();

            foreach (int duration in _standardDurations)
            {
                ActivityPeakDetail d = new ActivityPeakDetail(_activityId, _streamType, duration);

                // if full duration peak then just calculate the average now and it'll be skipped out of the main loop.
                if (d.Duration == int.MaxValue)
                {
                    d.StartIndex = 0; 
                    d.Value = (int)_data.Average();
                }
                  
                peaks.Add(d);
            }

            int dataCount = _data.Count;

            // loop over the data for each possible starting point 
            for (int startDataPoint = 0; startDataPoint <= dataCount; startDataPoint++)
            {
                int remainingDataCount = dataCount - startDataPoint;

                // loop around each duration for which we have enough data left.
                Parallel.ForEach(peaks.Where(d=>d.Duration <= remainingDataCount), p =>
                {
                    int loopPeak = (int)_data.Skip(startDataPoint).Take(p.Duration).Average();

                    if (p.Value == null || loopPeak > p.Value)
                    {
                        p.Value = loopPeak;
                        p.StartIndex = startDataPoint;
                    }
                });
            }       

            return peaks;
        }

        public ActivityPeakDetail FindPeakForDuration(int duration)
        {
            // override default duration as we're only interested in this one!
            _standardDurations = new int[] { duration };

            List<ActivityPeakDetail> peaks = FindPeaks();

            // we should only get back one result as we only asked for one duration.
            if (peaks.Count() != 1)
                return null;

            return peaks[0];
        }
    }
}