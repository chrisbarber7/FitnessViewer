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

namespace Strava.Clubs
{
    /// <summary>
    /// A summary of a Strava Club. The ClubSummary contains less information than a Club.
    /// </summary>
    public class ClubSummary
    {
        /// <summary>
        /// The id of the club. The id is provided by Strava and can't be changed.
        /// </summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>
        /// The level of detail of the Club.
        /// </summary>
        [JsonProperty("resource_state")]
        public int ResourceState { get; set; }

        /// <summary>
        /// The name of the club.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Url to a 62x62 pixel picture of the club. Use the ImageLoader class to load the picture.
        /// </summary>
        [JsonProperty("profile_medium")]
        public string ProfileMedium { get; set; }

        /// <summary>
        /// Url to a 124x124 pixel picture of the club. Use the ImageLoader class to load the picture.
        /// </summary>
        [JsonProperty("profile")]
        public string Profile { get; set; }
    }
}
