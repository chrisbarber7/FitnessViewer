﻿using FitnessViewer.Infrastructure.enums;
using FitnessViewer.Infrastructure.Helpers.Conversions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Helpers
{
    public static class ExtensionMethods
    {

        public static decimal ToSportDistance(this decimal distanceInMetres, SportType sport)
        {
            switch(sport)
            {
                case SportType.Ride: { return distanceInMetres.ToMiles(); }
                case SportType.Run: { return distanceInMetres.ToMiles(); }
                case SportType.Swim: { return distanceInMetres; }
                case SportType.Other: { return distanceInMetres.ToMiles(); }
                default: { return distanceInMetres.ToMiles(); }
            }
        }


        /// <summary>
        /// Convert a distance in meters to a distance in miles.
        /// </summary>
        /// <param name="distanceInMetres">Distance to convert (in meters)</param>
        /// <returns>Distance in Miles</returns>
        public static decimal ToMiles(this decimal distanceInMetres)
        {
            return Distance.MetersToMiles(distanceInMetres);
        }

        /// <summary>
        /// Convert a distance in meters to a distance in feet.
        /// </summary>
        /// <param name="distanceInMetres">Distance to convert (in meters)</param>
        /// <returns>Distance in Miles</returns>
        public static decimal ToFeet(this decimal distanceInMetres)
        {
            return Distance.MetersToFeet(distanceInMetres);
        }

        /// <summary>
        /// Convert a distance in meters to a distance in feet.
        /// </summary>
        /// <param name="distanceInMetres">Distance to convert (in meters)</param>
        /// <returns>Distance in Miles</returns>
        public static decimal ToFeet(this int distanceInMetres)
        {
            return Distance.MetersToFeet(distanceInMetres);
        }

        /// <summary>
        /// Convert a distance in meters to a distance in feet.
        /// </summary>
        /// <param name="distanceInMetres">Distance to convert (in meters)</param>
        /// <returns>Distance in Miles</returns>
        public static decimal ToFeet(this double distanceInMetres)
        {
            return Distance.MetersToFeet(Convert.ToDecimal(distanceInMetres));
        }

        /// <summary>
        ///  Format TimeSpan/Duration as MM:SS
        /// </summary>
        /// <param name="duration">Time</param>
        /// <returns>Time formatted as MM:SS</returns>
        public static string ToMinSec(this TimeSpan duration)
        {
            return string.Format("{0}:{1}", duration.Minutes.ToString().PadLeft(2, '0'), duration.Seconds.ToString().PadLeft(2, '0'));
        }

        /// <summary>
        /// Get sport type as a string.
        /// </summary>
        /// <param name="sport">SportType enum to convert</param>
        /// <returns>SportType as a string</returns>
        public static string ToString(this SportType sport)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])sport.GetType().GetField(sport.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }

        /// <summary>
        /// Convert DateTime to Unix timestamp.
        /// </summary>
        /// <param name="date">Date to convert</param>
        /// <returns></returns>
        public static int ToUnixDateTime(this DateTime date)
        {
            return DateHelpers.DateTimeToUnixTimeStamp(date);
        }
    }
}
