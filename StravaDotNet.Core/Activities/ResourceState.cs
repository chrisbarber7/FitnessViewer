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
    /// Indicates the levels of detail of a object returned by Strava.
    /// </summary>
    public enum ResourceState
    {
        /// <summary>
        /// Only meta information are contained.
        /// </summary>
        Meta,
        /// <summary>
        /// Summary information are contained.
        /// </summary>
        Summary,
        /// <summary>
        /// All information are contained.
        /// </summary>
        Detailed
    }
}