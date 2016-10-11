using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using FitnessViewer.Infrastructure.Models;
using FitnessViewer.Infrastructure.Models.Dto;

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
        public DbSet<Measurement> Measurement { get; set; }
        public DbSet<Lap> Lap { get; set; }
        public DbSet<ActivityPeakDetail> ActivityPeakDetail { get; set; }

        public DbSet<FitbitUser> FitbitUser { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Lap>()
            //    .HasKey(l=>l.Id)
            //    .HasRequired(l=>l.Athlete)
                
            //    .WithRequiredDependent()
            //    .WillCascadeOnDelete(false);
               
                
            //modelBuilder.Entity<Activity>()
            //    .HasRequired<ActivityType>(t => t.ActivityType)
            // .WithRequiredPrincipal()
            //    .WillCascadeOnDelete(false); ;
        



            base.OnModelCreating(modelBuilder);
        }
    }
}