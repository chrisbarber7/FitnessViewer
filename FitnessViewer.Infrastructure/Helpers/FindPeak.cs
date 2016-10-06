using FitnessViewer.Infrastructure.enums;
using FitnessViewer.Infrastructure.Models;
using System.Collections.Generic;
using System.Linq;

namespace FitnessViewer.Infrastructure.Helpers
{
    /// <summary>
    /// Find peak values for a duration in a stream (ie 30 second peak power)
    /// </summary>
    public class PeakValueFinder
    {
        private List<int> _data;
        private int[] _standardDurations;
        private ActivityPeakDetail _peakInformation;
        private bool _peakFound = false;
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
            _data = stream;
            _streamType = type;
            _activityId = activityId;

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
                peaks.Add(FindPeakForDuration(duration));

            return peaks;
        }

        /// <summary>
        /// Find peak in stream for a single duration
        /// </summary>
        /// <param name="duration">Time to find peak for (in seconds)</param>
        /// <returns></returns>
        public ActivityPeakDetail FindPeakForDuration(int duration)
        {
            // if there isn't enough data for the duration then we can't find a peak
            // ie looking for 60 minute peak in a 30 minute activity
            if (_data.Count < duration)
                return new ActivityPeakDetail(_activityId, _streamType, duration);

            _peakFound = false;

            _peakInformation = new ActivityPeakDetail(_activityId, _streamType) { Duration = duration, Value = 0, StartIndex = 0 };

            // if duration is full activity then change duration to full list size to process.
            if (duration == int.MaxValue)
                duration = _data.Count;

            // loop over the data for each possible starting point for the given duration
            for (int startDataPoint = 0; startDataPoint <= _data.Count - duration; startDataPoint++)
                CheckDuration(startDataPoint, duration);

            if (!_peakFound)
                _peakInformation.Value = _peakInformation.StartIndex = null;

            return _peakInformation;
        }

        /// <summary>
        /// Find peak for a given duration
        /// </summary>
        /// <param name="startDataPoint">Starting data point in stream</param>
        /// <param name="duration">number of data points to examine in stream</param>
        private void CheckDuration(int startDataPoint, int duration)
        {
            int totalForDuration = 0;
            int pointsCounted = 0;
            for (int i = 0; i <= duration - 1; i++)
            {
                // for cadence peaks we ignore 0 values to exclude times when stationary.
                if (_streamType == PeakStreamType.Cadence && _data[startDataPoint + i] == 0)
                    continue;

                totalForDuration += _data[startDataPoint + i];
                pointsCounted++;
            }

            if (pointsCounted == 0)
                return;

            int loopPeak = totalForDuration / pointsCounted;

            if (loopPeak > _peakInformation.Value)
            {
                _peakFound = true;
                _peakInformation.Value = loopPeak;
                _peakInformation.StartIndex = startDataPoint;
            }

            return;
        }
    }
}