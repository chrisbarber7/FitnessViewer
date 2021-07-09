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

namespace Strava.Common
{
    /// <summary>
    /// COnverts a Json string to an object.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Unmarshaller<T>
    {
        /// <summary>
        /// Converts a Json string to an object.
        /// </summary>
        /// <param name="json">The json string.</param>
        /// <returns>The converted object of type T.</returns>
        public static T Unmarshal(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                throw new ArgumentException("The json string is null or empty.");
            }

            T deserializedObject = JsonConvert.DeserializeObject<T>(json);
            return deserializedObject;
        }
    }
}
