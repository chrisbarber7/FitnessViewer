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

namespace Strava.Statistics
{
    /// <summary>
    /// This class contains all statistics of an athlete.
    /// </summary>
    public class Stats
    {
        /// <summary>
        /// The distance of your biggest ride.
        /// </summary>
        [JsonProperty("biggest_ride_distance")]
        public double BiggestRideDistance { get; set; }

        /// <summary>
        /// The most elevation gain in a single ride.
        /// </summary>
        [JsonProperty("biggest_climb_elevation_gain")]
        public double BiggestClimbElevationGain { get; set; }

        /// <summary>
        /// Statistics about your recent rides.
        /// </summary>
        [JsonProperty("recent_ride_totals")]
        public RecentRideTotals RecentRideTotals { get; set; }

        /// <summary>
        /// Statistics about your recent runs.
        /// </summary>
        [JsonProperty("recent_run_totals")]
        public RecentRunTotals RecentRunTotals { get; set; }

        /// <summary>
        /// Ride statistics from this year.
        /// </summary>
        [JsonProperty("ytd_ride_totals")]
        public YearToDateRideTotals YearToDateRideTotals { get; set; }

        /// <summary>
        /// Run statistics from this year.
        /// </summary>
        [JsonProperty("ytd_run_totals")]
        public YearToDateRunTotals YearToDateRunTotals { get; set; }

        /// <summary>
        /// Total ride statistics.
        /// </summary>
        [JsonProperty("all_ride_totals")]
        public AllRideTotals RideTotals { get; set; }

        /// <summary>
        /// Total run statistics.
        /// </summary>
        [JsonProperty("all_run_totals")]
        public AllRunTotals RunTotals { get; set; }
    }
}
