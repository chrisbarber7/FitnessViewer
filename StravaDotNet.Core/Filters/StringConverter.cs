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

namespace Strava.Filters
{
    /// <summary>
    /// Converts various Filters to a string that can be used as a parameter in a Strava request.
    /// </summary>
    public static class StringConverter
    {
        /// <summary>
        /// Converts the time filter to the appropriate string.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>The appropriate string for the specified filter.</returns>
        public static string TimeFilterToString(TimeFilter filter)
        {
            switch (filter)
            {
                case TimeFilter.ThisMonth:
                    return "this_month";
                case TimeFilter.ThisWeek:
                    return "this_week";
                case TimeFilter.ThisYear:
                    return "this_year";
                case TimeFilter.Today:
                    return "today";

                default:
                    return string.Empty;

            }
        }

        /// <summary>
        /// Converts the gender filter object to the appropriate string.
        /// </summary>
        /// <param name="gender">The gender filter.</param>
        /// <returns>The appropriate string for the specified filter.</returns>
        public static string GenderFilterToString(GenderFilter gender)
        {
            switch (gender)
            {
                case GenderFilter.Male:
                    return "M";
                case GenderFilter.Female:
                    return "F";

                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Converts the age filter object to the appropriate string.
        /// </summary>
        /// <param name="age">The age filter.</param>
        /// <returns>The appropriate string for the specified filter.</returns>
        public static string AgeFilterToString(AgeFilter age)
        {
            switch (age)
            {
                case AgeFilter.One:
                    return "0-24";
                case AgeFilter.Two:
                    return "age=25-34";
                case AgeFilter.Three:
                    return "35-44";
                case AgeFilter.Four:
                    return "45-54";
                case AgeFilter.Five:
                    return "55-64";
                case AgeFilter.Six:
                    return "65_plus";

                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Converts the weight filter object to the appropriate string.
        /// </summary>
        /// <param name="weight">The weight class.</param>
        /// <returns>The appropriate string for the specified weight class.</returns>
        public static string WeightFilterToString(WeightFilter weight)
        {
            switch (weight)
            {
                case WeightFilter.One:
                    return "0-54";
                case WeightFilter.Two:
                    return "55-64";
                case WeightFilter.Three:
                    return "65-74";
                case WeightFilter.Four:
                    return "75-84";
                case WeightFilter.Five:
                    return "85-94";
                case WeightFilter.Six:
                    return "95_plus";

                default:
                    return string.Empty;
            }
        }
    }
}
