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
using Newtonsoft.Json;

namespace Strava.Activities
{
    /// <summary>
    /// Represents a Strava activity zone.
    /// </summary>
    public class ActivityZone
    {
        /// <summary>
        /// The score of the activity zone.
        /// </summary>
        [JsonProperty("score")]
        public int Score { get; set; }

        /// <summary>
        /// The list of distribution buckets of the activity zone.
        /// </summary>
        [JsonProperty("distribution_buckets")]
        public List<DistributionBucket> Buckets { get; set; }

        /// <summary>
        /// The type of the zone.
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// Resource state.
        /// </summary>
        [JsonProperty("resource_state")]
        public int ResourceState { get; set; }

        /// <summary>
        /// True if the data is sensor based.
        /// </summary>
        [JsonProperty("sensor_based")]
        public bool IsSensorBased { get; set; }

        /// <summary>
        /// Points in this zone.
        /// </summary>
        [JsonProperty("points")]
        public int Points { get; set; }

        /// <summary>
        /// Custom zones.
        /// </summary>
        [JsonProperty("custom_zones")]
        public bool IsCustomZones { get; set; }

        /// <summary>
        /// The max value of the activity zone.
        /// </summary>
        [JsonProperty("max")]
        public int Max { get; set; }
    }
}
