using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessViewer.Infrastructure.Models
{
    public class Athlete : Entity<long>, IEntity<long>, IUserEntity
    {
        public Athlete() 
        {
            this.Activities = new Collection<Activity>();
                      }


        // disabling auto identity column to allow use of strava athlete id as the key.
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override long Id { get; set; }

        [Required]
        [MaxLength(128)]
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public AthleteSetting AthleteSetting { get; set;  }
        

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
        public decimal? Weight { get; set; }
        public string Token { get; set; }

        public ICollection<Activity> Activities { get; private set; }
    }
}