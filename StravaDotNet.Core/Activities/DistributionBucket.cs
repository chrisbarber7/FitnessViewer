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

using Newtonsoft.Json;

namespace Strava.Activities
{
    /// <summary>
    /// This class represents a distribution bucket. It holds information about the max and min value and the time 
    /// spent in this zone.
    /// </summary>
    public class DistributionBucket
    {
        /// <summary>
        /// Maxvalue of the bucket.
        /// </summary>
        [JsonProperty("max")]
        public int Maximum { get; set; }

        /// <summary>
        /// Min value of the bucket.
        /// </summary>
        [JsonProperty("min")]
        public int Minimum { get; set; }

        /// <summary>
        /// Time spent in this zone.
        /// </summary>
        [JsonProperty("time")]
        public int Time { get; set; }
    }
}
