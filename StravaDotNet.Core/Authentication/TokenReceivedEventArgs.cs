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

namespace Strava.Authentication
{
    /// <summary>
    /// Thius class holds information about a access token that was received from the server.
    /// </summary>
    public class TokenReceivedEventArgs
    {
        /// <summary>
        /// The received access token.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Initializes a new instance of the TokenReceivedEventArgs class.
        /// </summary>
        /// <param name="token">The token received from the server.</param>
        public TokenReceivedEventArgs(string token)
        {
            Token = token;
        }
    }
}
