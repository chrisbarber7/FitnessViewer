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
    /// This class contains information about the route of an activity.
    /// </summary>
    public class Map
    {
        /// <summary>
        /// The map id.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// The map's polyline. This polyline can be converted to coordinates.
        /// </summary>
        [JsonProperty("polyline")]
        public string Polyline { get; set; }

        /// <summary>
        /// A summary of the polyline.
        /// </summary>
        [JsonProperty("summary_polyline")]
        public string SummaryPolyline { get; set; }

        /// <summary>
        /// The resource state gives information about the level of details of the map.
        /// </summary>
        [JsonProperty("resource_state")]
        public int ResourceState { get; set; }
    }
}
