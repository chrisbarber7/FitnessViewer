using FitnessViewer.Infrastructure.enums;
using FitnessViewer.Infrastructure.Helpers.Conversions;
using FitnessViewer.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Helpers
{
    public class StreamTypeHelper
    {
        /// <summary>
        /// Return possible stream types for activity 
        /// </summary>
        /// <param name="activity">Activity Type</param>
        /// <returns>Possible StreamType's</returns>
        public static StreamType SportStreams(ActivityType activity)
        {
            if (activity.IsRide)
            {
                return StreamType.Altitude | StreamType.Cadence | StreamType.Heartrate | StreamType.Velocity | StreamType.Watts | StreamType.Temp;
            }
            else if (activity.IsRun)
            {
                return StreamType.Altitude | StreamType.Cadence | StreamType.Heartrate | StreamType.Temp |  StreamType.Pace;
            }
            else if (activity.IsSwim)
            {
                return StreamType.Cadence | StreamType.Heartrate | StreamType.Velocity | StreamType.Watts;
            }
            else if (activity.IsOther)
            {
                return StreamType.Altitude | StreamType.Cadence | StreamType.Heartrate | StreamType.Temp | StreamType.Velocity | StreamType.Watts;
            }

            return StreamType.Distance;
        }

        /// <summary>
        /// Priortise streams.  Used to determine the order in which streams are displayed.
        /// </summary>
        /// <param name="streamType">Stream Type to priortise</param>
        /// <returns>Priority (low = first)</returns>
        public static byte Priority(StreamType streamType)
        {
            switch (streamType)
            {
                case StreamType.Watts: { return 1; }
                case StreamType.Heartrate: { return 2; }
                case StreamType.Cadence: { return 3; }
                case StreamType.Velocity: { return 4; }
                case StreamType.Pace: { return 5; }
                case StreamType.Altitude: { return 6; }
                case StreamType.Temp: { return 7; }

                case StreamType.Time: { return 10; }
                case StreamType.Latitude: { return 11; }
                case StreamType.Longitude: { return 12; }
                case StreamType.Distance: { return 13; }
                case StreamType.Moving: { return 14; }
                case StreamType.Gradient: { return 15; }
                default: { return byte.MaxValue; }
            }
        }

        /// <summary>
        /// Get a readable name for a stream
        /// </summary>
        /// <param name="type">Stream Type</param>
        /// <returns></returns>
        public static string Name(StreamType type)
        {
            switch (type)
            {
                case StreamType.Time: { return "Time"; }
                case StreamType.Latitude: { return "Latitude"; }
                case StreamType.Longitude: { return "Longitude"; }
                case StreamType.Distance: { return "Distance"; }
                case StreamType.Altitude: { return "Elevation"; }
                case StreamType.Velocity: { return "Speed"; }
                case StreamType.Pace: { return "Pace"; }
                case StreamType.Heartrate: { return "Heart Rate"; }
                case StreamType.Cadence: { return "Cadence"; }
                case StreamType.Watts: { return "Power"; }
                case StreamType.Temp: { return "Temperature"; }
                case StreamType.Moving: { return "Moving"; }
                case StreamType.Gradient: { return "Gradient"; }
                default: return "";
            }
        }

        public static string Units(StreamType type)
        {
            switch (type)
            {
                case StreamType.Time: { return "secs"; }
                case StreamType.Latitude: { return ""; }
                case StreamType.Longitude: { return ""; }
                case StreamType.Distance: { return "m"; }
                case StreamType.Altitude: { return "ft"; }
                case StreamType.Velocity: { return "mph"; }
                case StreamType.Pace: { return "min/mi"; }
                case StreamType.Heartrate: { return "bpm"; }
                case StreamType.Cadence: { return "rpm"; }
                case StreamType.Watts: { return "w"; }
                case StreamType.Temp: { return "c"; }
                case StreamType.Moving: { return ""; }
                case StreamType.Gradient: { return ""; }
                default: return "";
            }
        }


        /// <summary>
        /// Convert standard Strava Stream units into User units (eg Strava Velocity is Meters per second, user units is miles per hour).
        /// </summary>
        /// <param name="streamType">Stream Type</param>
        /// <param name="value">Strava stream value</param>
        /// <returns>Users perferred units</returns>
        public static string ConvertToUserUnits(StreamType streamType, decimal value)
        {
            switch (streamType)
            {
                case StreamType.Time: { return value.ToString(); }
                case StreamType.Latitude: { return value.ToString(); }
                case StreamType.Longitude: { return value.ToString(); }
                case StreamType.Distance: { return value.ToString(); }
                case StreamType.Altitude: { return value.ToFeet().ToString("N0"); }
                case StreamType.Velocity: { return Distance.MetrePerSecondToMilesPerHour(value).ToString("N1"); }
                case StreamType.Pace: { return TimeSpan.FromSeconds( Distance.MetrePerSecondToSecondPerMile(value)).ToString(); }
                case StreamType.Heartrate: { return value.ToString("N0"); }
                case StreamType.Cadence: { return value.ToString("N0"); }
                case StreamType.Watts: { return value.ToString("N0"); }
                case StreamType.Temp: { return value.ToString("N1"); }
                case StreamType.Moving: { return value.ToString(); }
                case StreamType.Gradient: { return value.ToString(); }
                default: return "";
            }
        }

    }
}
