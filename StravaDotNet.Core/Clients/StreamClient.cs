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
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Strava.Api;
using Strava.Authentication;
using Strava.Common;
using Strava.Http;
using Strava.Streams;

namespace Strava.Clients
{
    /// <summary>
    /// Streams is the Strava term for the raw data associated with an activity. 
    /// All streams for a given activity or segment effort will be the same length and the values at 
    /// a given index correspond to the same time.
    /// </summary>
    public class StreamClient : BaseClient
    {
        /// <summary>
        /// Initializes a new instance of the StreamClient class.
        /// </summary>
        /// <param name="auth">A IAuthenticator object that contains a valid Strava access token.</param>
        public StreamClient(IAuthentication auth) : base(auth) { }

        #region Async

        /// <summary>
        /// Gets an activity stream asynchronously.
        /// </summary>
        /// <param name="activityId">The Strava activity id.</param>
        /// <param name="typeFlags">Specifies the type of stream.</param>
        /// <param name="resolution">Specifies the resolution of the stream.</param>
        /// <returns>The stream data.</returns>
        public async Task<List<ActivityStream>> GetActivityStreamAsync(string activityId, StreamType typeFlags, StreamResolution resolution = StreamResolution.All)
        {
            StringBuilder types = new StringBuilder();

            foreach (StreamType type in (StreamType[])Enum.GetValues(typeof(StreamType)))
            {
                if (typeFlags.HasFlag(type))
                {
                    types.Append(type.ToString().ToLower());
                    types.Append(",");
                }
            }

            types.Remove(types.ToString().Length - 1, 1);

            string getUrl = string.Format("{0}/{1}/streams/{2}?{3}&access_token={4}",
                Endpoints.Activity,
                activityId,
                types,
                resolution != StreamResolution.All ? "resolution=" + resolution.ToString().ToLower() : "",
                Authentication.AccessToken
                );

            string json = await WebRequest.SendGetAsync(new Uri(getUrl));

            return Unmarshaller<List<ActivityStream>>.Unmarshal(json);
        }

        /// <summary>
        /// Gets a segment stream asynchronously.
        /// </summary>
        /// <param name="segmentId">The Strava segment id.</param>
        /// <param name="typeFlags">Specifies the type of stream.</param>
        /// <param name="resolution">Specifies the resolution of the stream.</param>
        /// <returns>The stream data.</returns>
        public async Task<List<SegmentStream>> GetSegmentStreamAsync(string segmentId, SegmentStreamType typeFlags, StreamResolution resolution = StreamResolution.All)
        {
            // Only distance, altitude and latlng stream types are available.

            StringBuilder types = new StringBuilder();

            foreach (SegmentStreamType type in (StreamType[])Enum.GetValues(typeof(SegmentStreamType)))
            {
                if (typeFlags.HasFlag(type))
                {
                    types.Append(type.ToString().ToLower());
                    types.Append(",");
                }
            }

            types.Remove(types.ToString().Length - 1, 1);

            string getUrl = string.Format("{0}/{1}/streams/{2}?{3}&access_token={4}",
                Endpoints.Leaderboard,
                segmentId,
                types,
                resolution != StreamResolution.All ? "resolution=" + resolution.ToString().ToLower() : "",
                Authentication.AccessToken
                );

            string json = await WebRequest.SendGetAsync(new Uri(getUrl));

            return Unmarshaller<List<SegmentStream>>.Unmarshal(json);
        }

        /// <summary>
        /// Gets a segment effort stream asynchronously.
        /// </summary>
        /// <param name="effortId">The Strava segment effort id.</param>
        /// <param name="typeFlags">Specifies the type of stream.</param>
        /// <param name="resolution">Specifies the resolution of the stream.</param>
        /// <returns>The stream data.</returns>
        public async Task<List<SegmentEffortStream>> GetSegmentEffortStreamAsync(string effortId, SegmentStreamType typeFlags, StreamResolution resolution = StreamResolution.All)
        {
            StringBuilder types = new StringBuilder();

            foreach (SegmentStreamType type in (StreamType[])Enum.GetValues(typeof(SegmentStreamType)))
            {
                if (typeFlags.HasFlag(type))
                {
                    types.Append(type.ToString().ToLower());
                    types.Append(",");
                }
            }

            types.Remove(types.ToString().Length - 1, 1);

            string getUrl = string.Format("https://www.strava.com/api/v3/segment_efforts/{0}/streams/{1}?{2}&access_token={3}",
                effortId,
                types,
                resolution != StreamResolution.All ? "resolution=" + resolution.ToString().ToLower() : "",
                Authentication.AccessToken
                );

            string json = await WebRequest.SendGetAsync(new Uri(getUrl));

            return Unmarshaller<List<SegmentEffortStream>>.Unmarshal(json);
        }


        #endregion
        
        #region Sync

        /// <summary>
        /// Gets an activity stream.
        /// </summary>
        /// <param name="activityId">The Strava activity id.</param>
        /// <param name="typeFlags">Specifies the type of stream.</param>
        /// <param name="resolution">Specifies the resolution of the stream.</param>
        /// <returns>The stream data.</returns>
        public List<ActivityStream> GetActivityStream(string activityId, StreamType typeFlags, StreamResolution resolution = StreamResolution.All)
        {
            StringBuilder types = new StringBuilder();

            foreach (StreamType type in (StreamType[])Enum.GetValues(typeof(StreamType)))
            {
                if (typeFlags.HasFlag(type))
                {
                    types.Append(type.ToString().ToLower());
                    types.Append(",");
                }
            }

            types.Remove(types.ToString().Length - 1, 1);

            string getUrl = string.Format("{0}/{1}/streams/{2}?{3}&access_token={4}",
                Endpoints.Activity,
                activityId,
                types,
                resolution != StreamResolution.All ? "resolution=" + resolution.ToString().ToLower() : "",
                Authentication.AccessToken
                );

            string json = WebRequest.SendGet(new Uri(getUrl));
  //          Debug.WriteLine(getUrl);
            return Unmarshaller<List<ActivityStream>>.Unmarshal(json);
        }

        /// <summary>
        /// Gets a segment effort stream.
        /// </summary>
        /// <param name="effortId">The Strava segment effort id.</param>
        /// <param name="typeFlags">Specifies the type of stream.</param>
        /// <param name="resolution">Specifies the resolution of the stream.</param>
        /// <returns>The stream data.</returns>
        public List<SegmentEffortStream> GetSegmentEffortStream(string effortId, SegmentStreamType typeFlags, StreamResolution resolution = StreamResolution.All)
        {
            StringBuilder types = new StringBuilder();

            foreach (SegmentStreamType type in (StreamType[])Enum.GetValues(typeof(SegmentStreamType)))
            {
                if (typeFlags.HasFlag(type))
                {
                    types.Append(type.ToString().ToLower());
                    types.Append(",");
                }
            }

            types.Remove(types.ToString().Length - 1, 1);

            string getUrl = string.Format("https://www.strava.com/api/v3/segment_efforts/{0}/streams/{1}?{2}&access_token={3}",
                effortId,
                types,
                resolution != StreamResolution.All ? "resolution=" + resolution.ToString().ToLower() : "",
                Authentication.AccessToken
                );

            string json = WebRequest.SendGet(new Uri(getUrl));

            return Unmarshaller<List<SegmentEffortStream>>.Unmarshal(json);
        }

        /// <summary>
        /// Gets a segment stream.
        /// </summary>
        /// <param name="segmentId">The Strava segment id.</param>
        /// <param name="typeFlags">Specifies the type of stream.</param>
        /// <param name="resolution">Specifies the resolution of the stream.</param>
        /// <returns>The stream data.</returns>
        public List<SegmentStream> GetSegmentStream(string segmentId, SegmentStreamType typeFlags, StreamResolution resolution = StreamResolution.All)
        {
            // Only distance, altitude and latlng stream types are available.

            StringBuilder types = new StringBuilder();

            foreach (SegmentStreamType type in (StreamType[])Enum.GetValues(typeof(SegmentStreamType)))
            {
                if (typeFlags.HasFlag(type))
                {
                    types.Append(type.ToString().ToLower());
                    types.Append(",");
                }
            }

            types.Remove(types.ToString().Length - 1, 1);

            string getUrl = string.Format("{0}/{1}/streams/{2}?{3}&access_token={4}",
                Endpoints.Leaderboard,
                segmentId,
                types,
                resolution != StreamResolution.All ? "resolution=" + resolution.ToString().ToLower() : "",
                Authentication.AccessToken
                );

            string json = WebRequest.SendGet(new Uri(getUrl));

            return Unmarshaller<List<SegmentStream>>.Unmarshal(json);
        }

        #endregion

    }
}
