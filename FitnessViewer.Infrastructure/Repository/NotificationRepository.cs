using AutoMapper;
using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using FitnessViewer.Infrastructure.enums;

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

        public IEnumerable<Models.Dto.Notification> GetUserNotifications(string userId)
        {
            var notifications = _context.UserNotification

                .Include(n=>n.Notification)
              .Include(n => n.Notification.Activity)
                .Where(un => un.UserId == userId && !un.IsRead)
              .Select(un => new Models.Dto.Notification
              {
                  Id = un.Id,
                  ActivityName = un.Notification.Activity.Name,
                  ItemsAdded = un.Notification.ItemsAdded,
                  Type = un.Notification.Type,
                  Message =""
              })
              
              .ToList();


            foreach (var notification in notifications)
                notification.Message = NotificationTypeConversion.EnumToString(notification.Type);
            

            return notifications;// notifications.Select(Mapper.Map<Notification, Models.Dto.Notification>);
        }
    }
}
