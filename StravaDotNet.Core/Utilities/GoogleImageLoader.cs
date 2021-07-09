//#region Copyright (C) 2014 Sascha Simon

////  This program is free software: you can redistribute it and/or modify
////  it under the terms of the GNU General Public License as published by
////  the Free Software Foundation, either version 3 of the License, or
////  (at your option) any later version.
////
////  This program is distributed in the hope that it will be useful,
////  but WITHOUT ANY WARRANTY; without even the implied warranty of
////  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
////  GNU General Public License for more details.
////
////  You should have received a copy of the GNU General Public License
////  along with this program.  If not, see http://www.gnu.org/licenses/.
////
////  Visit the official homepage at http://www.sascha-simon.com

//#endregion

//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Linq;
//using System.Threading.Tasks;
//using Strava.Common;
//using Strava.Http;

//namespace Strava.Utilities
//{
//    /// <summary>
//    /// This class sends your polyline to the Google Maps server which then return a map image.
//    /// You have to add a Google license agreement to your application when using this method.
//    /// </summary>
//    public class GoogleImageLoader
//    {
//        /// <summary>
//        /// Loads an image of a map.
//        /// </summary>
//        /// <param name="polyline">The polyline of your activity.</param>
//        /// <param name="dimension">The dimension of the picture which will be returned.</param>
//        /// <param name="mapType">Choose the map type of the image.</param>
//        /// <returns>An image of your activity on the specified map.</returns>
//        public static async Task<Image> LoadActivityPreviewAsync(string polyline, Dimension dimension, MapType mapType)
//        {
//            if (dimension.Width == 0 || dimension.Height == 0)
//            {
//                throw new ArgumentException("Both width and height must be greater than zero.");
//            }

//            List<Coordinate> c = PolylineDecoder.Decode(polyline);
//            string markerStart = string.Format("&markers=icon:http://tinyurl.com/np8ozqm%7C{0},{1}", c.First().Latitude, c.First().Longitude);
//            string markerEnd = string.Format("&markers=icon:http://tinyurl.com/mzj8mvq%7C{0},{1}", c.Last().Latitude, c.Last().Longitude);

//            string url = string.Format("http://maps.googleapis.com/maps/api/staticmap?sensor=false&maptype={0}&size={1}x{2}&{3}&{4}&path=weight:3|color:red|enc:{5}",
//                mapType.ToString().ToLower(),
//                dimension.Width,
//                dimension.Height,
//                markerStart,
//                markerEnd,
//                polyline);

//            Image image = await ImageLoader.LoadImage(new Uri(url));
//            return image;
//        }
//    }
//}
