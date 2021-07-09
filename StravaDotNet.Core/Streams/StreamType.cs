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

namespace Strava.Streams
{
    /// <summary>
    /// Used to specify which stream should be received from the server.
    /// </summary>
    [Flags]
    public enum StreamType
    {
        /// <summary>
        /// Time information is received.
        /// </summary>
        Time = 1,
        /// <summary>
        /// Coordinate information is received.
        /// </summary>
        LatLng = 2,
        /// <summary>
        /// Distance information is received.
        /// </summary>
        Distance = 4,
        /// <summary>
        /// Altitude information is received.
        /// </summary>
        Altitude = 8,
        /// <summary>
        /// Velocity information is received.
        /// </summary>
        Velocity_Smooth = 16,
        /// <summary>
        /// Heartrate information is received.
        /// </summary>
        Heartrate = 32,
        /// <summary>
        /// Cadence information is received.
        /// </summary>
        Cadence = 64,
        /// <summary>
        /// Power information is received.
        /// </summary>
        Watts = 128,
        /// <summary>
        /// Temperature information is received.
        /// </summary>
        Temp = 256,
        /// <summary>
        /// Information about the time moved is received.
        /// </summary>
        Moving = 512,
        /// <summary>
        /// Information about the grade is received.
        /// </summary>
        Grade_Smooth = 1024
    }
}