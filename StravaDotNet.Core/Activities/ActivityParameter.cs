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
    /// Specifies the parameter that is being updated.
    /// </summary>
    public enum ActivityParameter
    {
        /// <summary>
        /// Updates the name of an activity. string values are allowed as parameter.
        /// </summary>
        Name,
        /// <summary>
        /// Set to true if the activity is a private one.
        /// </summary>
        Private,
        /// <summary>
        /// Indicates if the activity is a commute.
        /// </summary> 
        Commute,
        /// <summary>
        /// Indicates whether the activity was recorded on a stationary trainer.
        /// </summary>
        Trainer,
        /// <summary>
        /// Updates the gear used.
        /// </summary>
        GearId,
        /// <summary>
        /// Adds a description to an activity.
        /// </summary>
        Description
    }
}