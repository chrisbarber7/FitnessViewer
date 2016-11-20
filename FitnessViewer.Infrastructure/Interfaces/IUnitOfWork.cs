using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.Repository;

namespace FitnessViewer.Infrastructure.Interfaces
{
    public interface IUnitOfWork
    {
         void Complete();
         GenericRepository CRUDRepository { get; set; }

         ActivityRepository Activity { get;  set; }

         MetricsRepository Metrics { get;  set; }
         QueueRepository Queue { get;  set; }
         NotificationRepository Notification { get;  set; }
         SettingsRepository Settings { get;  set; }
    }
}
