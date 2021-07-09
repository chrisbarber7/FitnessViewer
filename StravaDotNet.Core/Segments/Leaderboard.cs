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
using Newtonsoft.Json;

namespace Strava.Activities
{
    /// <summary>
    /// Represents the leaderboard of a segment.
    /// </summary>
    public class Leaderboard
    {
        /// <summary>
        /// The number of efforts.
        /// </summary>
        [JsonProperty("effort_count")]
        public int EffortCount { get; set; }

        /// <summary>
        /// The number of entries in the leaderboard.
        /// </summary>
        [JsonProperty("entry_count")]
        public int EntryCount { get; set; }

        /// <summary>
        /// A list of the entries of the leaderboard.
        /// </summary>
        [JsonProperty("entries")]
        public List<LeaderboardEntry> Entries { get; set; }

        /// <summary>
        /// Initializes a new instance of the Leaderboard class.
        /// </summary>
        public Leaderboard()
        {
            Entries = new List<LeaderboardEntry>();
        }
    }
}
