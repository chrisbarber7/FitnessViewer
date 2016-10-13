using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Repository
{
    public class NotificationRepository
    {
        private ApplicationDb _context;

                public NotificationRepository(ApplicationDb context)
        {
            _context = context;
        }

        public void Add(UserNotification un)
        {
            _context.Notification.Add(un.Notification);
            _context.UserNotification.Add(un);
        }
    }
}
