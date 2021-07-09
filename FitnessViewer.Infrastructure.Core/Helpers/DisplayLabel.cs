using FitnessViewer.Infrastructure.Core.enums;
using System;

namespace FitnessViewer.Infrastructure.Core.Helpers
{
    public class DisplayLabel
    {

        public static string PeakStreamTypeUnits(PeakStreamType type)
        {
            switch (type)
            {
                case PeakStreamType.Cadence: return "rpm";
                case PeakStreamType.HeartRate: return "bpm";
                case PeakStreamType.Lap: return "";
                case PeakStreamType.Power: return "w";
                case PeakStreamType.Speed: return "mph";
                case PeakStreamType.Elevation: return "ft";
                default: return "";
            }
        }

        public static string StreamDurationForDisplay(int duration)
        {
            if (duration == int.MaxValue)
                return "Activity";

            TimeSpan time = TimeSpan.FromSeconds(duration);

            if (time.Hours > 0)
                return time.ToString(@"hh\:mm\:ss");

            if (time.Minutes > 0 && time.Seconds > 0)
                return string.Format("{0} min {1} secs", time.Minutes.ToString(), time.Seconds.ToString());

            if (time.Minutes > 0 && time.Seconds == 0)
                return string.Format("{0} min", time.Minutes);

            return string.Format("{0} secs", duration.ToString());
        }


        public static string ShortStreamDurationForDisplay(int duration)
        {
            if (duration == int.MaxValue)
                return "Activity";

            TimeSpan time = TimeSpan.FromSeconds(duration);

            if (time.Hours > 0)
            {
                if (time.Seconds == 0)
                    return string.Format("{0}m", time.TotalMinutes.ToString());
                else
                    return time.ToString(@"hh\:mm\:ss");
            }
            if (time.Minutes > 0 && time.Seconds > 0)
                return string.Format("{0}m {1}s", time.Minutes.ToString(), time.Seconds.ToString());

            if (time.Minutes > 0 && time.Seconds == 0)
                return string.Format("{0}m", time.Minutes);

            return string.Format("{0}s", duration.ToString());
        }
    }
}
