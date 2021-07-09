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

namespace Strava.Athletes
{
    /// <summary>
    /// Use this enum to specifiy which parameter of an athlete you want to update.
    /// </summary>
    public enum AthleteParameter
    {
        /// <summary>
        /// Update the city.
        /// </summary>
        City,
        /// <summary>
        /// Update the state.
        /// </summary>
        State,
        /// <summary>
        /// Update the country.
        /// </summary>
        Country,
        /// <summary>
        /// Update the athlete's wheight.
        /// </summary>
        Weight
    }
}