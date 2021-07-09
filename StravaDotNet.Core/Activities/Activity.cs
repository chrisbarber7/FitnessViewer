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

using System.Collections.Generic;
using Strava.Gear;
using Strava.Segments;
using Newtonsoft.Json;

namespace Strava.Activities
{
    /// <summary>
    /// Activities are the base object for Strava runs, rides, swims etc.
    /// </summary>
    public class Activity : ActivitySummary
    {
        /// <summary>
        /// A list of segment effort objects.
        /// </summary>
        [JsonProperty("segment_efforts")]
        public List<SegmentEffort> SegmentEfforts { get; set; }

        /// <summary>
        /// A summary of the gear used.
        /// </summary>
        [JsonProperty("gear")]
        public GearSummary Gear { get; set; }

        /// <summary>
        /// the burned kilocalories.
        /// </summary>
        [JsonProperty("calories")]
        public float Calories { get; set; }

        /// <summary>
        /// The activity's description.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// A list of best efforts for activity.
        /// </summary>
        [JsonProperty("best_efforts")]
        public List<BestEffort> BestEfforts { get; set; }

        /// <summary>
        /// he name of the device used to record the activity.
        /// </summary>
        [JsonProperty("device_name")]
        public string DeviceName { get; set; }

   
        /* ToAdd :
        splits_metric
        splits_standard
        device_name
        photos


    */
    }
}
