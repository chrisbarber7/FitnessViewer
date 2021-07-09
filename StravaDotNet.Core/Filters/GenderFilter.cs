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
    /// Use this enum to filter a Strava segment leaderboard. You can only filter the leaderboard by gender, if you have a Strava 
    /// premium account. If you do not have one, use the value "Both".
    /// </summary>
    public enum GenderFilter
    {
        /// <summary>
        /// Female
        /// </summary>
        Female,
        /// <summary>
        /// Male
        /// </summary>
        Male,
        /// <summary>
        /// Both male and female riders are shown in the leaderboard. Use this value if you do not have a Strava 
        /// premium account!
        /// </summary>
        Both
    }
}