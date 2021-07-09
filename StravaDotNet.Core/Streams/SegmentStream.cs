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
using Newtonsoft.Json;

namespace Strava.Streams
{
    /// <summary>
    /// Streams is the Strava term for the raw data associated with an activity. All streams for a given 
    /// activity or segment effort will be the same length and the values at a given index 
    /// correspond to the same time.
    /// </summary>
    public class SegmentStream
    {
        /// <summary>
        /// The type of stream.
        /// </summary>
        [JsonProperty("type")]
        private string Type { get; set; }

        /// <summary>
        /// The type of stream.
        /// </summary>
        public StreamType StreamType
        {
            get
            {
                if (Type.Equals("altitude"))
                {
                    return StreamType.Altitude;
                }
                if (Type.Equals("cadence"))
                {
                    return StreamType.Cadence;
                }
                if (Type.Equals("latlng"))
                {
                    return StreamType.LatLng;
                }
                if (Type.Equals("distance"))
                {
                    return StreamType.Distance;
                }
                if (Type.Equals("grade_smooth"))
                {
                    return StreamType.Grade_Smooth;
                }
                if (Type.Equals("heartrate"))
                {
                    return StreamType.Heartrate;
                }
                if (Type.Equals("moving"))
                {
                    return StreamType.Moving;
                }
                if (Type.Equals("temp"))
                {
                    return StreamType.Temp;
                }
                if (Type.Equals("time"))
                {
                    return StreamType.Time;
                }
                if (Type.Equals("velocity_smooth"))
                {
                    return StreamType.Velocity_Smooth;
                }

                return StreamType.Watts;
            }
        }

        /// <summary>
        /// Array of stream values.
        /// </summary>
        [JsonProperty("data")]
        public List<object> Data { get; set; }

        /// <summary>
        /// Series type used for down sampling, will be present even if not used.
        /// </summary>
        [JsonProperty("series_type")]
        public string SeriesType { get; set; }

        /// <summary>
        /// Complete stream length
        /// </summary>
        [JsonProperty("original_size")]
        public int OriginalSize { get; set; }

        /// <summary>
        /// Resolution of the stream.
        /// 'low', 'medium' or 'high'
        /// </summary>
        [JsonProperty("resolution")]
        public string Resolution { get; set; }
    }
}
