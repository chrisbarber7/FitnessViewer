using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using FitnessViewer.Infrastructure.Models;
using FitnessViewer.Infrastructure.Interfaces;

namespace FitnessViewer.Infrastructure.Data
{
    public class ApplicationDb : IdentityDbContext<ApplicationUser>, IApplicationDb
    {
        public ApplicationDb()
            : base("FitnessViewer", throwIfV1Schema: false)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public static ApplicationDb Create()
        {
            return new ApplicationDb();
        }

        public virtual DbSet<Athlete> Athlete { get; set; }
        public virtual DbSet<Activity> Activity { get; set; }
        public virtual DbSet<DownloadQueue> Queue { get; set; }
        public virtual DbSet<BestEffort> BestEffort { get; set; }
        public virtual DbSet<Stream> Stream { get; set; }
        public virtual DbSet<ActivityPeaks> ActivityPeak { get; set; }
        public virtual DbSet<ActivityType> ActivityType { get; set; }
        public virtual DbSet<Gear> Gear { get; set; }
        public virtual DbSet<Calendar> Calendar { get; set; }
        public virtual DbSet<Lap> Lap { get; set; }
        public virtual DbSet<ActivityPeakDetail> ActivityPeakDetail { get; set; }

        public virtual DbSet<FitbitUser> FitbitUser { get; set; }
        public virtual DbSet<Metric> Metric { get; set; }

        public virtual DbSet<Notification> Notification { get; set; }
        public virtual DbSet<UserNotification> UserNotification { get; set; }

        public virtual DbSet<PeakStreamTypeDuration> PeakStreamTypeDuration { get; set; }

        public virtual DbSet<Zone> Zone { get; set; }
        public virtual DbSet<ZoneRange> ZoneRange { get; set; }

  //      public virtual DbSet<AthleteSetting> AthleteSetting { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}