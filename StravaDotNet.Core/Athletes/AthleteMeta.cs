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

using Newtonsoft.Json;

namespace Strava.Athletes
{
    /// <summary>
    /// This class represents an athlete.
    /// </summary>
    public class AthleteMeta
    {
        /// <summary>
        /// The Strava athlete id.
        /// </summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>
        /// The representation of the Athlete object:
        /// 1 - meta
        /// 2 - summary
        /// 3 - detailed
        /// Listed in increasing levels of detail.
        /// </summary>
        [JsonProperty("resource_state")]
        public int ResourceState { get; set; }
    }
}
