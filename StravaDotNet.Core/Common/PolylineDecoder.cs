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
using System.Collections.Generic;
using System.Diagnostics;

namespace Strava.Common
{
    /// <summary>
    /// Decode a polyline to a list of coordinates.
    /// </summary>
    public static class PolylineDecoder
    {
        /// <summary>
        /// Decodes a polyline to a list of coordinates.
        /// </summary>
        /// <param name="encodedPoints">The encoded polyline.</param>
        /// <returns>A list of coordinates.</returns>
        public static List<Coordinate> Decode(string encodedPoints)
        {
            {
                if (string.IsNullOrEmpty(encodedPoints))
                    return null;

                List<Coordinate> poly = new List<Coordinate>();
                char[] polylinechars = encodedPoints.ToCharArray();
                int index = 0;

                int currentLat = 0;
                int currentLng = 0;
                int next5bits;
                int sum;
                int shifter;

                try
                {
                    while (index < polylinechars.Length)
                    {
                        // calculate next latitude
                        sum = 0;
                        shifter = 0;
                        do
                        {
                            next5bits = (int) polylinechars[index++] - 63;
                            sum |= (next5bits & 31) << shifter;
                            shifter += 5;
                        } while (next5bits >= 32 && index < polylinechars.Length);

                        if (index >= polylinechars.Length)
                            break;

                        currentLat += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                        //calculate next longitude
                        sum = 0;
                        shifter = 0;
                        do
                        {
                            next5bits = (int) polylinechars[index++] - 63;
                            sum |= (next5bits & 31) << shifter;
                            shifter += 5;
                        } while (next5bits >= 32 && index < polylinechars.Length);

                        if (index >= polylinechars.Length && next5bits >= 32)
                            break;

                        currentLng += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                        poly.Add(new Coordinate(Convert.ToDouble(currentLat) / 100000.0, Convert.ToDouble(currentLng) / 100000.0));
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(string.Format("Error: Decoding polyline: {0}", ex.Message));
                }

                return poly;
            }
        }
    }
}
