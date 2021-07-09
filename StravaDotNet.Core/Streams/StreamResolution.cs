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

namespace Strava.Streams
{
    /// <summary>
    /// Indicates desired number of data points, streams will only be down sampled.
    /// </summary>
    public enum StreamResolution
    {
        /// <summary>
        /// Returns 100 data points (maximum).
        /// </summary>
        Low,
        /// <summary>
        /// Returs 1000 data points (maximum).
        /// </summary>
        Medium,
        /// <summary>
        /// Returns 10000 data points (maximum).
        /// </summary>
        High,
        /// <summary>
        /// Returns all the data points.
        /// </summary>
        All
    }
}