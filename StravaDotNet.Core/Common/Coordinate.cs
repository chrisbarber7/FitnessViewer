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

namespace Strava.Common
{
    /// <summary>
    /// This class represents a lat/lng coordinate.
    /// </summary>
    public class Coordinate
    {
        /// <summary>
        /// The latitude.
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// The longitude.
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// Initializes a new instance of the Coordinate class.
        /// </summary>
        /// <param name="lat">The latitude.</param>
        /// <param name="lng">The longitude.</param>
        public Coordinate(double lat, double lng)
        {
            Latitude = lat;
            Longitude = lng;
        }

        /// <summary>
        /// Returns a string of the coordinate.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("({0}, {1})", Latitude, Longitude);
        }
    }
}
