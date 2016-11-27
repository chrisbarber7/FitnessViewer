using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.enums;
using FitnessViewer.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitnessViewer.Infrastructure.Models.Dto;
using FitnessViewer.Infrastructure.Helpers.Conversions;
using static FitnessViewer.Infrastructure.Models.Dto.ActivityMinMaxDto;
using FitnessViewer.Infrastructure.Repository;
using FitnessViewer.Infrastructure.Interfaces;

namespace FitnessViewer.Infrastructure.Models.Collections
{
    /// <summary>
    /// Streams for a single activity
    /// </summary>
    public class ActivityStreams
    {
        private List<Stream> _containedStreams { get; }
        public long ActivityId { get; private set; }
        private Interfaces.IUnitOfWork _unitOfWork;
        private Activity _activity;


        /// <summary>
        /// Create an empty stream for a new activity.
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public static ActivityStreams CreateForNewActivity(long activityId)
        {
            return new ActivityStreams(activityId);
        }

        internal Activity Activity
        {
            get
            {
                if (_activity == null)
                    _activity = _unitOfWork.CRUDRepository.GetByKey<Activity>(ActivityId, o => o.ActivityType, o => o.Athlete);

                return _activity;
            }
        }



        /// <summary>
        /// Create and load existing stream for an activity
        /// </summary>
        /// <param name="activityId">Strava ActivityId</param>
        /// <returns></returns>
        public static ActivityStreams CreateFromExistingActivityStream(long activityId)
        {
            return CreateFromExistingActivityStream(activityId, 0, int.MaxValue);
        }

        private ActivityStreams(long activityId, IEnumerable<Stream> stream)
        {
            _unitOfWork = new Data.UnitOfWork();
            _containedStreams = stream.OrderBy(s => s.Time).ToList();
            ActivityId = activityId;
        }

        private ActivityStreams(long activityId)
        {
            _unitOfWork = new Data.UnitOfWork();
            _containedStreams = new List<Models.Stream>();

            ActivityId = activityId;
        }

        /// <summary>
        /// Stream Duration
        /// </summary>
        /// <returns></returns>
        public TimeSpan GetTime()
        {
            return TimeSpan.FromSeconds(Stream.Count());
        }

        /// <summary>
        /// Average Speed
        /// </summary>
        /// <returns></returns>
        public decimal GetAverageSpeed()
        {
            if (!IsValid())
                return 0.00M;

            return Math.Round(GetDistance() / Convert.ToDecimal(GetTime().TotalSeconds) * 60 * 60, 2);
        }

        /// <summary>
        /// Elevation Gain
        /// </summary>
        /// <returns></returns>
        public decimal GetElevationGain()
        {
            if (!IsValid())
                return 0.00M;

            double? previousAltitude = null;
            double gain = 0;
            foreach (double? altitide in GetIndividualStream<double?>(StreamType.Altitude).Where(s => s.HasValue))
            {
                // first pass though we'll have no previous altitude.
                if (previousAltitude == null)
                    previousAltitude = altitide;

                // have we gained altitude?
                if (altitide > previousAltitude)
                    gain += altitide.Value - previousAltitude.Value;

                previousAltitude = altitide;
            }

            return gain.ToFeet();
        }


        /// <summary>
        /// Elevation Loss
        /// </summary>
        /// <returns></returns>
        public decimal GetElevationLoss()
        {
            if (!IsValid())
                return 0.00M;

            double? previousAltitude = null;
            double loss = 0;
            foreach (double? altitide in GetIndividualStream<double?>(StreamType.Altitude).Where(s => s.HasValue))
            {
                // first pass though we'll have no previous altitude.
                if (previousAltitude == null)
                    previousAltitude = altitide;

                // have we gained altitude?
                if (altitide < previousAltitude)
                    loss += previousAltitude.Value - altitide.Value;

                previousAltitude = altitide;
            }

            return loss.ToFeet();
        }

        /// <summary>
        /// Calculate Watts per Kg 
        /// </summary>
        /// <returns></returns>
        public decimal? GetWattsPerKg()
        {
            if (!IsValid())
                return null;

            // we need both a power meter and a weight
            if ((Activity.HasPowerMeter) && (Activity.Weight != null))
            {
                if (!HasIndividualStream(StreamType.Watts))
                    return null;

                double? aveWatts = GetIndividualStream<int?>(StreamType.Watts).Average(s => s.Value);

                if (aveWatts != null)
                    return Math.Round(Convert.ToDecimal(aveWatts.Value) / Activity.Weight.Value, 2);
            }

            return null;
        }

        /// <summary>
        /// Which streams are valid and available on the activity
        /// </summary>
        /// <returns></returns>
        public List<MinMaxAve> GetStreamSummary()
        {
            List<MinMaxAve> streamInfo = new List<MinMaxAve>();

            StreamType activityTypeStreams = StreamTypeHelper.SportStreams(Activity.ActivityType);

            foreach (StreamType t in Enum.GetValues(typeof(StreamType)))
            {
                if (!activityTypeStreams.HasFlag(t))
                    continue;

                MinMaxAve mma = GetMinMaxAve(t);

                if (mma.HasStream)
                    streamInfo.Add(mma);
            }

            return streamInfo.OrderBy(s => s.Priority).ToList();
        }
        /// <summary>
        /// Create and load partial stream for an activity
        /// </summary>
        /// <param name="activityId">Strava ActivityId</param>
        /// <param name="start">Start location in stream</param>
        /// <param name="end">End location in stream</param>
        /// <returns></returns>
        public static ActivityStreams CreateFromExistingActivityStream(long activityId, int start, int end)
        {
            IUnitOfWork uow = new Data.UnitOfWork();
            return new ActivityStreams(activityId, uow.CRUDRepository
                                                        .GetByActivityId<Stream>(activityId)
                                                        .Where(s => s.Time >= start && s.Time <= end)
                                                        .OrderBy(s => s.Time));
        }

        /// <summary>
        /// Create using provided activity id and stream
        /// </summary>
        /// <param name="activityId">Strava ActivityId</param>
        /// <param name="stream">Existing stream details.</param>
        /// <returns></returns>
        public static ActivityStreams CreateFromExistingActivityStream(long activityId, IEnumerable<Stream> stream)
        {
            return new ActivityStreams(activityId, stream);
        }

        /// <summary>
        /// Allow access to the full stream information.
        /// </summary>
        public List<Stream> Stream
        {
            get
            {
                return _containedStreams;
            }
        }

        /// <summary>
        /// Create Activity Analytics based on the activity type.
        /// </summary>
        /// <returns>Analytics</returns>
        public ActivityAnalyticsDto GetAnalytics()
        {
            if (!IsValid())
                return ActivityAnalyticsDto.EmptyStream();

            if (Activity.ActivityType.IsRide)
                return ActivityAnalyticsDto.RideCreateFromPowerStream(Stream, 295);
            else if (Activity.ActivityType.IsRun)
                return ActivityAnalyticsDto.RunCreateFromPaceOrHeartRateStream(Stream.Select(w => w.Velocity).ToList(), Stream.Select(w => w.HeartRate).ToList());
            else if (Activity.ActivityType.IsSwim)
                return ActivityAnalyticsDto.SwimCreateFromPaceStream(Stream.Select(w => w.Velocity).ToList());
            else
                return ActivityAnalyticsDto.OtherUnknown();
        }



        internal ActivityPeakDetails CalculatePeak(StreamType type, int? duration)
        {
            if (!this.HasIndividualStream(type))
                return new ActivityPeakDetails();

            return new ActivityPeakDetails(PeakSeeker.Create(this, type).FindPeaks(duration));
        }


        /// <summary>
        ///  Calculate a peak information for a given stream type.
        /// </summary>
        /// <param name="type">StreamType for which to calculate peaks</param>
        /// <returns></returns>
        internal ActivityPeakDetails CalculatePeak(StreamType type)
        {
            return CalculatePeak(type, null);
        }

        /// <summary>
        /// Return an invividual stream
        /// </summary>
        /// <typeparam name="T">Return Type. 
        /// double? for Lat,Long, Distance, Altitude, Velocity & gradient
        /// int? for heartRate, Cadence, Watts & temperature
        /// bool? for moving.</typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public IEnumerable<T> GetIndividualStream<T>(StreamType type)
        {
            switch (type)
            {
                case StreamType.Watts: { return _containedStreams.Select(s => s.Watts) as IEnumerable<T>; }
                case StreamType.Altitude: { return _containedStreams.Select(s => s.Altitude) as IEnumerable<T>; }
                case StreamType.Cadence: { return _containedStreams.Select(s => s.Cadence) as IEnumerable<T>; }
                case StreamType.Distance: { return _containedStreams.Select(s => s.Distance) as IEnumerable<T>; }
                case StreamType.Gradient: { return _containedStreams.Select(s => s.Gradient) as IEnumerable<T>; }
                case StreamType.Heartrate: { return _containedStreams.Select(s => s.HeartRate) as IEnumerable<T>; }
                case StreamType.Latitude: { return _containedStreams.Select(s => s.Latitude) as IEnumerable<T>; }
                case StreamType.Longitude: { return _containedStreams.Select(s => s.Longitude) as IEnumerable<T>; }
                case StreamType.Moving: { return _containedStreams.Select(s => s.Moving) as IEnumerable<T>; }
                case StreamType.Temp: { return _containedStreams.Select(s => s.Temperature) as IEnumerable<T>; }
                case StreamType.Velocity: { return _containedStreams.Select(s => s.Velocity) as IEnumerable<T>; }
                case StreamType.Pace: { return _containedStreams.Select(s => s.Velocity) as IEnumerable<T>; }
                case StreamType.Time: { return _containedStreams.Select(s => s.Time) as IEnumerable<T>; }
                default: return null;
            }
        }


        private MinMaxAve GetMinMaxAve(StreamType streamType)
        {
            // if the activity doesn't have the requested stream then no point checkings for values;
            if (!HasIndividualStream(streamType))
                return new MinMaxAve(streamType);


            // need a better method to do this!  Having to split into two queries depending on the data type of the stream!!!
            if (streamType.HasFlag(StreamType.Distance) ||
                streamType.HasFlag(StreamType.Altitude) ||
                streamType.HasFlag(StreamType.Velocity) ||
                 streamType.HasFlag(StreamType.Pace) ||
                streamType.HasFlag(StreamType.Gradient))
            {
                return GetIndividualStream<double?>(streamType).GroupBy(i => 1)
                          .Select(g => new MinMaxAve(streamType)
                          {
                              HasStream = true,
                              Min = Convert.ToDecimal(g.Min(s => s.Value)),
                              Ave = Convert.ToDecimal(g.Average(s => s.Value)),
                              Max = Convert.ToDecimal(g.Max(s => s.Value))

                          }).First();
            }
            else if (streamType.HasFlag(StreamType.Heartrate) ||
                streamType.HasFlag(StreamType.Cadence) ||
                streamType.HasFlag(StreamType.Watts) ||
                streamType.HasFlag(StreamType.Temp))
            {
                return GetIndividualStream<int?>(streamType)
                            .Where(i => streamType == StreamType.Cadence ? i.Value > 0 : true).GroupBy(i => 1)
                            .Select(g => new MinMaxAve(streamType)
                            {
                                HasStream = true,
                                Min = g.Min(s => s.Value),
                                Ave = Convert.ToDecimal(g.Average(s => s.Value)),
                                Max = g.Max(s => s.Value)
                            }).First();
            }

            // unhandled stream type!
            return new MinMaxAve(streamType);
        }


        /// <summary>
        /// Does this activity have the given stream.
        /// </summary>
        /// <param name="type">Which stream?</param>
        /// <returns>Flag to indicate if the given stream exists for the activity.</returns>
        private bool HasIndividualStream(StreamType type)
        {
            if (!IsValid())
                return false;

            switch (type)
            {
                case StreamType.Cadence: { return !GetIndividualStream<int?>(StreamType.Cadence).Contains(null); }
                case StreamType.Watts: { return !GetIndividualStream<int?>(StreamType.Watts).Contains(null); }
                case StreamType.Heartrate: { return !GetIndividualStream<int?>(StreamType.Heartrate).Contains(null); }
                case StreamType.Altitude: { return !GetIndividualStream<double?>(StreamType.Altitude).Contains(null); }
                case StreamType.Distance: { return !GetIndividualStream<double?>(StreamType.Distance).Contains(null); }
                case StreamType.Gradient: { return !GetIndividualStream<double?>(StreamType.Gradient).Contains(null); }
                case StreamType.Latitude: { return !GetIndividualStream<double?>(StreamType.Latitude).Contains(null); }
                case StreamType.Longitude: { return !GetIndividualStream<double?>(StreamType.Longitude).Contains(null); }
                case StreamType.Moving: { return !GetIndividualStream<bool?>(StreamType.Moving).Contains(null); }
                case StreamType.Temp: { return !GetIndividualStream<int?>(StreamType.Temp).Contains(null); }
                case StreamType.Time: { return true; }
                case StreamType.Velocity: { return !GetIndividualStream<double?>(StreamType.Velocity).Contains(null); }
                case StreamType.Pace: { return !GetIndividualStream<double?>(StreamType.Pace).Contains(null); }
                default: return false;
            }
        }

        /// <summary>
        /// Write all steams to the database.
        /// </summary>
        internal void StoreStreams()
        {
            StreamRepository repo = new StreamRepository();
            repo.AddStreamBulk(_containedStreams);
        }

        /// <summary>
        /// Calculate peaks for stream and save.
        /// </summary>
        public void CalculatePeaksAndSave()
        {
            this.CalculatePeak(StreamType.Watts).Save();
            this.CalculatePeak(StreamType.Heartrate).Save();
            this.CalculatePeak(StreamType.Cadence).Save();
        }

        public void AddPowerCurveCalculationJobs()
        {
            foreach (int d in PeakDuration.CreatePowerCurveDurations(this.GetIndividualStream<int?>(StreamType.Watts).Count()).Durations)
                DownloadQueue.CreateQueueJob(this.Activity.Athlete.UserId,
                                             enums.DownloadType.CalculateActivityStats,
                                             this.ActivityId,
                                             d)
                             .Save();
        }

        internal int?[] GetSecondsPerMile()
        {
            return _containedStreams.Where(s => s.Velocity.HasValue && s.Velocity.Value > 0)
                                    .Select(s => (int?)Distance.MetrePerSecondToSecondPerMile(s.Velocity.Value))
                                    .ToArray();
        }

        /// <summary>
        /// Calculate distance based on start/end distances of stream.
        /// </summary>
        /// <returns></returns>
        public decimal GetDistance()
        {
            if (Stream.Count == 0)
                return 0.00M;

            var startDetails = Stream.First();
            var endDetails = Stream.Last();

            if ((startDetails.Distance != null) && (endDetails.Distance != null))
                return Helpers.Conversions.Distance.MetersToMiles(Convert.ToDecimal(endDetails.Distance.Value - startDetails.Distance.Value));

            return 0.00M;
        }

        /// <summary>
        /// Is this a valid stream?
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            if (this.Stream == null)
                return false;

            if (this.Stream.Count() == 0)
                return false;

            return true;
        }

        /// <summary>
        /// Are there any gaps in the stream (caused by activity stopped/started)?
        /// </summary>
        /// <returns></returns>
        internal bool GapsInStream()
        {
            return FindGapsInStream().Count() == 0 ? false : true;
        }


        /// <summary>
        /// List of any times missing from the stream?
        /// </summary>
        /// <returns></returns>
        private IEnumerable<int> FindGapsInStream()
        {
            //get an array of the times we've got
            var time = this.GetIndividualStream<int>(StreamType.Time).ToArray();

            // and compare it against the range of times we're expecting.
            return Enumerable.Range(0, this.Stream.Count()).Except(time);
        }

        /// <summary>
        /// Fix any gaps in the stream by adding empty stream rows (just have a time stream)
        /// </summary>
        internal void FixGaps()
        {
            foreach (int missingTime in FindGapsInStream())
                this.Stream.Add(new Models.Stream() { Time = missingTime }); 
        }
    }
}