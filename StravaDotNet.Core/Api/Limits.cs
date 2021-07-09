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

namespace Strava.Api
{
    /// <summary>
    /// This class contains information about the Strava API limits and how much requests are consumed by your application.
    /// </summary>
    public static class Limits
    {
        /// <summary>
        /// UsageChanged is raised whenever a web request is sent to the Strava servers and a response is received.
        /// </summary>
        public static event EventHandler<UsageChangedEventArgs> UsageChanged;

        private static Usage _usage;
        private static Limit _limit;
        
        /// <summary>
        /// The short- and long-term usage of your application.
        /// </summary>
        public static Usage Usage
        {
            get
            {
                if (_usage == null)
                {
                    _usage = new Usage(0, 0);
                }

                return _usage;
            }
            set
            {
                _usage = value;

                if (UsageChanged != null)
                {
                    UsageChanged(null, new UsageChangedEventArgs(value.ShortTerm, value.LongTerm));
                }
            }
        }

        /// <summary>
        /// The short- and long-term limit of your application.
        /// </summary>
        public static Limit Limit
        {
            get
            {
                if (_limit == null)
                {
                    _limit = new Limit(0, 0);
                }

                return _limit;
            }
            set
            {
                _limit = value;
            }
        }
    }
}
