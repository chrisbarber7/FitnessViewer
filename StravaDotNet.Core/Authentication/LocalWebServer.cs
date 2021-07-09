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
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using Strava.Common;

namespace Strava.Authentication
{
    /// <summary>
    /// This class starts a local web server. This web server is needed to receive the callback from
    /// Strava.
    /// </summary>
    public class LocalWebServer
    {
        /// <summary>
        /// AuthCodeReceived is raised whenever an auth code is received from the Strava servers.
        /// </summary>
        public event EventHandler<AuthCodeReceivedEventArgs> AuthCodeReceived;

        /// <summary>
        /// AccessTokenReceived is raised whenever an access token is received from the Strava servers.
        /// </summary>
        public event EventHandler<TokenReceivedEventArgs> AccessTokenReceived;

        /// <summary>
        /// The Client Id provided by Strava upon registering your application.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// The Client secret provided by Strava upon registering your application.
        /// </summary>
        public string ClientSecret { get; set; }
        
        private HttpListener _httpListener = new HttpListener();
        private HttpListenerContext _context;

        /// <summary>
        /// Initializes a new instance of the LocalWebServer class.
        /// </summary>
        /// <param name="prefix">The server prefix.</param>
        public LocalWebServer(string prefix)
        {
            _httpListener = new HttpListener();
            _httpListener.Prefixes.Add(prefix);
        }

        /// <summary>
        /// Starts the local web server.
        /// </summary>
        public void Start()
        {
            _httpListener.Start();

            new Thread(ProcessRequest).Start();
        }

        /// <summary>
        /// Stops the local web server.
        /// </summary>
        public void Stop()
        {
            _httpListener.Stop();
        }

        /// <summary>
        /// Processes a request.
        /// </summary>
        public async void ProcessRequest()
        {
            _context = _httpListener.GetContext();
            NameValueCollection queries = _context.Request.QueryString;

            // Access Token laden
            // 0 = state
            // 1 = code
            string code = queries.GetValues(1)[0];

            if (!string.IsNullOrEmpty(code))
            {
                if (AuthCodeReceived != null)
                {
                    AuthCodeReceived(this, new AuthCodeReceivedEventArgs(code));
                }
            }

            // Save token to hard disk
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "StravaApi");
            string file = Path.Combine(path, "AccessToken.auth");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            FileStream stream = new FileStream(file, FileMode.OpenOrCreate);
            stream.Write(Encoding.UTF8.GetBytes(code), 0, Encoding.UTF8.GetBytes(code).Length);
            stream.Close();

            // Antwort liefen und anzeigen
            byte[] b = Encoding.UTF8.GetBytes("Access token was loaded - You can close your browser window.");
            _context.Response.ContentLength64 = b.Length;
            _context.Response.OutputStream.Write(b, 0, b.Length);
            _context.Response.OutputStream.Close();


            // Getting the Access Token
            string url = string.Format("https://www.strava.com/oauth/token?client_id={0}&client_secret={1}&code={2}", ClientId, ClientSecret, code);
            string json = await Http.WebRequest.SendPostAsync(new Uri(url));

            AccessToken auth = Unmarshaller<AccessToken>.Unmarshal(json);

            if (!string.IsNullOrEmpty(auth.Token))
            {
                if (AccessTokenReceived != null)
                {
                    AccessTokenReceived(this, new TokenReceivedEventArgs(auth.Token));
                }
            }
        }
    }
}
