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
using Newtonsoft.Json;

namespace Strava.Activities
{
    /// <summary>
    /// This class contains information about a single leaderboard entry.
    /// </summary>
    public class LeaderboardEntry
    {
        /// <summary>
        /// The full name of the athlete.
        /// </summary>
        [JsonProperty("athlete_name")]
        public string AthleteName { get; set; }

        /// <summary>
        /// The athlete id. Use this id to load additional information about the athlete.
        /// </summary>
        [JsonProperty("athlete_id")]
        public long AthleteId { get; set; }

        /// <summary>
        /// The athlete's gender.
        /// </summary>
        [JsonProperty("athlete_gender")]
        public string AthleteGender { get; set; }

        /// <summary>
        /// The average heartrate.
        /// </summary>
        [JsonProperty("average_hr")]
        public float? AverageHeartrate { get; set; }

        /// <summary>
        /// The average power.
        /// </summary>
        [JsonProperty("average_watts")]
        public float? AveragePower { get; set; }

        /// <summary>
        /// The distance ridden on this single attempt.
        /// </summary>
        [JsonProperty("distance")]
        public float Distance { get; set; }

        /// <summary>
        /// The elapsed time.
        /// </summary>
        [JsonProperty("elapsed_time")]
        public int ElapsedTime { get; set; }

        /// <summary>
        /// The moving time.
        /// </summary>
        [JsonProperty("moving_time")]
        public int MovingTime { get; set; }

        /// <summary>
        /// The start date.
        /// </summary>
        [JsonProperty("start_date")]
        public string StartDate { get; set; }

        /// <summary>
        /// Local start date.
        /// </summary>
        [JsonProperty("start_date_local")]
        public string StartDateLocal { get; set; }

        /// <summary>
        /// Returns the StartDate-Property as a DateTime object.
        /// </summary>
        public DateTime DateTimeStart
        {
            get { return DateTime.Parse(StartDate); }
        }

        /// <summary>
        /// Returns the StartDateLocal-Property as a DateTime object.
        /// </summary>
        public DateTime DateTimeStartLocal
        {
            get { return DateTime.Parse(StartDateLocal); }
        }

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

        /// <summary>
        /// The activity id. use this id to load additional information about the activity.
        /// </summary>
        [JsonProperty("activity_id")]
        public long ActivityId { get; set; }

        /// <summary>
        /// The effort id. Use this id to load additional information about the segment effort.
        /// </summary>
        [JsonProperty("effort_id")]
        public long EffortId { get; set; }

        /// <summary>
        /// The rank of athlete.
        /// </summary>
        [JsonProperty("rank")]
        public int Rank { get; set; }

        /// <summary>
        /// Url to a picture of the athlete. If not null or empty, you can use the ImageLoader to load the picture from the Url.
        /// </summary>
        [JsonProperty("athlete_profile")]
        public string AthleteProfile { get; set; }

        /// <summary>
        /// Returns the total seconds in a more convenient TimeSpan object.
        /// </summary>
        public TimeSpan Time
        {
            get
            {
                return TimeSpan.FromSeconds(ElapsedTime);
            }
        }

        /// <summary>
        /// Returns the information about the entry as a string.
        /// </summary>
        /// <returns>Entry as string.</returns>
        public override string ToString()
        {
            return string.Format("{0}:\t{1}:{2}:{3}\t{4}",
                Rank,
                Time.Hours.ToString("D2"),
                Time.Minutes.ToString("D2"),
                Time.Seconds.ToString("D2"),
                AthleteName
                );
        }
    }
}
