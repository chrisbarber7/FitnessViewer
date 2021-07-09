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

namespace Strava.Activities
{
    /// <summary>
    /// Specifies the parameter whereby the leaderboard will be sorted by.
    /// </summary>
    public enum LeaderboardParameter
    {
        /// <summary>
        /// Sorts the leaderboard by the athlete's name.
        /// </summary>
        AthleteName,
        /// <summary>
        /// Sorts the leaderboard by date.
        /// </summary>
        Date,
        /// <summary>
        /// Sorts the leaderboard by the average heartrate.
        /// </summary>
        AverageHeartrate,
        /// <summary>
        /// Sorts the leaderboard by moving time.
        /// </summary>
        MovingTime,
        /// <summary>
        /// Sorts the leaderboard by the elapsed time.
        /// </summary>
        ElapsedTime,
        /// <summary>
        /// Sorts the leaderboard by the average power.
        /// </summary>
        AveragePower
    }
}