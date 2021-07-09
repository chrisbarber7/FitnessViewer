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
    /// This enum represents the category of a segment.
    /// </summary>
    public enum ClimbCategory
    {
        /// <summary>
        /// The segment is a HC climb.
        /// </summary>
        CategoryHc,
        /// <summary>
        /// The segment is a Cat 4 climb.
        /// </summary>
        Category4,
        /// <summary>
        /// The segment is a Cat 3 climb.
        /// </summary>
        Category3,
        /// <summary>
        /// The segment is a Cat 2 climb.
        /// </summary>
        Category2,
        /// <summary>
        /// The segment is a Cat 1 climb.
        /// </summary>
        Category1,
        /// <summary>
        /// The segment is not categorized (usually a very flat segment).
        /// </summary>
        CategoryNc
    }
}
