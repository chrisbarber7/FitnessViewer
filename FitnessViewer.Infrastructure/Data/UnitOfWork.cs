using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitnessViewer.Infrastructure.Repository;
using System.Data.Entity.Validation;

namespace FitnessViewer.Infrastructure.Data
{
    public class UnitOfWork
    {
        private ApplicationDb _context;
        internal ActivityRepository Activity { get; private set; }
  
        public MetricsRepository Metrics { get; private set; }
        public QueueRepository Queue { get; private set; }
        public NotificationRepository Notification { get; private set; }
        public SettingsRepository Settings { get; private set; }
        public GenericRepository CRUDRepository { get; set; }

        public UnitOfWork()
        {
            _context = new ApplicationDb();
            Activity = new ActivityRepository(_context);
            Metrics = new MetricsRepository(_context);
            Queue = new QueueRepository(_context);
            Notification = new NotificationRepository(_context);
            Settings = new SettingsRepository(_context);
            CRUDRepository = new GenericRepository(_context);
        }

        public void Complete()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    System.Diagnostics.Debug.WriteLine("Validation errors for Entity: \"{0}\" State: \"{1}\"", 
                            eve.Entry.Entity.GetType().Name, 
                            eve.Entry.State);

                    foreach (var ve in eve.ValidationErrors)
                    {
                        System.Diagnostics.Debug.WriteLine("Property: \"{0}\" Value: \"{1}\" Error: \"{2}\"",
                            ve.PropertyName,
                            eve.Entry.CurrentValues.GetValue<object>(ve.PropertyName),
                            ve.ErrorMessage);
                    }
                }
                throw;
            }

        }
    }
}
