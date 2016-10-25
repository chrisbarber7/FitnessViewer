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

        public DbSet<Athlete> Athlete { get; set; }
        public DbSet<Activity> Activity { get; set; }
        public DbSet<DownloadQueue> Queue { get; set; }
        public DbSet<BestEffort> BestEffort { get; set; }
        public DbSet<Stream> Stream { get; set; }
        public DbSet<ActivityPeaks> ActivityPeak { get; set; }
        public DbSet<ActivityType> ActivityType { get; set; }
        public DbSet<Gear> Gear { get; set; }
        public DbSet<Calendar> Calendar { get; set; }
        public DbSet<Lap> Lap { get; set; }
        public DbSet<ActivityPeakDetail> ActivityPeakDetail { get; set; }

        public DbSet<FitbitUser> FitbitUser { get; set; }
        public DbSet<Metric> Metric { get; set; }

        public DbSet<Notification> Notification { get; set; }
        public DbSet<UserNotification> UserNotification { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}