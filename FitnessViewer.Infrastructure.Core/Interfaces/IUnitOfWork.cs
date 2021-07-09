using FitnessViewer.Infrastructure.Core.Data;
using FitnessViewer.Infrastructure.Core.Repository;

namespace FitnessViewer.Infrastructure.Core.Interfaces
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
