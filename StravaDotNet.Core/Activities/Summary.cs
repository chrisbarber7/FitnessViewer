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
using System.Collections.Generic;

namespace Strava.Activities
{
    /// <summary>
    /// This class represents a summary.
    /// </summary>
    public class Summary
    {
        /// <summary>
        /// The distance the athlete has ridden this week.
        /// </summary>
        public float RideDistance { get; set; }

        /// <summary>
        /// The start of the summary.
        /// </summary>
        public DateTime Start { get; set; }

        /// <summary>
        /// The end of the summary.
        /// </summary>
        public DateTime End { get; set; }

        /// <summary>
        /// The duration of the summary.
        /// </summary>
        public TimeSpan Duration
        {
            get
            {
                return End - Start;
            }
        }

        /// <summary>
        /// The distance the athlete has run this week.
        /// </summary>
        public float RunDistance { get; set; }

        /// <summary>
        /// All of the rides this week.
        /// </summary>
        public List<ActivitySummary> Rides { get; set; }

        /// <summary>
        /// All of the runs this week.
        /// </summary>
        public List<ActivitySummary> Runs { get; set; }

        /// <summary>
        /// The list of activities other than rides or runs.
        /// </summary>
        public List<ActivitySummary> OtherActivities { get; set; }

        /// <summary>
        /// The total time spent this week.
        /// </summary>
        public TimeSpan TotalTime { get; set; }

        /// <summary>
        /// Returns the total count of activities
        /// </summary>
        public int ActivityCount
        {
            get
            {
                return Rides.Count + Runs.Count + OtherActivities.Count;
            }
        }

        /// <summary>
        /// Initializes a new instance of the WeeklyProgress class.
        /// </summary>
        public Summary()
        {
            Rides = new List<ActivitySummary>();
            Runs = new List<ActivitySummary>();
            OtherActivities = new List<ActivitySummary>();
            TotalTime = new TimeSpan();
        }

        /// <summary>
        /// Adds a run to the list of runs.
        /// </summary>
        /// <param name="activity">The run to add.</param>
        public void AddRun(ActivitySummary activity)
        {
            Runs.Add(activity);
        }

        /// <summary>
        /// Adds a ride to the list of rides.
        /// </summary>
        /// <param name="activity">The ride to add.</param>
        public void AddRide(ActivitySummary activity)
        {
            Rides.Add(activity);
        }

        /// <summary>
        /// Adds the time to the total time spent on activities.
        /// </summary>
        /// <param name="time">The time to add.</param>
        public void AddTime(TimeSpan time)
        {
            TotalTime = TotalTime.Add(time);
        }

        /// <summary>
        /// Adds the activity to the list of activities.
        /// </summary>
        /// <param name="other">The activity to add.</param>
        public void AddActivity(ActivitySummary other)
        {
            OtherActivities.Add(other);
        }
    }
}
