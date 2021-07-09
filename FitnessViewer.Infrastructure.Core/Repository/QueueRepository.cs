using FitnessViewer.Infrastructure.Core.Data;
using FitnessViewer.Infrastructure.Core.enums;
using FitnessViewer.Infrastructure.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace FitnessViewer.Infrastructure.Core.Repository
{
    public class QueueRepository
    {
        private ApplicationDb _context;

        public QueueRepository(ApplicationDb context)
        {
            _context = context;
        }
        
        public IEnumerable<DownloadQueue> GetQueue(int count, int? type)
        {

            if (type != null)
            {
                return _context.Queue.Where(x => !x.Processed && !x.HasError.Value && x.DownloadType == (DownloadType)type.Value).OrderBy(x => x.Id).Take(count).ToList();
            }
            else
            {
                return _context.Queue.Where(x => !x.Processed && !x.HasError.Value).OrderBy(x => x.Id).Take(count).ToList();
            }
            
        }

        public IEnumerable<DownloadQueue> GetFailedJob()
        {
            return _context.Queue
                           .Where(x => x.HasError.Value)
                           .OrderBy(x => x.Id)
                           .ToList();
        }
    }
}
