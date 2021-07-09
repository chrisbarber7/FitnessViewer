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
using Strava.Authentication;

namespace Strava.Clients
{
    /// <summary>
    /// Base class for all the clients except the StravaClient.
    /// </summary>
    public class BaseClient
    {
        /// <summary>
        /// IAuthentication object used for authentication.
        /// </summary>
        protected IAuthentication Authentication;

        /// <summary>
        /// Initializes a new instance of the BaseClient class.
        /// </summary>
        /// <param name="auth">A valid object that implements the IAuthentication interface.</param>
        public BaseClient(IAuthentication auth)
        {
            if (auth == null)
            {
                throw new ArgumentException("The IAuthentication object must not be null!");
            }

            Authentication = auth;

            System.Net.ServicePointManager.SecurityProtocol |= System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;
        }
    }
}
