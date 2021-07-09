#region Copyright (C) 2014 Sascha Simon

//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see http://www.gnu.org/licenses/.
//
//  Visit the official homepage at http://www.sascha-simon.com

#endregion

using System;
using Strava.Athletes;
using Newtonsoft.Json;

namespace Strava.Activities
{
    /// <summary>
    /// Laps are triggered by athletes using their respective devices, such as Garmin watches.
    /// </summary>
    public class ActivityLap
    {
        /// <summary>
        /// The Strava id of the lap.
        /// </summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>
        /// Indicates the level of detail.
        /// </summary>
        [JsonProperty("resource_state")]
        public int ResourceState { get; set; }

        /// <summary>
        /// The name of the lap.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Contains basic information about the activity.
        /// </summary>
        [JsonProperty("activity")]
        public ActivityMeta Activity { get; set; }

        /// <summary>
        /// Contains basic information about the athlete.
        /// </summary>
        [JsonProperty("athlete")]
        public AthleteMeta Athlete { get; set; }

        /// <summary>
        /// The elapsed time of the lap in seconds.
        /// </summary>
        [JsonProperty("elapsed_time")]
        public int ElapsedTime { get; set; }

        /// <summary>
        /// The moving the of the lap in seconds.
        /// </summary>
        [JsonProperty("moving_time")]
        public int MovingTime { get; set; }

        /// <summary>
        /// Returns the moving time as a TimeSpan object rather than an int value.
        /// </summary>
        public TimeSpan MovingTimeSpan
        {
            get { return TimeSpan.FromSeconds(MovingTime); }
        }

        /// <summary>
        /// Returns the elapsed time as a TimeSpan object rather than an int value.
        /// </summary>
        public TimeSpan ElapsedTimeSpan
        {
            get { return TimeSpan.FromSeconds(ElapsedTime); }
        }

        [JsonProperty("start_date")]
        private string _start = null;

        /// <summary>
        /// The date and time when the lap was started.
        /// </summary>
        public DateTime Start
        {
            get
            {
                if (!string.IsNullOrEmpty(_start))
                    return DateTime.Parse(_start);

                return DateTime.MinValue;
            }
        }

        [JsonProperty("start_date_local")]
        private string _startLocal;

        /// <summary>
        /// The local date and time when the lap was started.
        /// </summary>
        public DateTime StartLocal
        {
            get
            {
                if (!string.IsNullOrEmpty(_startLocal))
                    return DateTime.Parse(_startLocal);

                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// The distance of the lap measured in meters.
        /// </summary>
        [JsonProperty("distance")]
        public float Distance { get; set; }

        /// <summary>
        /// The start index point of the file.
        /// </summary>
        [JsonProperty("start_index")]
        public int StartIndex { get; set; }

        /// <summary>
        /// The end index point of the file.
        /// </summary>
        [JsonProperty("end_index")]
        public int EndIndex { get; set; }

        /// <summary>
        /// Gained meters in the lap.
        /// </summary>
        [JsonProperty("total_elevation_gain")]
        public float TotalElevationGain { get; set; }

        /// <summary>
        /// The average speed measured in meters per second.
        /// </summary>
        [JsonProperty("average_speed")]
        public float AverageSpeed { get; set; }

        /// <summary>
        /// The max speed measured in meters per second.
        /// </summary>
        [JsonProperty("max_speed")]
        public float MaxSpeed { get; set; }

        /// <summary>
        /// The average cadence.
        /// </summary>
        [JsonProperty("average_cadence")]
        public float AverageCadence { get; set; }

        /// <summary>
        /// The average power measured in watts.
        /// </summary>
        [JsonProperty("average_watts")]
        public float AveragePower { get; set; }

        /// <summary>
        /// The average heartrate in beats per minute.
        /// </summary>
        [JsonProperty("average_heartrate")]
        public float AverageHeartrate { get; set; }

        /// <summary>
        /// The max heartrate in beats per minute.
        /// </summary>
        [JsonProperty("max_heartrate")]
        public float MaxHeartrate { get; set; }

        /// <summary>
        /// The index of the lap.
        /// </summary>
        [JsonProperty("lap_index")]
        public int LapIndex { get; set; }
    }
}
