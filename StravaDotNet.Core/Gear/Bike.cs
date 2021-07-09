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

namespace Strava.Gear
{
    /// <summary>
    /// This class represents gear.
    /// </summary>
    public class Bike : GearSummary
    {
        /// <summary>
        /// The gear's brand name.
        /// </summary>
        [JsonProperty("brand_name")]
        public string Brand { get; set; }

        /// <summary>
        /// The gear's model.
        /// </summary>
        [JsonProperty("model_name")]
        public string Model { get; set; }

        /// <summary>
        /// The type of bike.
        /// </summary>
        [JsonProperty("frame_type")]
        private string _frameType = string.Empty;

        /// <summary>
        /// The type of bike.
        /// </summary>
        public BikeType FrameType
        {
            get
            {
                if (_frameType.Equals("1"))
                {
                    return BikeType.Mountain;
                }
                if (_frameType.Equals("2"))
                {
                    return BikeType.Cross;
                }
                if (_frameType.Equals("3"))
                {
                    return BikeType.Road;
                }
                if (_frameType.Equals("4"))
                {
                    return BikeType.Timetrial;
                }

                return BikeType.Road;
            }
        }

        /// <summary>
        /// The gear's description.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
