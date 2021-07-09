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
using Strava.Clubs;
using Strava.Gear;
using Newtonsoft.Json;

namespace Strava.Athletes
{
    /// <summary>
    /// Represents a Strava athlete.
    /// </summary>
    public class Athlete : AthleteSummary
    {
        /// <summary>
        /// The count of the athlete's followers.
        /// </summary>
        [JsonProperty("follower_count")]
        public int FollowerCount { get; set; }

        /// <summary>
        /// The count of the athlete's friends.
        /// </summary>
        [JsonProperty("friend_count")]
        public int FriendCount { get; set; }

        /// <summary>
        /// The weight of the athlete.
        /// </summary>
        [JsonProperty("weight")]
        public float? Weight { get; set; }

        /// <summary>
        /// The count of the athlete's friends that both this athlete and the currently authenticated athlete are following.
        /// </summary>
        [JsonProperty("mutual_friend_count")]
        public int MutualFriendCount { get; set; }

        /// <summary>
        /// The date preference. ISO 8601 time string.
        /// </summary>
        [JsonProperty("date_preference")]
        public string DatePreference { get; set; }

        /// <summary>
        /// Either 'feet' or 'meters'
        /// </summary>
        [JsonProperty("measurement_preference")]
        public string MeasurementPreference { get; set; }

        /// <summary>
        /// The email address.
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// The functional threshold power.
        /// </summary>
        [JsonProperty("ftp")]
        public int? Ftp { get; set; }

        /// <summary>
        /// A list of the athlete's bikes.
        /// </summary>
        [JsonProperty("bikes")]
        public List<Bike> Bikes { get; set; }

        /// <summary>
        /// A list of the athlete's shoes.
        /// </summary>
        [JsonProperty("shoes")]
        public List<Shoes> Shoes { get; set; }

        /// <summary>
        /// A list of the athlete's clubs.
        /// </summary>
        [JsonProperty("clubs")]
        public List<Club> Clubs { get; set; }

        /// <summary>
        /// athlete’s default sport type: 0 = cyclist, 1 = runner
        /// </summary>
        [JsonProperty("athlete_type")]
        public int AthleteType { get; set; }
    }
}
