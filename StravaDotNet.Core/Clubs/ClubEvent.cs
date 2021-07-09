#region Copyright (C) 201 Sascha Simon

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

using Strava.Athletes;
using Newtonsoft.Json;

namespace Strava.Clubs
{
    /// <summary>
    /// This class represents a club event.
    /// </summary>
    public class ClubEvent
    {
        /// <summary>
        /// The Strava Id of the club event.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Level of detail of the returned data.
        /// </summary>
        [JsonProperty("resource_state")]
        public int ResourceState { get; set; }

        /// <summary>
        /// The title of the club event.
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// The Strava Id of the club.
        /// </summary>
        [JsonProperty("club_id")]
        public int ClubId { get; set; }

        /// <summary>
        /// The organising athlete of the club event.
        /// </summary>
        [JsonProperty("organizing_athlete")]
        public AthleteSummary Organizer { get; set; }

        /// <summary>
        /// The type of the club event.
        /// </summary>
        [JsonProperty("activity_type")]
        public string ActivityType { get; set; }

        /// <summary>
        /// The data when the club event was crated by the organizer.
        /// </summary>
        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        /// <summary>
        /// If the club event takes place on a route that was crated using the Strava Route Builder, 
        /// this property holds the id of that route.
        /// </summary>
        [JsonProperty("route_id")]
        public int? RouteId { get; set; }

        /// <summary>
        /// True, if only women are allowed on this event.
        /// </summary>
        [JsonProperty("woman_only")]
        public bool IsWomanOnly { get; set; }

        /// <summary>
        /// True if the club event is private.
        /// </summary>
        [JsonProperty("private")]
        public bool IsPrivate { get; set; }

        /// <summary>
        /// The skill level required to participate in this club event.
        /// </summary>
        [JsonProperty("skill_levels")]
        public int SkillLevels { get; set; }

        /// <summary>
        /// The terrain of the route.
        /// </summary>
        [JsonProperty("terrain")]
        public int Terrain { get; set; }

        /// <summary>
        /// Special occurences of the club event.
        /// </summary>
        [JsonProperty("upcoming_occurences")]
        public string[] Occurences { get; set; }

        /// <summary>
        /// The meeting point of the club event, i.e. where the evewnt starts.
        /// </summary>
        [JsonProperty("address")]
        public string Address{ get; set; }
    }
}
