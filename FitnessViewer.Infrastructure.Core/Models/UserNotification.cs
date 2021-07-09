using FitnessViewer.Infrastructure.Core.Data;
using FitnessViewer.Infrastructure.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Core.Models
{
    public class UserNotification : Entity<int>, IEntity<int>, IUserEntity
    {
        private UserNotification()
        {
        }

    //    public int Id { get; set; }

        [Required]
        [MaxLength(450)]
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        [ForeignKey("Notification")]
        public int NotificationId { get; set; }
        public virtual Notification Notification { get; set; }

        public bool IsRead { get; set; }

        public UserNotification(string userId, Notification notification)
        {
            if (userId == null)
                throw new ArgumentNullException("userId");

            if (notification == null)
                throw new ArgumentNullException("notification");

            UserId = userId;
            Notification = notification;
        }

        public void MarkAsRead()
        {
            this.IsRead = true;
        }
    }
}