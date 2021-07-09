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

using System;

namespace Strava.Utilities
{
    /// <summary>
    /// This class is used to convert dates.
    /// </summary>
    public static class DateConverter
    {
        /// <summary>
        /// Converts a DateTime object to unix epoch seconds.
        /// </summary>
        /// <param name="date">The Date you want to convert.</param>
        /// <returns>The amount of seconds since 1/1/1970 0:00:00 AM</returns>
        public static long GetSecondsSinceUnixEpoch(DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date.ToUniversalTime() - epoch).TotalSeconds);
        }

        /// <summary>
        /// Converts an ISO 8601 time string to a DateTime object.
        /// </summary>
        /// <param name="isoDate">The ISO 8601 string.</param>
        /// <returns>The DateTime object</returns>
        public static DateTime ConvertIsoTimeToDateTime(string isoDate)
        {
            try
            {
                DateTime time = DateTime.Parse(isoDate);
                return time;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error converting time: {0}", ex.Message);
            }

            return new DateTime();
        }
    }
}
