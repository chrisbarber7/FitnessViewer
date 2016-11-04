using FitnessViewer.Infrastructure.enums;
using System;

namespace FitnessViewer.Infrastructure.Helpers
{
    public class DisplayLabel
    {
        public static string StreamTypeUnits(PeakStreamType type)
        {
            switch (type)
            {
                case PeakStreamType.Cadence: return "rpm";
                case PeakStreamType.HeartRate: return "bpm";
                case PeakStreamType.Lap: return "";
                case PeakStreamType.Power: return "watts";
                case PeakStreamType.Speed: return "mph";
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
    }
}
