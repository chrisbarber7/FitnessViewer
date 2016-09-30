using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.Models;

namespace FitnessViewer.Infrastructure.Repository
{
    public class MetricsRepository
    {
        private ApplicationDb _context;

        public MetricsRepository(ApplicationDb context)
        {
            _context = context;
        }

        public void AddMeasurement(Measurement m)
        {
            _context.Measurement.Add(m);
        }
    }
}
