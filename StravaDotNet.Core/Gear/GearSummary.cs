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

namespace Strava.Gear
{
    /// <summary>
    /// Represents a piece of gear (Bike or Shoes). This object only contains summary details.
    /// </summary>
    public class GearSummary
    {
        /// <summary>
        /// The gear's id.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// True if this is the primary gear.
        /// </summary>
        [JsonProperty("primary")]
        public Boolean IsPrimary { get; set; }

        /// <summary>
        /// Gear's name. Athlete entered for bikes, generated from brand and model for shoes
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Distance travelled with gear in meters.
        /// </summary>
        [JsonProperty("distance")]
        public float Distance { get; set; }

        /// <summary>
        /// Resource state / level of details.
        /// </summary>
        [JsonProperty("resource_state")]
        public int ResourceState { get; set; }
    }
}
