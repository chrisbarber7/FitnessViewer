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
//using System.Diagnostics;
//using System.Drawing;
//using System.IO;
//using System.Net.Http;
//using System.Threading.Tasks;

//namespace Strava.Http
//{
//    /// <summary>
//    /// This class can be used to download a picture.
//    /// </summary>
//    public class ImageLoader
//    {
//        /// <summary>
//        /// DOwnloads a picture from the specified url.
//        /// </summary>
//        /// <param name="uri">The url of the image.</param>
//        /// <returns>The downloaded image.</returns>
//        public async static Task<Image> LoadImage(Uri uri)
//        {
//            if (uri == null)
//            {
//                throw new ArgumentException("The uri object must not be null.");
//            }

//            try
//            {
//                HttpClient client = new HttpClient();
//                Stream stream = await client.GetStreamAsync(uri);
//                Image image = new Bitmap(stream);
//                return image;
//            }
//            catch (Exception ex)
//            {
//                Debug.WriteLine("Couldn't load the image: {0}", ex.Message);
//            }

//            return null;
//        }
//    }
//}
