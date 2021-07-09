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
    /// This enum is used to filter a segment leaderboard. You need a Strava premium account to filter segment leaderboards.
    /// </summary>
    public enum WeightFilter
    {
        /// <summary>
        /// 0-124 pounds - 0-54 kg 
        /// </summary>
        One,
        /// <summary>
        /// 125-149 pounds - 55-64 kg
        /// </summary>
        Two,
        /// <summary>
        /// 150-164 pounds - 65-74 kg
        /// </summary>
        Three,
        /// <summary>
        /// 165-179 pounds - 75-84kg
        /// </summary>
        Four,
        /// <summary>
        /// 180-199 pounds - 85-94kg
        /// </summary>
        Five,
        /// <summary>
        /// 200+ pounds - 95+ kg
        /// </summary>
        Six,
        /// <summary>
        /// All riders are shown and the leaderboard doesn't get filtered by weight. Use this value if you 
        /// do not have a Strava premium account!
        /// </summary>
        All
    }
}