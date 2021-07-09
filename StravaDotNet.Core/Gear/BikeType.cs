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

namespace Strava.Gear
{
    /// <summary>
    /// This enum is used to return a more expressive bike type instead of just a number.
    /// </summary>
    public enum BikeType
    {
        /// <summary>
        /// The bike is a mountain bike.
        /// </summary>
        Mountain,
        /// <summary>
        /// The bike is a cross bike.
        /// </summary>
        Cross,
        /// <summary>
        /// The bike is a road bike.
        /// </summary>
        Road,
        /// <summary>
        /// The bike is a time trial bike.
        /// </summary>
        Timetrial
    }
}