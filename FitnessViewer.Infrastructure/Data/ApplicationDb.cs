using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using FitnessViewer.Infrastructure.Models;

namespace FitnessViewer.Infrastructure.Data
{
    public class ApplicationDb : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDb()
            : base("FitnessViewer", throwIfV1Schema: false)
        {
        }

        public static ApplicationDb Create()
        {
            return new ApplicationDb();
        }

        public DbSet<StravaAthlete> StravaAthlete { get; set; }
        public DbSet<StravaActivity> StravaActivity { get; set; }
        public DbSet<StravaQueue> Queue { get; set; }
        public DbSet<StravaBestEffort> StravaBestEffort { get; set; }
        public DbSet<StravaStream> StravaStream { get; set; }
        public DbSet<StravaActivityPeaks> StravaActivityPeak { get; set; }
    }
}