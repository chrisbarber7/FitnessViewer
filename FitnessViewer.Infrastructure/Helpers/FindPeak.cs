using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.enums;
using FitnessViewer.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Helpers
{




    /// <summary>
    /// Find peak values for a duration in a stream (ie 30 second peak power)
    /// </summary>
    public class PeakValueFinder
    {
        private int[] _data;
        private int[] _standardDurations;
        private PeakStreamType _streamType;
        private long _activityId;
        private bool _includeFullDuration = true;

        public bool IncludeFullDuration
        {
            get { return _includeFullDuration; }
            set { _includeFullDuration = value; }
        }


        public PeakValueFinder(List<int> stream, PeakStreamType type, long activityIds)
            : this(stream, type, activityIds, false)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream">Stream to be analysed for peaks.</param>
        /// <param name="type">Type of stream (power, HR, cadence)</param>
        /// <param name="activityId">Activity Id.</param>
        public PeakValueFinder(List<int> stream, PeakStreamType type, long activityId, bool usePowerCurveDurations)
        {
            _streamType = type;
            _activityId = activityId;

            // for cadence peaks we ignore 0 values to exclude times when stationary so strip out now.
            _data = stream.Where(d => _streamType == PeakStreamType.Cadence ? d > 0 : true).ToArray();

            if (usePowerCurveDurations)
                SetupPowerCurveDurations();
            else
            {
                UnitOfWork uow = new UnitOfWork();
                _standardDurations = uow.Analysis.GetPeakStreamTypeDuration(type);
            }

        }


        public int[] StandardDurations
        {
            get { return _standardDurations; }
        }

        private int[] SetupPowerCurveDurations()
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

            //    durations.Add(int.MaxValue);

            // include any duration less than event time (and int.MaxValue for full event).
            _standardDurations = durations.Where(d => d <= _data.Count() || d == int.MaxValue).ToArray();

            return _standardDurations;
        }


        public static List<ActivityPeakDetail> ExtractPeaksFromStream(long activityId, List<int?> stream, PeakStreamType type)
        {
            if (stream.Contains(null))
                return null;

            PeakValueFinder finder = new PeakValueFinder(
                stream.Select(s => s.Value).ToList(),
                type,
                activityId, true);

            return finder.FindPeaks();
        }


        /// <summary>
        /// Find peaks in stream for all standard durations
        /// </summary>
        /// <returns></returns>
        public List<ActivityPeakDetail> FindPeaks()
        {
            List<ActivityPeakDetailCalculator> peaks = new List<ActivityPeakDetailCalculator>();

            foreach (int duration in _standardDurations)
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


            int dataCount = _data.Length;

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

            if (_includeFullDuration)
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

            List<ActivityPeakDetail> details = new List<ActivityPeakDetail>();

            foreach (ActivityPeakDetailCalculator p in peaks)
            {
                if (p.Value == -1)
                    p.Value = null;

                details.Add(AutoMapper.Mapper.Map<ActivityPeakDetail>(p));
            }


            return details;
        }

        public ActivityPeakDetail FindPeakForDuration(int duration)
        {
            // override default duration as we're only interested in this one!
            _standardDurations = new int[] { duration };


            this.IncludeFullDuration = false;

            List<ActivityPeakDetail> peaks = FindPeaks();


            // we should only get back one result as that's all we asked for.
            if (peaks.Count() != 1)
                return null;

            return peaks[0];
        }  
    }

   /// <summary>
   /// Add a Queue collection to base class which will be used to hold values used to 
   /// calculate the average.
   /// </summary>
          [NotMapped]
        public class ActivityPeakDetailCalculator : ActivityPeakDetail
        {
            public Queue<int> rollingValues = new Queue<int>();

            public ActivityPeakDetailCalculator(long activityId, PeakStreamType type, int duration) : base(activityId, type, duration)
            {
            }

        }

   


}