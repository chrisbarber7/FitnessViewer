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

namespace Strava.Api
{
    /// <summary>
    /// This class holds information about the Strava API usage of your application.
    /// </summary>
    public class Usage
    {
        /// <summary>
        /// The usage of the short term limit. Default short term limit is 600 requests per 15 minutes.
        /// </summary>
        public int ShortTerm { get; set; }

        /// <summary>
        /// The usage of the long term limit. Default long term limit is 30.000 requests a day.
        /// </summary>
        public int LongTerm { get; set; }

        /// <summary>
        /// Initializes a new instances of the Usage class.
        /// </summary>
        /// <param name="shortTerm">The short term usage.</param>
        /// <param name="longTerm">The long term usage.</param>
        public Usage(int shortTerm, int longTerm)
        {
            ShortTerm = shortTerm;
            LongTerm = longTerm;
        }
    }
}
