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

namespace FitnessViewer.Infrastructure.Models.Collections
{
    /// <summary>
    /// Streams for a single activity
    /// </summary>
    public class ActivityStreams
    {
        private List<Stream> _containedStreams { get; }
        public long ActivityId { get; private set; }
        private UnitOfWork _unitOfWork;
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
                    _activity = _unitOfWork.Activity.GetActivity(ActivityId);

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


        /// <summary>
        /// Create and load partial stream for an activity
        /// </summary>
        /// <param name="activityId">Strava ActivityId</param>
        /// <param name="start">Start location in stream</param>
        /// <param name="end">End location in stream</param>
        /// <returns></returns>
        public static ActivityStreams CreateFromExistingActivityStream(long activityId, int start, int end)
        {
            UnitOfWork uow = new UnitOfWork();
            return new ActivityStreams(activityId, uow.Activity
                                                        .GetStreamForActivity(activityId)
                                                        .Where(s=>s.Time>=start && s.Time<=end));
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

        private ActivityStreams(long activityId, IEnumerable<Stream> stream)
        {
            _unitOfWork = new UnitOfWork();
            _containedStreams = stream.ToList();
            ActivityId = activityId;
        }

        private ActivityStreams(long activityId)
        {
            _unitOfWork = new UnitOfWork();
            _containedStreams = new List<Models.Stream>();

            ActivityId = activityId;
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
            switch(type)
            {
                case StreamType.Watts: {  return _containedStreams.Select(s => s.Watts) as IEnumerable<T>; }
                case StreamType.Altitude: { return _containedStreams.Select(s => s.Activity) as IEnumerable<T>; }
                case StreamType.Cadence: { return _containedStreams.Select(s => s.Cadence) as IEnumerable<T>; }
                case StreamType.Distance: { return _containedStreams.Select(s => s.Distance) as IEnumerable<T>; }
                case StreamType.Gradient: { return _containedStreams.Select(s => s.Gradient) as IEnumerable<T>; }
                case StreamType.Heartrate: { return _containedStreams.Select(s => s.HeartRate) as IEnumerable<T>; }
                case StreamType.Latitude: { return _containedStreams.Select(s => s.Latitude) as IEnumerable<T>; }
                case StreamType.Longitude: { return _containedStreams.Select(s => s.Longitude) as IEnumerable<T>; }
                case StreamType.Moving: { return _containedStreams.Select(s => s.Moving) as IEnumerable<T>; }
                case StreamType.Temp: { return _containedStreams.Select(s => s.Temperature) as IEnumerable<T>; }
                case StreamType.Time: { return _containedStreams.Select(s => s.Time) as IEnumerable<T>; }
                default: return null;
            }
        }

        /// <summary>
        /// Does this activity have the given stream.
        /// </summary>
        /// <param name="type">Which stream?</param>
        /// <returns>Flag to indicate if the given stream exists for the activity.</returns>
        private bool HasIndividualStream(StreamType type)
        {
            switch (type)
            {
                case StreamType.Cadence: { return !GetIndividualStream<int?>(StreamType.Cadence).Contains(null); }
                case StreamType.Watts: { return !GetIndividualStream<int?>(StreamType.Watts).Contains(null); }
                case StreamType.Heartrate: { return !GetIndividualStream<int?>(StreamType.Heartrate).Contains(null); }
                default: return false;
            }
        }

        /// <summary>
        /// Write all steams to the database.
        /// </summary>

        internal void StoreStreams()
        {
            // write all details to database.
            _unitOfWork.Activity.AddStreamBulk(_containedStreams);
            _unitOfWork.Complete();
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

        public ActivityMinMaxDto BuildSummaryInformation()
        {
            return ActivityMinMaxDto.CreateFromActivityStreams(this);
        }


        internal int?[] GetSecondsPerMileFromVelocity()
        {
            return _containedStreams.Where(s => s.Velocity.HasValue && s.Velocity.Value > 0)
                                    .Select(s => (int?)Distance.MetrePerSecondToSecondPerMile(s.Velocity.Value))
                                .ToArray();

            //List<int> secsPerMile = new List<int>();

            //foreach (Stream s in stream)
            //{
            //    if (s.Velocity.HasValue && s.Velocity.Value > 0)
            //        secsPerMile.Add(Distance.MetrePerSecondToSecondPerMile(s.Velocity.Value));
            //}

            //     return secsPerMile.Select(s => (int?)s).ToArray();
        }

    }
}