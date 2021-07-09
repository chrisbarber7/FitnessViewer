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
using Newtonsoft.Json;

namespace Strava.Upload
{
    /// <summary>
    /// Contains information about the status of your upload.
    /// </summary>
    public class UploadStatus
    {
        /// <summary>
        /// The upload id of the activity. Provided by Strava upon upload.
        /// </summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>
        /// The external id. Can not be changed.
        /// </summary>
        [JsonProperty("external_id")]
        public string ExternalId { get; set; }

        /// <summary>
        /// Error message.
        /// </summary>
        [JsonProperty("error")]
        public string Error { get; set; }

        /// <summary>
        /// Status message.
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }

        /// <summary>
        /// The Strava activity id.
        /// </summary>
        [JsonProperty("activity_id")]
        public string ActivityId { get; set; }

        /// <summary>
        /// Returns the status string as a more user-friendly enum.
        /// </summary>
        public CurrentUploadStatus CurrentStatus
        {
            get
            {
                switch (Status)
                {
                    case "Your activity is still being processed.":
                        return CurrentUploadStatus.Processing;
                    case "The created activity has been deleted.":
                        return CurrentUploadStatus.Deleted;
                    case "There was an error processing your activity.":
                        return CurrentUploadStatus.Error;
                }

                return CurrentUploadStatus.Ready;
            }
        }
    }
}