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
        public string Type { get; set; }
        public int? SufferScore { get; set; }
        public string EmbedToken { get; set; }
        public float Distance { get; set; }
        public int TotalPhotoCount { get; set; }
        public float ElevationGain { get; set; }
        public bool HasKudoed { get; set; }
        public float AverageHeartrate { get; set; }
        public float MaxHeartrate { get; set; }
        public int? Truncated { get; set; }
        public string GearId { get; set; }
        public float AverageSpeed { get; set; }
        public float MaxSpeed { get; set; }
        public float AverageCadence { get; set; }
        public float AverageTemperature { get; set; }
        public float AveragePower { get; set; }
        public float Kilojoules { get; set; }
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
        public DateTime StartDateLocal { get; set; }
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

        public float Calories { get; set; }
        public string Description { get; set; }
    }
}
