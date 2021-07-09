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
using Strava.Activities;
using Newtonsoft.Json;

namespace Strava.Segments
{
    /// <summary>
    /// Represents a less detailed version of a segment.
    /// </summary>
    public class SegmentSummary
    {
        /// <summary>
        /// The id provided by Strava.
        /// </summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>
        /// The resource state.
        /// </summary>
        [JsonProperty("resource_state")]
        public int ResourceState { get; set; }

        /// <summary>
        /// The name of the segment.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// The type of activity.
        /// </summary>
        [JsonProperty("activity_type")]
        public string ActivityType { get; set; }

        /// <summary>
        /// The segment's distance.
        /// </summary>
        [JsonProperty("distance")]
        public float Distance { get; set; }

        /// <summary>
        /// The average grade of the segment.
        /// </summary>
        [JsonProperty("average_grade")]
        public float AverageGrade { get; set; }

        /// <summary>
        /// The max grade of the segment.
        /// </summary>
        [JsonProperty("maximum_grade")]
        public float MaxGrade { get; set; }
        
      
        private float? _maxElevation { get; set; }

        /// <summary>
        /// The segment's highest elevation.
        /// </summary>
        [JsonProperty("elevation_high")]
        public float? MaxElevation
        {
            get
            { return _maxElevation; }
            set
            {
                if (value != null)
                    _maxElevation = value;
                else
                    _maxElevation = 0;
            }
        }

        private float? _minElevation { get; set; }
        /// <summary>
        /// The segment's lowest elevation.
        /// </summary>
        ///  [JsonProperty("elevation_low")]
        public float? MinElevation
        {
            get
            { return _minElevation; }
            set
            {
                if (value != null)
                    _minElevation = value;
                else
                    _minElevation = 0;
            }
        }

        /// <summary>
        /// the climb category of the segment.
        /// </summary>
        [JsonProperty("climb_category")]
        public int Category { get; set; }

        /// <summary>
        /// The category of this climb.
        /// </summary>
        public ClimbCategory ClimbCategory
        {
            get
            {
                switch (Category)
                {
                    case 0:
                        return ClimbCategory.CategoryHc;
                    case 1:
                        return ClimbCategory.Category4;
                    case 2:
                        return ClimbCategory.Category3;
                    case 3:
                        return ClimbCategory.Category2;
                    case 4:
                        return ClimbCategory.Category1;
                    case 5:
                        return ClimbCategory.CategoryNc;
                    default:
                        return ClimbCategory.CategoryNc;
                }
            }
        }

        /// <summary>
        /// The city where this segment is located in.
        /// </summary>
        [JsonProperty("city")]
        public string City { get; set; }

        /// <summary>
        /// The state where this segment is located in.
        /// </summary>
        [JsonProperty("state")]
        public string State { get; set; }

        /// <summary>
        /// The country where this segment is located in.
        /// </summary>
        [JsonProperty("country")]
        public string Country { get; set; }

        /// <summary>
        /// True if this segment is private.
        /// </summary>
        [JsonProperty("private")]
        public Boolean IsPrivate { get; set; }

        /// <summary>
        /// True if the segment is starred by the currently authenticated athlete.
        /// </summary>
        [JsonProperty("starred")]
        public Boolean IsStarred { get; set; }
    }
}
