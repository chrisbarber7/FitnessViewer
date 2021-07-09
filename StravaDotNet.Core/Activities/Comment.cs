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

using Strava.Athletes;
using Newtonsoft.Json;

namespace Strava.Activities
{
    /// <summary>
    /// Represents a comment of an activity.
    /// </summary>
    public class Comment
    {
        /// <summary>
        /// The comment id.
        /// </summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>
        /// The resource state.
        /// </summary>
        [JsonProperty("resource_state")]
        public int ResourceState { get; set; }

        /// <summary>
        /// The activity to which the comment was posted.
        /// </summary>
        [JsonProperty("activity_id")]
        public long ActivityId { get; set; }

        /// <summary>
        /// The comment's text.
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; }

        /// <summary>
        /// The athlete who wrote the comment.
        /// </summary>
        [JsonProperty("athlete")]
        public Athlete Athlete { get; set; }

        /// <summary>
        /// The time when the comment was crated.
        /// </summary>
        [JsonProperty("created_at")]
        public string TimeCreated { get; set; }
    }
}
