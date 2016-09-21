using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessViewer.Core
{
    public class StravaAthlete
    {
        // disabling auto identity column to allow use of strava activity id as the key.
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        [Required]
        [MaxLength(128)]
        public string UserId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfileMedium { get; set; }
        public string Profile { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Sex { get; set; }
        public string Friend { get; set; }
        public string Follower { get; set; }
        public Boolean IsPremium { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public Boolean ApproveFollowers { get; set; }

        public int AthleteType { get; set; }
        public string DatePreference { get; set; }
        public string MeasurementPreference { get; set; }
        public string Email { get; set; }
        public int? FTP { get; set; }
        public float? Weight { get; set; }
        public string Token { get; set; }
    }
}