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

        public IEnumerable<Models.Dto.NotificationDto> GetUnreadNotifications(string userId)
        {
            var notifications = _context.UserNotification
                .Include(n=>n.Notification)
                .Include(n => n.Notification.Activity)
                .Where(un => un.UserId == userId && !un.IsRead)
                .Select(un => new Models.Dto.NotificationDto
                {
                    Id = un.Id,
                    ActivityName = un.Notification.Activity.Name,
                    ActivityId = un.Notification.ActivityId,
                    ItemsAdded = un.Notification.ItemsAdded,
                    ActivityLink = "/Activity/ViewActivity/" + un.Notification.ActivityId,
                    Type = un.Notification.Type,
                    Message =""
                })
                .ToList();
            
            foreach (var notification in notifications)
                notification.Message = NotificationTypeConversion.EnumToString(notification.Type);
            
            return notifications;
        }

        public IEnumerable<Models.UserNotification> GetUserNotifications(string userId)
        {
            return _context.UserNotification
                .Where(un => un.UserId == userId && !un.IsRead)
                .ToList();
        }
    }
}
