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
using System.Diagnostics;

namespace Strava.Authentication
{
    /// <summary>
    /// This class is used to start a local web server to receive a callback message from Strava. This message 
    /// contains a auth token. This token is then used to obtain an access token.
    /// Using this class requires admin privileges.
    /// </summary>
    public class WebAuthentication : IAuthentication
    {
        /// <summary>
        /// AccessTokenReceived is raised when an access token is received from the Strava server.
        /// </summary>
        public event EventHandler<TokenReceivedEventArgs> AccessTokenReceived;

        /// <summary>
        /// AuthCodeReceived is raised when an auth token is received from the Strava server.
        /// </summary>
        public event EventHandler<AuthCodeReceivedEventArgs> AuthCodeReceived;

        /// <summary>
        /// The access token that was received from the Strava server.
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// the auth token that was received from the Strava server.
        /// </summary>
        public string AuthCode { get; set; }

        /// <summary>
        /// Loads an access token asynchronously from the Strava servers. Invoking this method opens a web browser.
        /// </summary>
        /// <param name="clientId">The client id from your application (provided by Strava).</param>
        /// <param name="clientSecret">The client secret (provided by Strava).</param>
        /// <param name="scope">Define what your application is allowed to do.</param>
        /// <param name="callbackPort">Define the callback port (optional, default value is 1895). Only change this, 
        /// if the default port 1895 is already used on your machine.</param>
        public void GetTokenAsync(string clientId, string clientSecret, Scope scope, int callbackPort = 1895)
        {
            LocalWebServer server = new LocalWebServer(string.Format("http://*:{0}/", callbackPort));
            server.ClientId = clientId;
            server.ClientSecret = clientSecret;

            server.AccessTokenReceived += delegate(object sender, TokenReceivedEventArgs args)
            {
                if (AccessTokenReceived != null)
                {
                    AccessTokenReceived(this, args);
                    AccessToken = args.Token;
                }
            };

            server.AuthCodeReceived += delegate(object sender, AuthCodeReceivedEventArgs args)
            {
                if (AuthCodeReceived != null)
                {
                    AuthCodeReceived(this, args);
                    AuthCode = args.AuthCode;
                }
            };

            server.Start();

            string url = "https://www.strava.com/oauth/authorize";
            string scopeLevel = string.Empty;

            switch (scope)
            {
                case Scope.Full:
                    scopeLevel = "view_private,write";
                    break;
                case Scope.Public:
                    scopeLevel = "public";
                    break;
                case Scope.ViewPrivate:
                    scopeLevel = "view_private";
                    break;
                case Scope.Write:
                    scopeLevel = "write";
                    break;
            }

            Process process = new Process();
            process.StartInfo = new ProcessStartInfo(string.Format("{0}?client_id={1}&response_type=code&redirect_uri=http://localhost:{2}&scope={3}&approval_prompt=auto", url, clientId, callbackPort, scopeLevel));
            process.Start();
        }
    }
}