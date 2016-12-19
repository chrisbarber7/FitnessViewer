using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.enums;
using FitnessViewer.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Models
{
    public class AthleteSetting : Entity<long>, IEntity<long>, IUserEntity
    {
        [Key]
        [ForeignKey("Athlete")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override long Id { get; set; }
        public virtual Athlete Athlete { get; set; }

        [Required]
        [MaxLength(128)]
        [ForeignKey("User")]
        [Index("IX_AthleteSetting_UserId", 1, IsUnique = true)]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public DateTime DashboardStart { get; set; }
        public DateTime DashboardEnd { get; set; }

        [MaxLength(32)]
        public string DashboardRange { get; set; }


        public bool ShowRun { get; set; }
        public bool ShowRide { get; set; }
        public bool ShowSwim { get; set; }
        public bool ShowOther { get; set; }
        public bool ShowAll { get; set; }

        public SportUnitsDistance RunDistanceUnit { get; set; }
        public SportUnitsDistance RideDistanceUnit { get; set; }
        public SportUnitsDistance SwimDistanceUnit { get; set; }
        public SportUnitsDistance OtherDistanceUnit { get; set; }
        public SportUnitsDistance AllDistanceUnit { get; set; }

        public SportUnitsPace RunPaceUnit { get; set; }
        public SportUnitsPace RidePaceUnit { get; set; }
        public SportUnitsPace SwimPaceUnit { get; set; }
        public SportUnitsPace OtherPaceUnit { get; set; }
        public SportUnitsPace AllPaceUnit { get; set; }


        public SportUnitsElevation RunElevationUnit { get; set; }
        public SportUnitsElevation RideElevationUnit { get; set; }
        public SportUnitsElevation SwimElevationUnit { get; set; }
        public SportUnitsElevation OtherElevationUnit { get; set; }
        public SportUnitsElevation AllElevationUnit { get; set; }

        public static AthleteSetting DefaultSettings(long id, string userId)
        {
            return new AthleteSetting()
            {
                Id = id,
                UserId = userId,
                DashboardStart = DateTime.Now.AddDays(-29).Date,
                DashboardEnd = DateTime.Now.Date,

                ShowRun = true,
                ShowRide = true,
                ShowSwim = true,
                ShowOther = true,
                ShowAll = true,

                RunDistanceUnit = SportUnitsDistance.Miles,
                RideDistanceUnit = SportUnitsDistance.Miles,
                SwimDistanceUnit = SportUnitsDistance.Meters,
                OtherDistanceUnit = SportUnitsDistance.Miles,
                AllDistanceUnit = SportUnitsDistance.Miles,

                RunPaceUnit = SportUnitsPace.MinutePerMile,
                RidePaceUnit = SportUnitsPace.MilesPerHour,
                SwimPaceUnit = SportUnitsPace.MinutePerHundredMeter,
                OtherPaceUnit = SportUnitsPace.MilesPerHour,
                AllPaceUnit = SportUnitsPace.MilesPerHour,

                RunElevationUnit = SportUnitsElevation.Feet,
                RideElevationUnit = SportUnitsElevation.Feet,
                SwimElevationUnit = SportUnitsElevation.Feet,
                OtherElevationUnit = SportUnitsElevation.Feet,
                AllElevationUnit = SportUnitsElevation.Feet

            };
        }
    }
}