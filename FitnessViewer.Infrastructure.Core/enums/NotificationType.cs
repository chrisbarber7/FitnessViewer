using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Core.enums
{
    public enum NotificationType
    {
        Invalid = 0,
        Error = 1,
        StravaScan = 2,
        StravaActivity = 3,
        Fitbit = 4
    }

    public class NotificationTypeConversion
    {
        public static string EnumToString(NotificationType type)
        {
            switch (type)
            {
                case NotificationType.Error: { return "Error!"; }
                case NotificationType.StravaScan: { return "New Activity Scan"; }
                case NotificationType.StravaActivity: { return "Activity Download"; }
                case NotificationType.Fitbit: { return "Fitbit Metric Download"; }
                default: { return "Unknown"; }
            }
        }
    }
}

