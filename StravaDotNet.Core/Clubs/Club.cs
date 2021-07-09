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
    /// This class represents a Strava Club.
    /// </summary>
    public class Club : ClubSummary
    {
        /// <summary>
        /// The club's description.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// The club's type.
        /// </summary>
        [JsonProperty("club_type")]
        private string _clubType { get; set; }

        /// <summary>
        /// The club's type.
        /// </summary>
        public ClubType ClubType
        {
            get
            {
                if (_clubType.Equals("casual_club"))
                    return ClubType.Casual;
                if (_clubType.Equals("racing_team"))
                    return ClubType.RacingTeam;
                if (_clubType.Equals("shop"))
                    return ClubType.Shop;
                if (_clubType.Equals("company"))
                    return ClubType.Company;

                return ClubType.Other;
            }
        }

        /// <summary>
        /// The sports type of the club.
        /// </summary>
        [JsonProperty("sport_type")]
        private string _sportType { get; set; }

        /// <summary>
        /// The sports type of the club.
        /// </summary>
        public SportType SportType
        {
            get
            {
                if (_sportType.Equals("cycling"))
                    return SportType.Cycling;
                if (_sportType.Equals("running"))
                    return SportType.Running;
                if (_sportType.Equals("triathlon"))
                    return SportType.Triathlon;

                return SportType.Other;
            }
        }

        /// <summary>
        /// the club's city.
        /// </summary>
        [JsonProperty("city")]
        public string City { get; set; }

        /// <summary>
        /// The club's state.
        /// </summary>
        [JsonProperty("state")]
        public string State { get; set; }

        /// <summary>
        /// The club's country.
        /// </summary>
        [JsonProperty("country")]
        public string Country { get; set; }

        /// <summary>
        /// True if the club is a private club.
        /// </summary>
        [JsonProperty("private")]
        public Boolean IsPrivate { get; set; }

        /// <summary>
        /// The club's member count.
        /// </summary>
        [JsonProperty("member_count")]
        public int MemberCount { get; set; }
    }
}
