#region Copyright (C) 2014-2015 Sascha Simon

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
using System.Globalization;
using System.Threading.Tasks;
using Strava.Activities;
using Strava.Api;
using Strava.Authentication;
using Strava.Common;
using Strava.Filters;
using Strava.Http;
using Strava.Segments;

namespace Strava.Clients
{
    /// <summary>
    /// Segments are specific sections of road. Athletes’ times are compared on these segments and leaderboards are created.
    /// With this client you can get various data about segments.
    /// </summary>
    public class SegmentClient : BaseClient
    {
        /// <summary>
        /// Initializes a new instance of the SegmentClient class.
        /// </summary>
        /// <param name="auth">IAuthentication object containing a valid Strava access token.</param>
        public SegmentClient(IAuthentication auth) : base(auth) { }

        #region Async

        /// <summary>
        /// Gets the leaderboard of a segment. You can use various filters to filter the data.
        /// Some of the filters require a Strava Premium account!
        /// </summary>
        /// <param name="segmentId">The Strava segment Id.</param>
        /// <param name="weight">The weight class used to filter the leaderboard.</param>
        /// <param name="age">The age group used to filter the leaderboard.</param>
        /// <param name="time">The time used to filter the leaderboard.</param> 
        /// <param name="gender">The gender used to filter the leaderboard.</param>
        /// <returns>The leaderboard filtered by gender.</returns>
        public async Task<Leaderboard> GetSegmentLeaderboardAsync(string segmentId, WeightFilter weight, AgeFilter age, TimeFilter time, GenderFilter gender)
        {
            int page = 1;

            //Create one dummy request to get the number of entries.
            Leaderboard request = await GetSegmentLeaderboardAsync(segmentId, weight, age, time, gender, 1, 1);
            int totalAthletes = request.EntryCount;

            Leaderboard leaderboard = new Leaderboard
            {
                EffortCount = request.EffortCount,
                EntryCount = request.EntryCount
            };

            while ((page - 1) * 200 < totalAthletes)
            {
                Leaderboard l = await GetSegmentLeaderboardAsync(segmentId, weight, age, time, gender, page++, 200);

                foreach (LeaderboardEntry entry in l.Entries)
                {
                    leaderboard.Entries.Add(entry);
                }
            }

            return leaderboard;
        }

        /// <summary>
        /// Gets the leaderboard of a segment. You can use various filters to filter the data.
        /// Some of the filters require a Strava Premium account!
        /// </summary>
        /// <param name="segmentId">The Strava segment Id.</param>
        /// <param name="weight">The weight class used to filter the leaderboard.</param>
        /// <param name="age">The age group used to filter the leaderboard.</param>
        /// <param name="time">The time used to filter the leaderboard.</param>
        /// <param name="gender">The gender used to filter the leaderboard.</param>
        /// <param name="page">The result page.</param>
        /// <param name="perPage">Efforts shown per page.</param>
        /// <returns>The leaderboard filtered by gender.</returns>
        public async Task<Leaderboard> GetSegmentLeaderboardAsync(string segmentId, WeightFilter weight, AgeFilter age, TimeFilter time, GenderFilter gender, int page, int perPage)
        {
            bool useGender = false;
            bool useTime = false;
            bool useAge = false;
            bool useWeight = false;
            string genderFilter = string.Empty;
            string timeFilter = string.Empty;
            string ageFilter = string.Empty;
            string weightFilter = string.Empty;

            if (!string.IsNullOrEmpty(StringConverter.GenderFilterToString(gender)))
            {
                genderFilter = string.Format("gender={0}", StringConverter.GenderFilterToString(gender));
                useGender = true;
            }
            if (!string.IsNullOrEmpty(StringConverter.TimeFilterToString(time)))
            {
                timeFilter = string.Format("date_range={0}", StringConverter.TimeFilterToString(time));
                useTime = true;
            }
            if (!string.IsNullOrEmpty(StringConverter.AgeFilterToString(age)))
            {
                ageFilter = string.Format("age_group={0}", StringConverter.AgeFilterToString(age));
                useAge = true;
            }
            if (!string.IsNullOrEmpty(StringConverter.WeightFilterToString(weight)))
            {
                weightFilter = string.Format("weight_class={0}", StringConverter.WeightFilterToString(weight));
                useWeight = true;
            }

            string getUrl = string.Format("{0}/{1}/leaderboard?{2}&{3}&{4}&{5}&page={6}&per_page={7}&access_token={8}",
                Endpoints.Leaderboard,
                segmentId,
                useGender ? genderFilter : string.Empty,
                useTime ? timeFilter : string.Empty,
                useAge ? ageFilter : string.Empty,
                useWeight ? weightFilter : string.Empty,
                page,
                perPage,
                Authentication.AccessToken
                );

            string json = await WebRequest.SendGetAsync(new Uri(getUrl));

            return Unmarshaller<Leaderboard>.Unmarshal(json);
        }
        
        /// <summary>
        /// Gets the leaderboard of a segment. You can use various filters to filter the data.
        /// Some of the filters require a Strava Premium account!
        /// </summary>
        /// <param name="segmentId">The Strava segment Id.</param>
        /// <param name="clubId">The club id used to filter the leaderboard.</param>
        /// <param name="weight">The weight class used to filter the leaderboard.</param>
        /// <param name="age">The age group used to filter the leaderboard.</param>
        /// <param name="time">The time used to filter the leaderboard.</param> 
        /// <param name="gender">The gender used to filter the leaderboard.</param>
        /// <returns>The leaderboard filtered by gender.</returns>
        public async Task<Leaderboard> GetSegmentLeaderboardAsync(string segmentId, int clubId, WeightFilter weight, AgeFilter age, TimeFilter time, GenderFilter gender)
        {
            int page = 1;

            //Create one dummy request to get the number of entries.
            Leaderboard request = await GetSegmentLeaderboardAsync(segmentId, clubId, weight, age, time, gender, 1, 1);
            int totalAthletes = request.EntryCount;

            Leaderboard leaderboard = new Leaderboard
            {
                EffortCount = request.EffortCount,
                EntryCount = request.EntryCount
            };

            while ((page - 1) * 200 < totalAthletes)
            {
                Leaderboard l = await GetSegmentLeaderboardAsync(segmentId, clubId, weight, age, time, gender, page++, 200);

                foreach (LeaderboardEntry entry in l.Entries)
                {
                    leaderboard.Entries.Add(entry);
                }
            }

            return leaderboard;
        }

        /// <summary>
        /// Gets the leaderboard of a segment. You can use various filters to filter the data.
        /// Some of the filters require a Strava Premium account!
        /// </summary>
        /// <param name="segmentId">The Strava segment Id.</param>
        /// <param name="clubId">The club id used to filter the leaderboard.</param>
        /// <param name="weight">The weight class used to filter the leaderboard.</param>
        /// <param name="age">The age group used to filter the leaderboard.</param>
        /// <param name="time">The time used to filter the leaderboard.</param>
        /// <param name="gender">The gender used to filter the leaderboard.</param>
        /// <param name="page">The result page.</param>
        /// <param name="perPage">Efforts shown per page.</param>
        /// <returns>The leaderboard filtered by gender.</returns>
        public async Task<Leaderboard> GetSegmentLeaderboardAsync(string segmentId, int clubId, WeightFilter weight, AgeFilter age, TimeFilter time, GenderFilter gender, int page, int perPage)
        {
            bool useGender = false;
            bool useTime = false;
            bool useAge = false;
            bool useWeight = false;
            string genderFilter = string.Empty;
            string timeFilter = string.Empty;
            string ageFilter = string.Empty;
            string weightFilter = string.Empty;

            if (!string.IsNullOrEmpty(StringConverter.GenderFilterToString(gender)))
            {
                genderFilter = string.Format("gender={0}", StringConverter.GenderFilterToString(gender));
                useGender = true;
            }
            if (!string.IsNullOrEmpty(StringConverter.TimeFilterToString(time)))
            {
                timeFilter = string.Format("date_range={0}", StringConverter.TimeFilterToString(time));
                useTime = true;
            }
            if (!string.IsNullOrEmpty(StringConverter.AgeFilterToString(age)))
            {
                ageFilter = string.Format("age_group={0}", StringConverter.AgeFilterToString(age));
                useAge = true;
            }
            if (!string.IsNullOrEmpty(StringConverter.WeightFilterToString(weight)))
            {
                weightFilter = string.Format("weight_class={0}", StringConverter.WeightFilterToString(weight));
                useWeight = true;
            }

            string getUrl = string.Format("{0}/{1}/leaderboard?{2}&{3}&{4}&{5}&club_id={6}&page={7}&per_page={8}&access_token={9}",
                Endpoints.Leaderboard,
                segmentId,
                useGender ? genderFilter : string.Empty,
                useTime ? timeFilter : string.Empty,
                useAge ? ageFilter : string.Empty,
                useWeight ? weightFilter : string.Empty,
                clubId,
                page,
                perPage,
                Authentication.AccessToken
                );

            string json = await WebRequest.SendGetAsync(new Uri(getUrl));

            return Unmarshaller<Leaderboard>.Unmarshal(json);
        }

        /// <summary>
        /// Gets the leaderboard of a segment. You can use various filters to filter the data.
        /// Some of the filters require a Strava Premium account!
        /// </summary>
        /// <param name="segmentId">The Strava segment Id.</param>
        /// <param name="following">The leaderboard only shows athletes you're following.</param>
        /// <param name="weight">The weight class used to filter the leaderboard.</param>
        /// <param name="age">The age group used to filter the leaderboard.</param>
        /// <param name="time">The time used to filter the leaderboard.</param> 
        /// <param name="gender">The gender used to filter the leaderboard.</param>
        /// <returns>The leaderboard filtered by gender.</returns>
        public async Task<Leaderboard> GetSegmentLeaderboardAsync(string segmentId, bool following, WeightFilter weight, AgeFilter age, TimeFilter time, GenderFilter gender)
        {
            int page = 1;

            //Create one dummy request to get the number of entries.
            Leaderboard request = await GetSegmentLeaderboardAsync(segmentId, following, weight, age, time, gender, 1, 1);
            int totalAthletes = request.EntryCount;

            Leaderboard leaderboard = new Leaderboard
            {
                EffortCount = request.EffortCount,
                EntryCount = request.EntryCount
            };

            while ((page - 1) * 200 < totalAthletes)
            {
                Leaderboard l = await GetSegmentLeaderboardAsync(segmentId, following, weight, age, time, gender, page++, 200);

                foreach (LeaderboardEntry entry in l.Entries)
                {
                    leaderboard.Entries.Add(entry);
                }
            }

            return leaderboard;
        }

        /// <summary>
        /// Gets the leaderboard of a segment. You can use various filters to filter the data.
        /// Some of the filters require a Strava Premium account!
        /// </summary>
        /// <param name="segmentId">The Strava segment Id.</param>
        /// <param name="following">The leaderboard only shows athletes you're following.</param>
        /// <param name="weight">The weight class used to filter the leaderboard.</param>
        /// <param name="age">The age group used to filter the leaderboard.</param>
        /// <param name="time">The time used to filter the leaderboard.</param>
        /// <param name="gender">The gender used to filter the leaderboard.</param>
        /// <param name="page">The result page.</param>
        /// <param name="perPage">Efforts shown per page.</param>
        /// <returns>The leaderboard filtered by gender.</returns>
        public async Task<Leaderboard> GetSegmentLeaderboardAsync(string segmentId, bool following, WeightFilter weight, AgeFilter age, TimeFilter time, GenderFilter gender, int page, int perPage)
        {
            bool useGender = false;
            bool useTime = false;
            bool useAge = false;
            bool useWeight = false;
            string genderFilter = string.Empty;
            string timeFilter = string.Empty;
            string ageFilter = string.Empty;
            string weightFilter = string.Empty;

            if (!string.IsNullOrEmpty(StringConverter.GenderFilterToString(gender)))
            {
                genderFilter = string.Format("gender={0}", StringConverter.GenderFilterToString(gender));
                useGender = true;
            }
            if (!string.IsNullOrEmpty(StringConverter.TimeFilterToString(time)))
            {
                timeFilter = string.Format("date_range={0}", StringConverter.TimeFilterToString(time));
                useTime = true;
            }
            if (!string.IsNullOrEmpty(StringConverter.AgeFilterToString(age)))
            {
                ageFilter = string.Format("age_group={0}", StringConverter.AgeFilterToString(age));
                useAge = true;
            }
            if (!string.IsNullOrEmpty(StringConverter.WeightFilterToString(weight)))
            {
                weightFilter = string.Format("weight_class={0}", StringConverter.WeightFilterToString(weight));
                useWeight = true;
            }

            string getUrl = string.Format("{0}/{1}/leaderboard?{2}&{3}&{4}&{5}&following={6}&page={7}&per_page={8}&access_token={9}",
                Endpoints.Leaderboard,
                segmentId,
                useGender ? genderFilter : string.Empty,
                useTime ? timeFilter : string.Empty,
                useAge ? ageFilter : string.Empty,
                useWeight ? weightFilter : string.Empty,
                following,
                page,
                perPage,
                Authentication.AccessToken
                );

            string json = await WebRequest.SendGetAsync(new Uri(getUrl));

            return Unmarshaller<Leaderboard>.Unmarshal(json);
        }

        /// <summary>
        /// Gets all the records of an athlete asynchronously.
        /// </summary>
        /// <param name="athleteId">The Strava athlete id.</param>
        /// <returns>A list of segments where the athlete is the record holder.</returns>
        public async Task<List<SegmentEffort>> GetRecordsAsync(string athleteId)
        {
            string getUrl = string.Format("{0}/{1}/koms?access_token={2}", Endpoints.Athletes, athleteId, Authentication.AccessToken);
            string json = await WebRequest.SendGetAsync(new Uri(getUrl));

            return Unmarshaller<List<SegmentEffort>>.Unmarshal(json);
        }

        /// <summary>
        /// Gets all the starred segments of the currently authenticated athlete asynchronously.
        /// </summary>
        /// <returns>A list of segments that are starred by the currently authenticated athlete.</returns>
        public async Task<List<SegmentSummary>> GetStarredSegmentsAsync()
        {
            string getUrl = string.Format("{0}/?access_token={1}", Endpoints.Starred, Authentication.AccessToken);
            string json = await WebRequest.SendGetAsync(new Uri(getUrl));

            return Unmarshaller<List<SegmentSummary>>.Unmarshal(json);
        }

        /// <summary>
        /// Gets all the starred segments of an Athlete.
        /// </summary>
        /// <returns>A list of segments that are starred by the athlete.</returns>
        public async Task<List<SegmentSummary>> GetStarredSegmentsAsync(string athleteId)
        {
            string getUrl = string.Format("https://www.strava.com/api/v3/athletes/{0}/segments/starred/?access_token={1}", athleteId, Authentication.AccessToken);
            string json = await WebRequest.SendGetAsync(new Uri(getUrl));

            return Unmarshaller<List<SegmentSummary>>.Unmarshal(json);
        }

        /// <summary>
        /// Gets the full and unfiltered leaderboard of a Strava segment asynchronously.
        /// </summary>
        /// <param name="segmentId">The Strava segment Id.</param>
        /// <returns>The full and unfiltered leaderboard></returns>
        public async Task<Leaderboard> GetFullSegmentLeaderboardAsync(string segmentId)
        {
            int page = 1;

            //Create one dummy request to get the number of entries.
            Leaderboard request = await GetSegmentLeaderboardAsync(segmentId, 1, 1);
            int totalAthletes = request.EntryCount;

            Leaderboard leaderboard = new Leaderboard
            {
                EffortCount = request.EffortCount,
                EntryCount = request.EntryCount
            };

            while ((page - 1) * 200 < totalAthletes)
            {
                Leaderboard l = await GetSegmentLeaderboardAsync(segmentId, page++, 200);
                
                foreach (LeaderboardEntry entry in l.Entries)
                {
                    leaderboard.Entries.Add(entry);
                }
            }

            return leaderboard;
        }

        /// <summary>
        /// Gets the leaderboard of a Strava segment.
        /// </summary>
        /// <param name="segmentId">The Strava segment id.</param>
        /// <param name="page">The page.</param>
        /// <param name="perPage">Defines how many entries will be loaded per page.</param>
        /// <returns>The segment leaderboard</returns>
        public async Task<Leaderboard> GetSegmentLeaderboardAsync(string segmentId, int page, int perPage)
        {
            string getUrl = string.Format("{0}/{1}/leaderboard?filter=overall&page={2}&per_page={3}&access_token={4}", 
                Endpoints.Leaderboard, 
                segmentId,
                page,
                perPage,
                Authentication.AccessToken);
            string json = await WebRequest.SendGetAsync(new Uri(getUrl));

            return Unmarshaller<Leaderboard>.Unmarshal(json);
        }

        /// <summary>
        /// Gets the number of entries of a segment.
        /// </summary>
        /// <param name="segmentId">The Strava segment id.</param>
        /// <returns>The total number of entries of the specified Strava segment.</returns>
        public async Task<int> GetSegmentEntryCountAsync(string segmentId)
        {
            string getUrl = string.Format("{0}/{1}/leaderboard?filter=overall&access_token={2}", Endpoints.Leaderboard, segmentId, Authentication.AccessToken);
            string json = await WebRequest.SendGetAsync(new Uri(getUrl));
            Leaderboard leaderboard = Unmarshaller<Leaderboard>.Unmarshal(json);

            return leaderboard.EntryCount;
        }

        /// <summary>
        /// Gets the number of efforts of a segment.
        /// </summary>
        /// <param name="segmentId">The Strava segment id.</param>
        /// <returns>The total number of efforts of the specified Strava segment.</returns>
        public async Task<int> GetSegmentEffortCountAsync(string segmentId)
        {
            string getUrl = string.Format("{0}/{1}/leaderboard?filter=overall&access_token={2}", Endpoints.Leaderboard, segmentId, Authentication.AccessToken);
            string json = await WebRequest.SendGetAsync(new Uri(getUrl));
            Leaderboard leaderboard = Unmarshaller<Leaderboard>.Unmarshal(json);

            return leaderboard.EffortCount;
        }

        /// <summary>
        /// Returns an array of up to ten segments.
        /// </summary>
        /// <param name="southWest">The south western border of the boundary.</param>
        /// <param name="northEast">The north eastern border of the boundary.</param>
        /// <returns>Up to ten segments within the boundary box. When there are more than ten segments, the ten most 
        /// popular ones will be returned.</returns>
        public async Task<ExplorerResult> ExploreSegmentsAsync(Coordinate southWest, Coordinate northEast)
        {
            string bnds = string.Format("{0},{1},{2},{3}",
                southWest.Latitude.ToString(CultureInfo.InvariantCulture),
                southWest.Longitude.ToString(CultureInfo.InvariantCulture),
                northEast.Latitude.ToString(CultureInfo.InvariantCulture),
                northEast.Longitude.ToString(CultureInfo.InvariantCulture)
                );

            string getUrl = string.Format("{0}/explore?bounds={1}&access_token={2}",
                Endpoints.Leaderboard,
                bnds,
                Authentication.AccessToken);

            string json = await WebRequest.SendGetAsync(new Uri(getUrl));

            return Unmarshaller<ExplorerResult>.Unmarshal(json);
        }

        /// <summary>
        /// Returns an array of up to ten segments.
        /// </summary>
        /// <param name="southWest">The south western border of the boundary.</param>
        /// <param name="northEast">The north eastern border of the boundary.</param>
        /// <param name="minCat">The min category 0-5, lower is harder.</param>
        /// <param name="maxCat">The max category 0-5, lower is harder.</param>
        /// <returns>Up to ten segments within the boundary box. When there are more than ten segments, the ten most 
        /// popular ones will be returned.</returns>
        public async Task<ExplorerResult> ExploreSegmentsAsync(Coordinate southWest, Coordinate northEast, int minCat, int maxCat)
        {
            string bnds = string.Format("{0},{1},{2},{3}",
                southWest.Latitude.ToString(CultureInfo.InvariantCulture),
                southWest.Longitude.ToString(CultureInfo.InvariantCulture),
                northEast.Latitude.ToString(CultureInfo.InvariantCulture),
                northEast.Longitude.ToString(CultureInfo.InvariantCulture)
                );

            string getUrl = string.Format("{0}/explore?bounds={1}&min_cat={2}&max_cat={3}&access_token={4}",
                Endpoints.Leaderboard,
                bnds,
                minCat,
                maxCat,
                Authentication.AccessToken);

            string json = await WebRequest.SendGetAsync(new Uri(getUrl));

            return Unmarshaller<ExplorerResult>.Unmarshal(json);
        }

        /// <summary>
        /// Gets a detailed representation of the specified segment id.
        /// </summary>
        /// <param name="segmentId">The Strava segment id.</param>
        /// <returns>A detailed representation of the segment.</returns>
        public async Task<Segment> GetSegmentAsync(string segmentId)
        {
            string getUrl = string.Format("{0}/{1}?access_token={2}",
                Endpoints.Leaderboard,
                segmentId,
                Authentication.AccessToken);

            string json = await WebRequest.SendGetAsync(new Uri(getUrl));

            return Unmarshaller<Segment>.Unmarshal(json);
        }

        #endregion

        #region Sync

        /// <summary>
        /// Gets the leaderboard of a segment. You can use various filters to filter the data.
        /// Some of the filters require a Strava Premium account!
        /// </summary>
        /// <param name="segmentId">The Strava segment Id.</param>
        /// <param name="weight">The weight class used to filter the leaderboard.</param>
        /// <param name="age">The age group used to filter the leaderboard.</param>
        /// <param name="time">The time used to filter the leaderboard.</param> 
        /// <param name="gender">The gender used to filter the leaderboard.</param>
        /// <returns>The leaderboard filtered by gender.</returns>
        public Leaderboard GetSegmentLeaderboard(string segmentId, WeightFilter weight, AgeFilter age, TimeFilter time, GenderFilter gender)
        {
            int page = 1;

            //Create one dummy request to get the number of entries.
            Leaderboard request = GetSegmentLeaderboard(segmentId, weight, age, time, gender, 1, 1);
            int totalAthletes = request.EntryCount;

            Leaderboard leaderboard = new Leaderboard
            {
                EffortCount = request.EffortCount,
                EntryCount = request.EntryCount
            };

            while ((page - 1) * 200 < totalAthletes)
            {
                Leaderboard l = GetSegmentLeaderboard(segmentId, weight, age, time, gender, page++, 200);

                foreach (LeaderboardEntry entry in l.Entries)
                {
                    leaderboard.Entries.Add(entry);
                }
            }

            return leaderboard;
        }

        /// <summary>
        /// Gets the leaderboard of a segment. You can use various filters to filter the data.
        /// Some of the filters require a Strava Premium account!
        /// </summary>
        /// <param name="segmentId">The Strava segment Id.</param>
        /// <param name="weight">The weight class used to filter the leaderboard.</param>
        /// <param name="age">The age group used to filter the leaderboard.</param>
        /// <param name="time">The time used to filter the leaderboard.</param>
        /// <param name="gender">The gender used to filter the leaderboard.</param>
        /// <param name="page">The result page.</param>
        /// <param name="perPage">Efforts shown per page.</param>
        /// <returns>The leaderboard filtered by gender.</returns>
        public Leaderboard GetSegmentLeaderboard(string segmentId, WeightFilter weight, AgeFilter age, TimeFilter time, GenderFilter gender, int page, int perPage)
        {
            bool useGender = false;
            bool useTime = false;
            bool useAge = false;
            bool useWeight = false;
            string genderFilter = string.Empty;
            string timeFilter = string.Empty;
            string ageFilter = string.Empty;
            string weightFilter = string.Empty;

            if (!string.IsNullOrEmpty(StringConverter.GenderFilterToString(gender)))
            {
                genderFilter = string.Format("gender={0}", StringConverter.GenderFilterToString(gender));
                useGender = true;
            }
            if (!string.IsNullOrEmpty(StringConverter.TimeFilterToString(time)))
            {
                timeFilter = string.Format("date_range={0}", StringConverter.TimeFilterToString(time));
                useTime = true;
            }
            if (!string.IsNullOrEmpty(StringConverter.AgeFilterToString(age)))
            {
                ageFilter = string.Format("age_group={0}", StringConverter.AgeFilterToString(age));
                useAge = true;
            }
            if (!string.IsNullOrEmpty(StringConverter.WeightFilterToString(weight)))
            {
                weightFilter = string.Format("weight_class={0}", StringConverter.WeightFilterToString(weight));
                useWeight = true;
            }

            string getUrl = string.Format("{0}/{1}/leaderboard?{2}&{3}&{4}&{5}&page={6}&per_page={7}&access_token={8}",
                Endpoints.Leaderboard,
                segmentId,
                useGender ? genderFilter : string.Empty,
                useTime ? timeFilter : string.Empty,
                useAge ? ageFilter : string.Empty,
                useWeight ? weightFilter : string.Empty,
                page,
                perPage,
                Authentication.AccessToken
                );

            string json = WebRequest.SendGet(new Uri(getUrl));

            return Unmarshaller<Leaderboard>.Unmarshal(json);
        }

        /// <summary>
        /// Gets the leaderboard of a segment. You can use various filters to filter the data.
        /// Some of the filters require a Strava Premium account!
        /// </summary>
        /// <param name="segmentId">The Strava segment Id.</param>
        /// <param name="clubId">The club id used to filter the leaderboard.</param>
        /// <param name="weight">The weight class used to filter the leaderboard.</param>
        /// <param name="age">The age group used to filter the leaderboard.</param>
        /// <param name="time">The time used to filter the leaderboard.</param> 
        /// <param name="gender">The gender used to filter the leaderboard.</param>
        /// <returns>The leaderboard filtered by gender.</returns>
        public Leaderboard GetSegmentLeaderboard(string segmentId, int clubId, WeightFilter weight, AgeFilter age, TimeFilter time, GenderFilter gender)
        {
            int page = 1;

            //Create one dummy request to get the number of entries.
            Leaderboard request = GetSegmentLeaderboard(segmentId, clubId, weight, age, time, gender, 1, 1);
            int totalAthletes = request.EntryCount;

            Leaderboard leaderboard = new Leaderboard
            {
                EffortCount = request.EffortCount,
                EntryCount = request.EntryCount
            };

            while ((page - 1) * 200 < totalAthletes)
            {
                Leaderboard l = GetSegmentLeaderboard(segmentId, clubId, weight, age, time, gender, page++, 200);

                foreach (LeaderboardEntry entry in l.Entries)
                {
                    leaderboard.Entries.Add(entry);
                }
            }

            return leaderboard;
        }

        /// <summary>
        /// Gets the leaderboard of a segment. You can use various filters to filter the data.
        /// Some of the filters require a Strava Premium account!
        /// </summary>
        /// <param name="segmentId">The Strava segment Id.</param>
        /// <param name="clubId">The club id used to filter the leaderboard.</param>
        /// <param name="weight">The weight class used to filter the leaderboard.</param>
        /// <param name="age">The age group used to filter the leaderboard.</param>
        /// <param name="time">The time used to filter the leaderboard.</param>
        /// <param name="gender">The gender used to filter the leaderboard.</param>
        /// <param name="page">The result page.</param>
        /// <param name="perPage">Efforts shown per page.</param>
        /// <returns>The leaderboard filtered by gender.</returns>
        public Leaderboard GetSegmentLeaderboard(string segmentId, int clubId, WeightFilter weight, AgeFilter age, TimeFilter time, GenderFilter gender, int page, int perPage)
        {
            bool useGender = false;
            bool useTime = false;
            bool useAge = false;
            bool useWeight = false;
            string genderFilter = string.Empty;
            string timeFilter = string.Empty;
            string ageFilter = string.Empty;
            string weightFilter = string.Empty;

            if (!string.IsNullOrEmpty(StringConverter.GenderFilterToString(gender)))
            {
                genderFilter = string.Format("gender={0}", StringConverter.GenderFilterToString(gender));
                useGender = true;
            }
            if (!string.IsNullOrEmpty(StringConverter.TimeFilterToString(time)))
            {
                timeFilter = string.Format("date_range={0}", StringConverter.TimeFilterToString(time));
                useTime = true;
            }
            if (!string.IsNullOrEmpty(StringConverter.AgeFilterToString(age)))
            {
                ageFilter = string.Format("age_group={0}", StringConverter.AgeFilterToString(age));
                useAge = true;
            }
            if (!string.IsNullOrEmpty(StringConverter.WeightFilterToString(weight)))
            {
                weightFilter = string.Format("weight_class={0}", StringConverter.WeightFilterToString(weight));
                useWeight = true;
            }

            string getUrl = string.Format("{0}/{1}/leaderboard?{2}&{3}&{4}&{5}&club_id={6}&page={7}&per_page={8}&access_token={9}",
                Endpoints.Leaderboard,
                segmentId,
                useGender ? genderFilter : string.Empty,
                useTime ? timeFilter : string.Empty,
                useAge ? ageFilter : string.Empty,
                useWeight ? weightFilter : string.Empty,
                clubId,
                page,
                perPage,
                Authentication.AccessToken
                );

            string json = WebRequest.SendGet(new Uri(getUrl));

            return Unmarshaller<Leaderboard>.Unmarshal(json);
        }

        /// <summary>
        /// Gets the leaderboard of a segment. You can use various filters to filter the data.
        /// Some of the filters require a Strava Premium account!
        /// </summary>
        /// <param name="segmentId">The Strava segment Id.</param>
        /// <param name="following">The leaderboard only shows athletes you're following.</param>
        /// <param name="weight">The weight class used to filter the leaderboard.</param>
        /// <param name="age">The age group used to filter the leaderboard.</param>
        /// <param name="time">The time used to filter the leaderboard.</param> 
        /// <param name="gender">The gender used to filter the leaderboard.</param>
        /// <returns>The leaderboard filtered by gender.</returns>
        public Leaderboard GetSegmentLeaderboard(string segmentId, bool following, WeightFilter weight, AgeFilter age, TimeFilter time, GenderFilter gender)
        {
            int page = 1;

            //Create one dummy request to get the number of entries.
            Leaderboard request = GetSegmentLeaderboard(segmentId, following, weight, age, time, gender, 1, 1);
            int totalAthletes = request.EntryCount;

            Leaderboard leaderboard = new Leaderboard
            {
                EffortCount = request.EffortCount,
                EntryCount = request.EntryCount
            };

            while ((page - 1) * 200 < totalAthletes)
            {
                Leaderboard l = GetSegmentLeaderboard(segmentId, following, weight, age, time, gender, page++, 200);

                foreach (LeaderboardEntry entry in l.Entries)
                {
                    leaderboard.Entries.Add(entry);
                }
            }

            return leaderboard;
        }

        /// <summary>
        /// Gets the leaderboard of a segment. You can use various filters to filter the data.
        /// Some of the filters require a Strava Premium account!
        /// </summary>
        /// <param name="segmentId">The Strava segment Id.</param>
        /// <param name="following">The leaderboard only shows athletes you're following.</param>
        /// <param name="weight">The weight class used to filter the leaderboard.</param>
        /// <param name="age">The age group used to filter the leaderboard.</param>
        /// <param name="time">The time used to filter the leaderboard.</param>
        /// <param name="gender">The gender used to filter the leaderboard.</param>
        /// <param name="page">The result page.</param>
        /// <param name="perPage">Efforts shown per page.</param>
        /// <returns>The leaderboard filtered by gender.</returns>
        public Leaderboard GetSegmentLeaderboard(string segmentId, bool following, WeightFilter weight, AgeFilter age, TimeFilter time, GenderFilter gender, int page, int perPage)
        {
            bool useGender = false;
            bool useTime = false;
            bool useAge = false;
            bool useWeight = false;
            string genderFilter = string.Empty;
            string timeFilter = string.Empty;
            string ageFilter = string.Empty;
            string weightFilter = string.Empty;

            if (!string.IsNullOrEmpty(StringConverter.GenderFilterToString(gender)))
            {
                genderFilter = string.Format("gender={0}", StringConverter.GenderFilterToString(gender));
                useGender = true;
            }
            if (!string.IsNullOrEmpty(StringConverter.TimeFilterToString(time)))
            {
                timeFilter = string.Format("date_range={0}", StringConverter.TimeFilterToString(time));
                useTime = true;
            }
            if (!string.IsNullOrEmpty(StringConverter.AgeFilterToString(age)))
            {
                ageFilter = string.Format("age_group={0}", StringConverter.AgeFilterToString(age));
                useAge = true;
            }
            if (!string.IsNullOrEmpty(StringConverter.WeightFilterToString(weight)))
            {
                weightFilter = string.Format("weight_class={0}", StringConverter.WeightFilterToString(weight));
                useWeight = true;
            }

            string getUrl = string.Format("{0}/{1}/leaderboard?{2}&{3}&{4}&{5}&following={6}&page={7}&per_page={8}&access_token={9}",
                Endpoints.Leaderboard,
                segmentId,
                useGender ? genderFilter : string.Empty,
                useTime ? timeFilter : string.Empty,
                useAge ? ageFilter : string.Empty,
                useWeight ? weightFilter : string.Empty,
                following,
                page,
                perPage,
                Authentication.AccessToken
                );

            string json = WebRequest.SendGet(new Uri(getUrl));

            return Unmarshaller<Leaderboard>.Unmarshal(json);
        }

        /// <summary>
        /// Gets all the records of an athlete.
        /// </summary>
        /// <param name="athleteId">The Strava athlete id.</param>
        /// <returns>A list of segments where the athlete is the record holder.</returns>
        public List<SegmentEffort> GetRecords(string athleteId)
        {
            string getUrl = string.Format("{0}/{1}/koms?access_token={2}", Endpoints.Athletes, athleteId, Authentication.AccessToken);
            string json = WebRequest.SendGet(new Uri(getUrl));

            return Unmarshaller<List<SegmentEffort>>.Unmarshal(json);
        }

        /// <summary>
        /// Gets all the starred segments of an Athlete.
        /// </summary>
        /// <returns>A list of segments that are starred by the athlete.</returns>
        public List<SegmentSummary> GetStarredSegments(string athleteId)
        {
            string getUrl = string.Format("https://www.strava.com/api/v3/athletes/{0}/segments/starred?access_token={1}", athleteId, Authentication.AccessToken);
            string json = WebRequest.SendGet(new Uri(getUrl));

            return Unmarshaller<List<SegmentSummary>>.Unmarshal(json);
        }

        /// <summary>
        /// Gets all the starred segments of the currently authenticated athlete.
        /// </summary>
        /// <returns>A list of segments that are starred by the currently authenticated athlete.</returns>
        public List<SegmentSummary> GetStarredSegments()
        {
            string getUrl = string.Format("{0}?access_token={1}", Endpoints.Starred, Authentication.AccessToken);
            string json = WebRequest.SendGet(new Uri(getUrl));

            return Unmarshaller<List<SegmentSummary>>.Unmarshal(json);
        }

        /// <summary>
        /// Gets the full and unfiltered leaderboard of a Strava segment.
        /// </summary>
        /// <param name="segmentId">The Strava segment Id.</param>
        /// <returns>The full and unfiltered leaderboard.</returns>
        public Leaderboard GetFullSegmentLeaderboard(string segmentId)
        {
            int page = 1;

            //Create one dummy request to get the number of entries.
            Leaderboard request = GetSegmentLeaderboard(segmentId, 1, 1);
            int totalAthletes = request.EntryCount;

            Leaderboard leaderboard = new Leaderboard
            {
                EffortCount = request.EffortCount,
                EntryCount = request.EntryCount
            };

            while ((page - 1) * 200 < totalAthletes)
            {
                Leaderboard l = GetSegmentLeaderboard(segmentId, page++, 200);

                foreach (LeaderboardEntry entry in l.Entries)
                {
                    leaderboard.Entries.Add(entry);
                }
            }

            return leaderboard;
        }

        /// <summary>
        /// Gets the leaderboard of a Strava segment.
        /// </summary>
        /// <param name="segmentId">The Strava segment id.</param>
        /// <param name="page">The page.</param>
        /// <param name="perPage">Defines how many entries will be loaded per page.</param>
        /// <returns>The segment leaderboard</returns>
        public Leaderboard GetSegmentLeaderboard(string segmentId, int page, int perPage)
        {
            string getUrl = string.Format("{0}/{1}/leaderboard?filter=overall&page={2}&per_page={3}&access_token={4}",
                Endpoints.Leaderboard,
                segmentId,
                page,
                perPage,
                Authentication.AccessToken);
            string json = WebRequest.SendGet(new Uri(getUrl));

            return Unmarshaller<Leaderboard>.Unmarshal(json);
        }

        /// <summary>
        /// Gets the number of entries of a segment.
        /// </summary>
        /// <param name="segmentId">The Strava segment id.</param>
        /// <returns>The total number of entries of the specified Strava segment.</returns>
        public int GetSegmentEntryCount(string segmentId)
        {
            string getUrl = string.Format("{0}/{1}/leaderboard?filter=overall&access_token={2}", Endpoints.Leaderboard, segmentId, Authentication.AccessToken);
            string json = WebRequest.SendGet(new Uri(getUrl));
            Leaderboard leaderboard = Unmarshaller<Leaderboard>.Unmarshal(json);

            return leaderboard.EntryCount;
        }

        /// <summary>
        /// Gets the number of efforts of a segment.
        /// </summary>
        /// <param name="segmentId">The Strava segment id.</param>
        /// <returns>The total number of efforts of the specified Strava segment.</returns>
        public int GetSegmentEffortCount(string segmentId)
        {
            string getUrl = string.Format("{0}/{1}/leaderboard?filter=overall&access_token={2}", Endpoints.Leaderboard, segmentId, Authentication.AccessToken);
            string json = WebRequest.SendGet(new Uri(getUrl));
            Leaderboard leaderboard = Unmarshaller<Leaderboard>.Unmarshal(json);

            return leaderboard.EffortCount;
        }

        /// <summary>
        /// Returns an array of up to ten segments.
        /// </summary>
        /// <param name="southWest">The south western border of the boundary.</param>
        /// <param name="northEast">The north eastern border of the boundary.</param>
        /// <returns>Up to ten segments within the boundary box. When there are more than ten segments, the ten most 
        /// popular ones will be returned.</returns>
        public ExplorerResult ExploreSegments(Coordinate southWest, Coordinate northEast)
        {
            string bnds = string.Format("{0},{1},{2},{3}",
                southWest.Latitude.ToString(CultureInfo.InvariantCulture),
                southWest.Longitude.ToString(CultureInfo.InvariantCulture),
                northEast.Latitude.ToString(CultureInfo.InvariantCulture),
                northEast.Longitude.ToString(CultureInfo.InvariantCulture)
                );

            string getUrl = string.Format("{0}/explore?bounds={1}&access_token={2}", Endpoints.Leaderboard, bnds, Authentication.AccessToken);
            string json = WebRequest.SendGet(new Uri(getUrl));

            return Unmarshaller<ExplorerResult>.Unmarshal(json);
        }

        /// <summary>
        /// Returns an array of up to ten segments.
        /// </summary>
        /// <param name="southWest">The south western border of the boundary.</param>
        /// <param name="northEast">The north eastern border of the boundary.</param>
        /// <param name="minCat">The min category 0-5, lower is harder.</param>
        /// <param name="maxCat">The max category 0-5, lower is harder.</param>
        /// <returns>Up to ten segments within the boundary box. When there are more than ten segments, the ten most 
        /// popular ones will be returned.</returns>
        public ExplorerResult ExploreSegments(Coordinate southWest, Coordinate northEast, int minCat, int maxCat)
        {
            string bnds = string.Format("{0},{1},{2},{3}",
                southWest.Latitude.ToString(CultureInfo.InvariantCulture),
                southWest.Longitude.ToString(CultureInfo.InvariantCulture),
                northEast.Latitude.ToString(CultureInfo.InvariantCulture),
                northEast.Longitude.ToString(CultureInfo.InvariantCulture)
                );

            string getUrl = string.Format("{0}/explore?bounds={1}&min_cat={2}&max_cat={3}&access_token={4}",
                Endpoints.Leaderboard, bnds, minCat, maxCat, Authentication.AccessToken);
            string json = WebRequest.SendGet(new Uri(getUrl));

            return Unmarshaller<ExplorerResult>.Unmarshal(json);
        }

        /// <summary>
        /// Gets a detailed representation of the specified segment id.
        /// </summary>
        /// <param name="segmentId">The Strava segment id.</param>
        /// <returns>A detailed representation of the segment.</returns>
        public Segment GetSegment(string segmentId)
        {
            string getUrl = string.Format("{0}/{1}?access_token={2}", Endpoints.Leaderboard, segmentId, Authentication.AccessToken);
            string json = WebRequest.SendGet(new Uri(getUrl));

            return Unmarshaller<Segment>.Unmarshal(json);
        }
        
        #endregion
    }
}
