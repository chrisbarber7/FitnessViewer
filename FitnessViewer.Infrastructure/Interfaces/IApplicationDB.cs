using System.Data.Entity;
using FitnessViewer.Infrastructure.Models;

namespace FitnessViewer.Infrastructure.Interfaces
{
    public interface IApplicationDb
    {
        DbSet<Activity> Activity { get; set; }
        DbSet<ActivityPeaks> ActivityPeak { get; set; }
        DbSet<ActivityPeakDetail> ActivityPeakDetail { get; set; }
        DbSet<ActivityType> ActivityType { get; set; }
        DbSet<Athlete> Athlete { get; set; }
        DbSet<BestEffort> BestEffort { get; set; }
        DbSet<Calendar> Calendar { get; set; }
        DbSet<FitbitUser> FitbitUser { get; set; }
        DbSet<Gear> Gear { get; set; }
        DbSet<Lap> Lap { get; set; }
        DbSet<Metric> Metric { get; set; }
        DbSet<Notification> Notification { get; set; }
        DbSet<PeakStreamTypeDuration> PeakStreamTypeDuration { get; set; }
        DbSet<DownloadQueue> Queue { get; set; }
        DbSet<Stream> Stream { get; set; }
        DbSet<UserNotification> UserNotification { get; set; }
        DbSet<Zone> Zone { get; set; }
        DbSet<ZoneRange> ZoneRange { get; set; }
    }
}