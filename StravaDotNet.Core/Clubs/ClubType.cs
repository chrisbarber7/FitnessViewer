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

namespace Strava.Clubs
{
    /// <summary>
    /// This enum is used by the Club class and represents the type of a club.
    /// </summary>
    public enum ClubType
    {
        /// <summary>
        /// The club is a casual club.
        /// </summary>
        Casual,
        /// <summary>
        /// The club is a racing team.
        /// </summary>
        RacingTeam,
        /// <summary>
        /// The club is owned by a shop.
        /// </summary>
        Shop,
        /// <summary>
        /// The club's members are all riding for a company.
        /// </summary>
        Company,
        /// <summary>
        /// Other club.
        /// </summary>
        Other
    }
}