using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitnessViewer.Infrastructure.Repository;

namespace FitnessViewer.Infrastructure.Data
{
    public class UnitOfWork
    {
        private ApplicationDb _context;
        public ActivityRepository Activity { get; private set; }
        public AnalysisRepository Analysis { get; private set; }
        public AthleteRepository Athlete { get; private set; }
        public MetricsRepository Metrics { get; private set; }
        public QueueRepository Queue { get; private set; }

        public UnitOfWork()
        {
            _context = new ApplicationDb();
            Activity = new ActivityRepository(_context);
            Analysis = new AnalysisRepository(_context);
            Athlete = new AthleteRepository(_context);
            Metrics = new MetricsRepository(_context);
            Queue = new QueueRepository(_context);
        }

        public void Complete()
        {
 
                _context.SaveChanges();
      
 
        }
    }
}
