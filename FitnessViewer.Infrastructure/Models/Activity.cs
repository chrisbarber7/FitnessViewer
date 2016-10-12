using FitnessViewer.Infrastructure.Helpers;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessViewer.Infrastructure.Models
{
    public class Activity
    {
        // disabling auto identity column to allow use of strava activity id as the key.
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        [Required]
        [ForeignKey("Athlete")]
        public long AthleteId { get; set; }
        public virtual Athlete Athlete { get; set; }

        public string Name { get; set; }
        public string ExternalId { get; set; }



        [ForeignKey("ActivityType")]
        public string ActivityTypeId { get; set; }
        public virtual ActivityType ActivityType { get; set; }

        public int? SufferScore { get; set; }
        public string EmbedToken { get; set; }

        private decimal _distance { get; set; }
        public decimal Distance
        {
            get
            {
                return _distance;

            }
            set
            {
                _distance = value;
                DistanceInMiles = MetreDistance.ToMiles(value);
            }
        }

        public decimal DistanceInMiles { get; set; }
        public int TotalPhotoCount { get; set; }
        public decimal ElevationGain { get; set; }
        public bool HasKudoed { get; set; }
        public decimal AverageHeartrate { get; set; }
        public decimal MaxHeartrate { get; set; }
        public int? Truncated { get; set; }
        public string GearId { get; set; }
        public decimal AverageSpeed { get; set; }
        public decimal MaxSpeed { get; set; }
        public decimal AverageCadence { get; set; }
        public decimal AverageTemperature { get; set; }
        public decimal AveragePower { get; set; }
        public decimal Kilojoules { get; set; }
        public bool IsTrainer { get; set; }
        public bool IsCommute { get; set; }
        public bool IsManual { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsFlagged { get; set; }
        public int AchievementCount { get; set; }
        public int KudosCount { get; set; }
        public int CommentCount { get; set; }
        public int AthleteCount { get; set; }
        public int PhotoCount { get; set; }
        public DateTime StartDate { get; set; }

        private DateTime _startDateLocal;
        public string DeviceName { get; set; }


        // necessay so that we can join to calendar on date only
        public DateTime StartDateLocal
        {
            get { return _startDateLocal; }
            set
            {
                _startDateLocal = value;
                Start = StartDateLocal.Date;
            }
        }

        // just the date (no time) to be used as FK to calendar.
        [ForeignKey("Calendar")]
        public DateTime Start { get; private set; }
        public virtual Calendar Calendar { get; set; }

        public TimeSpan? MovingTime { get; set; }
        public TimeSpan? ElapsedTime { get; set; }
        public string TimeZone { get; set; }
        public double? StartLatitude { get; set; }
        public double? StartLongitude { get; set; }
        public int WeightedAverageWatts { get; set; }

        public double? EndLatitude { get; set; }
        public double? EndLongitude { get; set; }
        public bool HasPowerMeter { get; set; }
        //public Map Map { get; set; }

        public string MapId { get; set; }
        public string MapPolyline { get; set; }
        public string MapPolylineSummary { get; set; }

        public decimal Calories { get; set; }
        public string Description { get; set; }

        private int? _streamSize;

        public int? StreamSize
        {
            get
            { return _streamSize; }
            set
            {
                _streamSize = value;
                
                if (_streamSize <= 500)
                    StreamStep = 1;
                else if (_streamSize <= 2000)
                    StreamStep = 2;
                else if (_streamSize <= 5000)
                    StreamStep = 5;
                else if (_streamSize <= 10000)
                    StreamStep = 10;
                else if (_streamSize <= 20000)
                    StreamStep = 20;
                else
                    StreamStep = 30;
            }
        }
        public int? StreamStep { get; private set; }

        public bool DetailsDownloaded { get; set; }

        /// <summary>
        /// return distance formatted for the given activity.
        /// </summary>
        /// <returns>formatted distance</returns>
        public string GetDistanceByActivityType()
        {
            if (this.ActivityType.IsRide)
                return string.Format("{0}", MetreDistance.ToMiles(this.Distance).ToString());
            else if (this.ActivityType.IsRun)
                return string.Format("{0}mi", MetreDistance.ToMiles(this.Distance).ToString());
            else if (this.ActivityType.IsSwim)
                return string.Format("{0}m", this.Distance.ToString());
            else
                return this.Distance.ToString();
        }
    }
}
