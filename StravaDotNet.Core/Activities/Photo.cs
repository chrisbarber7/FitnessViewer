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

namespace Strava.Activities
{
    /// <summary>
    /// Represents a photo linked to an activity.
    /// </summary>
    public class Photo
    {
        /// <summary>
        /// The photo's id.
        /// </summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>
        /// The id of the activity to which the photo is connected to.
        /// </summary>
        [JsonProperty("activity_id")]
        public long ActivityId { get; set; }

        /// <summary>
        /// The level of detail.
        /// </summary>
        [JsonProperty("resource_state")]
        public int ResourceState { get; set; }

        /// <summary>
        /// Url to the picture. Use the ImageLoader class to download the picture.
        /// </summary>
        [JsonProperty("ref")]
        public string ImageUrl { get; set; }

        /// <summary>
        /// The photo's external id.
        /// </summary>
        [JsonProperty("uid")]
        public string ExternalUid { get; set; }

        /// <summary>
        /// The caption.
        /// </summary>
        [JsonProperty("caption")]
        public string Caption { get; set; }

        /// <summary>
        /// Image source. Currently only "InstagramPhoto"
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// The date when the image was uploaded.
        /// </summary>
        [JsonProperty("uploaded_at")]
        public string UploadedAt { get; set; }

        /// <summary>
        /// The date when the image was crated.
        /// </summary>
        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        /// <summary>
        /// The location where the picture was taken.
        /// </summary>
        [JsonProperty("location")]
        public List<double> Location { get; set; }
    }
}
