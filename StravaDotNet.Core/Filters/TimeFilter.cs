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

namespace Strava.Filters
{
    /// <summary>
    /// Used to filter a segment leaderboard. Valid values from Strava are:
    /// this_year
    /// this_month
    /// this_week
    /// today
    /// </summary>
    public enum TimeFilter
    {
        /// <summary>
        /// Show efforts from this year.
        /// </summary>
        ThisYear,
        /// <summary>
        /// Show efforts from this month.
        /// </summary>
        ThisMonth,
        /// <summary>
        /// Show efforts from this week.
        /// </summary>
        ThisWeek,
        /// <summary>
        /// Show efforts from today.
        /// </summary>
        Today,
        /// <summary>
        /// The leaderboard does not get filtered by date.
        /// </summary>
        All
    }
}