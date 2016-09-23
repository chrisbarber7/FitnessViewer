using System.Collections.Generic;

namespace FitnessViewer.Infrastructure.Helpers
{
    /// <summary>
    /// holds detail of a single peak.
    /// </summary>
    public class PeakDetail
    {
        public PeakDetail(PeakStreamType type)
        {
            this.StreamType = type;
        }

        public PeakDetail(PeakStreamType type, int duration)
        {
            this.Duration = duration;
            this.Value = null;
            this.Start = null;
            this.StreamType = type;
        }

        public int Duration { get; set; }       // duration of peak (in seconds)
        public int? Value { get; set; }         // peak
        public int? Start { get; set; }         // starting point in stream for the peak
        public PeakStreamType StreamType { get; set; }
    }

    /// <summary>
    /// Type of stream (used to set standard duration periods)
    /// </summary>
    public enum PeakStreamType
    {
        Power,
        HeartRate,
        Cadence
    }

    /// <summary>
    /// Find peak values for a duration in a stream (ie 30 second peak power)
    /// </summary>
    public class PeakValueFinder
    {
        private List<int> _data;
        private int[] _standardDurations;
        private PeakDetail _peakInformation;
        private bool _peakFound = false;
        private PeakStreamType _streamType;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream">Stream to be analysed for peaks.</param>
        /// <param name="type">Type of stream (power, HR, cadence)</param>
        public PeakValueFinder(List<int> stream, PeakStreamType type)
        {
            _data = stream;
            _streamType = type;

            // set standard reporting durations (in seconds)
            switch (type)
            {
                case PeakStreamType.Power:
                    {
                        _standardDurations = new int[] { 5, 10, 30, 60, 120, 300, 360, 600, 720, 1200, 1800, 3600, _data.Count };
                        break;
                    }
                case PeakStreamType.HeartRate:
                    {
                        _standardDurations = new int[] { 60, 120, 300, 360, 600, 720, 1200, 1800, 3600, _data.Count };
                        break;
                    }
                case PeakStreamType.Cadence:
                    {
                        _standardDurations = new int[] { 5, 10, 30, 60, 120, 300, 360, 600, 720, 1200, 1800, 3600, _data.Count };
                        break;
                    };
            }
        }

        /// <summary>
        /// Find peaks in stream for all standard durations
        /// </summary>
        /// <returns></returns>
        public List<PeakDetail> FindPeaks()
        {
            List<PeakDetail> peaks = new List<PeakDetail>();

            foreach (int duration in _standardDurations)
                peaks.Add(FindPeakForDuration(duration));

            return peaks;
        }

        /// <summary>
        /// Find peak in stream for a single duration
        /// </summary>
        /// <param name="duration">Time to find peak for (in seconds)</param>
        /// <returns></returns>
        public PeakDetail FindPeakForDuration(int duration)
        {
            // if there isn't enough data for the duration then we can't find a peak
            // ie looking for 60 minute peak in a 30 minute activity
            if (_data.Count < duration)
                return new PeakDetail(_streamType, duration);

            _peakFound = false;

            _peakInformation = new PeakDetail(_streamType) { Duration = duration, Value = 0, Start = 0 };

            // loop over the data for each possible starting point for the given duration
            for (int startDataPoint = 0; startDataPoint <= _data.Count - duration; startDataPoint++)
                CheckDuration(startDataPoint, duration);

            if (!_peakFound)
                _peakInformation.Value = _peakInformation.Start = null;

            // if full activity peak then set duration to max int value so we've got a set value to search for to find activity average.
            if (_peakInformation.Duration == _data.Count)
                _peakInformation.Duration = int.MaxValue;

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
                _peakInformation.Start = startDataPoint;
            }

            return;
        }
    }
}