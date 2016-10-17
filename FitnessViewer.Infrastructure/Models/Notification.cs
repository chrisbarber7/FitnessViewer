using FitnessViewer.Infrastructure.enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Models
{
    public class Notification
    {
        private Notification()
        {
        }

        public int Id { get; set; }

        public DateTime Created { get; set; }
        public NotificationType Type { get; set; }

        [ForeignKey("Activity")]
        public long? ActivityId { get; set; }
        public virtual Activity Activity { get; set; }

        public int ItemsAdded { get; set; }

        public static Notification StravaActivityDownload(long activityId)
        {
            Notification n = new Notification();
            n.Created = DateTime.Now;
            n.Type = NotificationType.StravaActivity;
            n.ActivityId = activityId;
            return n;
        }

        public static Notification StravaActivityScan(int itemsAdded)
        {
            Notification n = new Notification();
            n.Created = DateTime.Now;
            n.Type = NotificationType.StravaScan;
            n.ActivityId = null;
            n.ItemsAdded = itemsAdded;
            return n;
        }

        public static Notification FitbitDownload(int metricsAdded)
        {
            Notification n = new Notification();
            n.Created = DateTime.Now;
            n.Type = NotificationType.Fitbit;
            n.ActivityId = null;
            n.ItemsAdded = metricsAdded;
            return n;
        }

        public string GetLinkAddress()
        {
            if (ActivityId == null)
                return "";

            return string.Format("/Activity/ViewActivity/{0}", ActivityId.ToString());

        }
    }
}
