#region Copyright (C) 2015 Sascha Simon

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

namespace Strava.Routes
{
    /// <summary>
    /// The type of a route.
    /// </summary>
    public enum SubType
    {
        /// <summary>
        /// The route is a road bike route.
        /// </summary>
        Road = 1,
        /// <summary>
        /// The route is a MTB route.
        /// </summary>
        MTB = 2,
        /// <summary>
        /// The route is a CX route.
        /// </summary>
        CX = 3,
        /// <summary>
        /// The route is a trail route.
        /// </summary>
        Trail = 4,
        /// <summary>
        /// The route is a mixed route.
        /// </summary>
        Mixed = 5
    }
}
