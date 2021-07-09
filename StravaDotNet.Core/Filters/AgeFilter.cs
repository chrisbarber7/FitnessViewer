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
    /// This enum is used to filter a segment leaderboard. A Strava premium account is needed to filter a segment leasderboard.
    /// </summary>
    public enum AgeFilter
    {
        /// <summary>
        /// 0-24
        /// </summary>
        One,
        /// <summary>
        /// 25-34
        /// </summary>
        Two,
        /// <summary>
        /// 35-44
        /// </summary>
        Three,
        /// <summary>
        /// 45-54
        /// </summary>
        Four,
        /// <summary>
        /// 55-64
        /// </summary>
        Five,
        /// <summary>
        /// 65+
        /// </summary>
        Six,
        /// <summary>
        /// The Strava Leaderboard doesn't get filtered by age.
        /// </summary>
        All
    }
}