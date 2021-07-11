using FitnessViewer.Infrastructure.Core.Data;
using FitnessViewer.Infrastructure.Core.enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FitnessViewer.Infrastructure.Core.Interfaces;

namespace FitnessViewer.Infrastructure.Core.Models
{
    /// <summary>
    /// Lower/Upper zone ranges for a user/sport 
    /// </summary>
    [Table("ZoneRanges")]
    public class ZoneRange : Entity<int>, IEntity<int>, IUserEntity
    {
     //   public int Id { get; set; }

        [Required]
        [MaxLength(450)]
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public ZoneType ZoneType { get; set; }

        /// <summary>
        /// User defined name for a zone
        /// </summary>
        [MaxLength(32)]
        public string ZoneName { get; set; }

        /// <summary>
        /// Percenage of Zone value where the zone starts. Zone.Value (FTP) =300, ZoneStart=25 = 25% of 300 = 75 watts  is where this range starts.
        /// </summary>
        public  byte ZoneStart { get; set; }

        internal static ZoneRange CreateDefault()
        {
            return new ZoneRange()
            {
                ZoneName = "Default",
                ZoneStart = 0
            };
        }
    }
}
