using FitnessViewer.Infrastructure.enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessViewer.Infrastructure.Models
{
    public class Gear
    {
        private Gear()
        {
        }

        // disabling auto identity column to allow use of strava athlete id as the key.
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; private set; }

        [Required]
        [ForeignKey("Athlete")]
        public long AthleteId { get; private set; }
        public virtual Athlete Athlete { get;  set; }

        public GearType GearType { get; private set; }

        public string Brand { get; set; }
        public string Model { get; set; }        
        public BikeType FrameType { get; set; }
        public string Description { get; set; }
        public bool? IsPrimary { get; set; }
        public string Name { get; set; }
        public decimal? Distance { get; set; }
        public int? ResourceState { get; set; }

        public static Gear CreateBike(string gearId, long athleteId)
        {
            return new Gear() { Id = gearId, GearType = GearType.Bike, AthleteId = athleteId };
        }

        public static Gear CreateShoe(string gearId, long atheleteId)
        {
            return new Gear() { Id = gearId, GearType = GearType.Shoe , AthleteId=atheleteId};
        }
    }
}
