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
using Strava.Common;
using Newtonsoft.Json;

namespace Strava.Activities
{
    /// <summary>
    /// A single segment returned from the segment explorer.
    /// </summary>
    public class ExplorerSegment
    {
        /// <summary>
        /// The id of the segment.
        /// </summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>
        /// The name of the segment.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// The climb category of the segment.
        /// </summary>
        [JsonProperty("climb_category")]
        public int ClimbCategory { get; set; }

        /// <summary>
        /// The climb category (descent) of the segment.
        /// </summary>
        [JsonProperty("climb_category_desc")]
        public string ClimbCategoryDesc { get; set; }

        /// <summary>
        /// The average grade of the segment.
        /// </summary>
        [JsonProperty("avg_grade")]
        public double AverageGrade { get; set; }

        /// <summary>
        /// Start coordinate.
        /// </summary>
        [JsonProperty("start_latlng")]
        private List<double> _start { get; set; }

        /// <summary>
        /// The start coordinate of the segment.
        /// </summary>
        public Coordinate Start
        {
            get
            {
                return new Coordinate(_start[0], _start[1]);
            }
        }

        /// <summary>
        /// End coordinate.
        /// </summary>
        [JsonProperty("end_latlng")]
        public List<double> _end { get; set; }

        /// <summary>
        /// The end coordinate of the segment.
        /// </summary>
        public Coordinate End
        {
            get
            {
                return new Coordinate(_end[0], _end[1]);
            }
        }

        /// <summary>
        /// The elevation difference of the segment.
        /// </summary>
        [JsonProperty("elev_difference")]
        public double ElevationDifference { get; set; }

        /// <summary>
        /// The distance of the segment.
        /// </summary>
        [JsonProperty("distance")]
        public double Distance { get; set; }

        /// <summary>
        /// The polyline of the segment. Use the PolylineDecoder to get a list of coordinates.
        /// </summary>
        [JsonProperty("points")]
        public string Polyline { get; set; }
    }
}
