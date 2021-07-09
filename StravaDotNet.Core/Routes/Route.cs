#region Copyright (C) 2015 Sascha Simon

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
using Strava.Activities;
using Strava.Athletes;
using Strava.Segments;
using System.Collections.Generic;

namespace Strava.Routes
{
    /// <summary>
    /// This class represents a route created on Strava.
    /// </summary>
    public class Route
    {
        /// <summary>
        /// The athlete who created this route.
        /// </summary>
        [JsonProperty("athlete")]
        public Athlete Athlete { get; set; }

        /// <summary>
        /// Description of the route.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Total distance of the route.
        /// </summary>
        [JsonProperty("distance")]
        public double Distance { get; set; }

        /// <summary>
        /// Total elevation gain of the route.
        /// </summary>
        [JsonProperty("elevation_gain")]
        public double Elevation { get; set; }

        /// <summary>
        /// The type of the route.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// This object contains data about the map (summary and full data).
        /// </summary>
        [JsonProperty("map")]
        public Map Map { get; set; }

        /// <summary>
        /// The name of the route.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// True if the route is private.
        /// </summary>
        [JsonProperty("@private")]
        public bool IsPrivate { get; set; }

        /// <summary>
        /// The Resource state.
        /// </summary>
        [JsonProperty("resource_state")]
        public int ResourceState { get; set; }

        /// <summary>
        /// The type of the route. See <see cref="SubType"/> class.
        /// </summary>
        public SubType SubType
        {
            get { return (SubType)sub_type; }
        }

        /// <summary>
        /// True if the route is starred.
        /// </summary>
        [JsonProperty("starred")]
        public bool IsStarred { get; set; }

        [JsonProperty("sub_type")]
        private int sub_type { get; set; }

        /// <summary>
        /// The timestamp when this route was created.
        /// </summary>
        [JsonProperty("timestamp")]
        public int Timestamp { get; set; }

        /// <summary>
        /// The type of the route.
        /// </summary>
        [JsonProperty("type")]
        public int Type { get; set; }

        /// <summary>
        /// Contains a list of all segments this route is traversing. This may be null if you load routes from another athlete.
        /// </summary>
        [JsonProperty("segments")]
        public List<Segment> Segments { get; set; }
    }
}
