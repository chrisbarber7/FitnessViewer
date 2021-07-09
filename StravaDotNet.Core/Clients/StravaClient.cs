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
using System.Reflection;
using Strava.Authentication;

namespace Strava.Clients
{
    /// <summary>
    /// The StravaClient is used to receive data from Strava. The client offers various subclients, which you can use to
    /// receive the data. 
    /// <list type="bullet">
    /// <listheader>
    ///    <term>Currently the following Strava API resources are supported:</term>
    /// </listheader>
    /// <item>
    ///     <term>Activities</term>
    /// </item>
    /// <item>
    ///     <term>Athletes</term>
    /// </item>
    /// <item>
    ///     <term>Clubs</term>
    /// </item>
    /// <item>
    ///     <term>Gear</term>
    /// </item>
    /// <item>
    ///     <term>Segments</term>
    /// </item>
    /// <item>
    ///     <term>Segment Efforts</term>
    /// </item>
    /// <item>
    ///     <term>Stats</term>
    /// </item>
    /// <item>
    ///     <term>Streams</term>
    /// </item>
    /// </list>
    /// </summary>
    public class StravaClient
    {
        private readonly IAuthentication _authenticator;

        /// <summary>
        /// Initializes a new instance of the StravaClient class.
        /// </summary>
        /// <param name="authenticator">The IAuthentication object that holds a valid Access Token.</param>
        /// <seealso cref="WebAuthentication"/>
        /// <seealso cref="StaticAuthentication"/>
        public StravaClient(IAuthentication authenticator)
        {
            if (authenticator != null)
            {
                _authenticator = authenticator;

                Activities = new ActivityClient(authenticator);
                Athletes = new AthleteClient(authenticator);
                Clubs = new ClubClient(authenticator);
                Gear = new GearClient(authenticator);
                Segments = new SegmentClient(authenticator);
                Streams = new StreamClient(authenticator);
                Uploads = new UploadClient(authenticator);
                Efforts = new EffortClient(authenticator);
                Stats = new StatsClient(authenticator);
                Routes = new RouteClient(authenticator);
            }
            else
            {
                throw new ArgumentException("The IAuthentication object must not be null.");
            }
        }

        #region Clients

        /// <summary>
        /// Predefined ActivityClient.
        /// </summary>
        public ActivityClient Activities { get; set; }

        /// <summary>
        /// Predefined AthleteClient.
        /// </summary>
        public AthleteClient Athletes { get; set; }

        /// <summary>
        /// Predefined ClubClient.
        /// </summary>
        public ClubClient Clubs { get; set; }

        /// <summary>
        /// Predefined GearClient.
        /// </summary>
        public GearClient Gear { get; set; }

        /// <summary>
        /// Predefined SegmentClient.
        /// </summary>
        public SegmentClient Segments { get; set; }

        /// <summary>
        /// Predefined StreamClient.
        /// </summary>
        public StreamClient Streams { get; set; }

        /// <summary>
        /// Predefined StatsClient.
        /// </summary>
        public StatsClient Stats { get; set; }

        /// <summary>
        /// The UploadClient is used to upload new activities to Strava.
        /// </summary>
        public UploadClient Uploads { get; set; }

        /// <summary>
        /// The EffortClient is used to receive efforts on a segment.
        /// </summary>
        public EffortClient Efforts { get; set; }

        /// <summary>
        /// Predefined RouteClient.
        /// </summary>
        public RouteClient Routes { get; set; }

        #endregion

        /// <summary>
        /// Returns the framework version of the StravaClient.
        /// </summary>
        /// <returns>The version number of the StravaClient.</returns>
        public override string ToString()
        {
            return string.Format("StravaClient Version {0}", Assembly.GetExecutingAssembly().GetName().Version);
        }
    }
}
