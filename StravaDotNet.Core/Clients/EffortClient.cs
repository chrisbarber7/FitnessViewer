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
using System.Threading.Tasks;
using Strava.Api;
using Strava.Authentication;
using Strava.Common;
using Strava.Http;
using Strava.Segments;

namespace Strava.Clients
{
    /// <summary>
    /// The EffortClient class is used to retrieve efforts of an segment.
    /// </summary>
    public class EffortClient : BaseClient
    {
        /// <summary>
        /// Intializes a new instance of the EffortClient class.
        /// </summary>
        /// <param name="auth">The IAuthentication object.</param>
        public EffortClient(IAuthentication auth) : base(auth) { }

        #region Async

        /// <summary>
        /// Gets all efforts on the specified segment (all athletes).
        /// </summary>
        /// <param name="segmentId">The Strava segment id.</param>
        /// <param name="after">The date after all the efforts have been recorded.</param>
        /// <param name="before">The date before all the efforts have been recorded.</param>
        /// <returns>A list of segment efforts</returns>
        public async Task<List<SegmentEffort>> GetSegmentEffortsByTimeAsync(string segmentId, DateTime after, DateTime before)
        {
            List<SegmentEffort> activities = new List<SegmentEffort>();
            int page = 1;
            bool hasEntries = true;

            while (hasEntries)
            {
                List<SegmentEffort> request = await GetSegmentEffortsByTimeAsync(segmentId, after, before, page++, 200);

                if (request.Count == 0)
                {
                    hasEntries = false;
                }

                foreach (SegmentEffort effort in request)
                {
                    activities.Add(effort);
                }
            }

            return activities;
        }

        /// <summary>
        /// Gets all efforts on the specified segment (all athletes).
        /// </summary>
        /// <param name="segmentId">The Strava segment id.</param>
        /// <param name="after">The date after all the efforts have been recorded.</param>
        /// <param name="before">The date before all the efforts have been recorded.</param>
        /// <param name="page">The results page.</param>
        /// <param name="perPage">The number of results shown per page.</param>
        /// <returns>A list of segment efforts</returns>
        public async Task<List<SegmentEffort>> GetSegmentEffortsByTimeAsync(string segmentId, DateTime after, DateTime before, int page, int perPage)
        {
            string getUrl = string.Format("{0}/{1}/all_efforts?start_date_local={2}&end_date_local={3}&page={4}&per_page={5}&access_token={6}",
                Endpoints.Leaderboard,
                segmentId,
                after.ToString("O"),
                before.ToString("O"),
                page,
                perPage,
                Authentication.AccessToken
                );

            string json = await WebRequest.SendGetAsync(new Uri(getUrl));

            return Unmarshaller<List<SegmentEffort>>.Unmarshal(json);
        }

        /// <summary>
        /// Gets all efforts on the specified segment of the specified athlete.
        /// </summary>
        /// <param name="segmentId">The Strava segment id.</param>
        /// <param name="athleteId">The Strava athlete id.</param>
        /// <returns>A list of efforts from the athlete on the specified segment.</returns>
        public async Task<List<SegmentEffort>> GetSegmentEffortsByAthleteAsync(string segmentId, string athleteId)
        {
            List<SegmentEffort> activities = new List<SegmentEffort>();
            int page = 1;
            bool hasEntries = true;

            while (hasEntries)
            {
                List<SegmentEffort> request = await GetSegmentEffortsByAthleteAsync(segmentId, athleteId, page++, 200);

                if (request.Count == 0)
                {
                    hasEntries = false;
                }

                foreach (SegmentEffort effort in request)
                {
                    activities.Add(effort);
                }
            }

            return activities;
        }

        /// <summary>
        /// Gets all efforts on the specified segment of the specified athlete.
        /// </summary>
        /// <param name="segmentId">The Strava segment id.</param>
        /// <param name="athleteId">The Strava athlete id.</param>
        /// <param name="page">The results page.</param>
        /// <param name="perPage">The number of results shown per page.</param>
        /// <returns>A list of efforts from the athlete on the specified segment.</returns>
        public async Task<List<SegmentEffort>> GetSegmentEffortsByAthleteAsync(string segmentId, string athleteId, int page, int perPage)
        {
            string getUrl = string.Format("{0}/{1}/all_efforts?athlete_id={2}&page={3}&per_page={4}&access_token={5}",
                Endpoints.Leaderboard,
                segmentId,
                athleteId,
                page,
                perPage,
                Authentication.AccessToken
                );

            string json = await WebRequest.SendGetAsync(new Uri(getUrl));

            return Unmarshaller<List<SegmentEffort>>.Unmarshal(json);
        }

        /// <summary>
        /// Gets all efforts on the specified segment of the specified athlete.
        /// </summary>
        /// <param name="segmentId">The Strava segment id.</param> 
        /// <param name="athleteId">The Strava athlete id.</param>
        /// <param name="after">The date after all the efforts have been recorded.</param>
        /// <param name="before">The date before all the efforts have been recorded.</param>
        /// <returns>A list of efforts from the athlete on the specified segment.</returns>
        public async Task<List<SegmentEffort>> GetSegmentEffortsAsync(string segmentId, string athleteId, DateTime after, DateTime before)
        {
            List<SegmentEffort> activities = new List<SegmentEffort>();
            int page = 1;
            bool hasEntries = true;

            while (hasEntries)
            {
                List<SegmentEffort> request = await GetSegmentEffortsAsync(segmentId, athleteId, after, before, page++, 200);

                if (request.Count == 0)
                {
                    hasEntries = false;
                }

                foreach (SegmentEffort effort in request)
                {
                    activities.Add(effort);
                }
            }

            return activities;
        }

        /// <summary>
        /// Gets all efforts on the specified segment of the specified athlete.
        /// </summary>
        /// <param name="segmentId">The Strava segment id.</param> 
        /// <param name="athleteId">The Strava athlete id.</param>
        /// <param name="after">The date after all the efforts have been recorded.</param>
        /// <param name="before">The date before all the efforts have been recorded.</param>
        /// <param name="page">The results page.</param>
        /// <param name="perPage">The number of results shown per page.</param>
        /// <returns>A list of efforts from the athlete on the specified segment.</returns>
        public async Task<List<SegmentEffort>> GetSegmentEffortsAsync(string segmentId, string athleteId, DateTime after, DateTime before, int page, int perPage)
        {
            string getUrl = string.Format("{0}/{1}/all_efforts?athlete_id={2}&start_date_local={3}&end_date_local={4}&page={5}per_page={6}&access_token={7}",
                Endpoints.Leaderboard,
                segmentId,
                athleteId,
                after.ToString("O"),
                before.ToString("O"),
                page,
                perPage,
                Authentication.AccessToken
                );

            string json = await WebRequest.SendGetAsync(new Uri(getUrl));

            return Unmarshaller<List<SegmentEffort>>.Unmarshal(json);
        }

        /// <summary>
        /// Gets all efforts on the specified segment of the specified athlete.
        /// </summary>
        /// <param name="segmentId">The Strava segment id.</param> 
        /// <returns>A list of efforts from the athlete on the specified segment.</returns>
        public async Task<List<SegmentEffort>> GetSegmentEffortsAsync(string segmentId)
        {
            List<SegmentEffort> activities = new List<SegmentEffort>();
            int page = 1;
            bool hasEntries = true;

            while (hasEntries)
            {
                List<SegmentEffort> request = await GetSegmentEffortsAsync(segmentId, page++, 200);

                if (request.Count == 0)
                {
                    hasEntries = false;
                }

                foreach (SegmentEffort effort in request)
                {
                    activities.Add(effort);
                }
            }

            return activities;
        }

        /// <summary>
        /// Gets all efforts on the specified segment of the specified athlete.
        /// </summary>
        /// <param name="segmentId">The Strava segment id.</param>
        /// <param name="page">The results page.</param>
        /// <param name="perPage">The number of results shown per page.</param>
        /// <returns>A list of efforts from the athlete on the specified segment.</returns>
        public async Task<List<SegmentEffort>> GetSegmentEffortsAsync(string segmentId, int page, int perPage)
        {
            string getUrl = string.Format("{0}/{1}/all_efforts?page={2}per_page={3}&access_token={4}",
                Endpoints.Leaderboard,
                segmentId,
                page,
                perPage,
                Authentication.AccessToken
                );

            string json = await WebRequest.SendGetAsync(new Uri(getUrl));

            return Unmarshaller<List<SegmentEffort>>.Unmarshal(json);
        }

        #endregion
        
        #region Sync

        /// <summary>
        /// Gets all efforts on the specified segment (all athletes).
        /// </summary>
        /// <param name="segmentId">The Strava segment id.</param>
        /// <param name="after">The date after all the efforts have been recorded.</param>
        /// <param name="before">The date before all the efforts have been recorded.</param>
        /// <returns>A list of segment efforts</returns>
        public List<SegmentEffort> GetSegmentEffortsByTime(string segmentId, DateTime after, DateTime before)
        {
            List<SegmentEffort> activities = new List<SegmentEffort>();
            int page = 1;
            bool hasEntries = true;

            while (hasEntries)
            {
                List<SegmentEffort> request = GetSegmentEffortsByTime(segmentId, after, before, page++, 200);

                if (request.Count == 0)
                {
                    hasEntries = false;
                }

                foreach (SegmentEffort effort in request)
                {
                    activities.Add(effort);
                }
            }

            return activities;
        }

        /// <summary>
        /// Gets all efforts on the specified segment (all athletes).
        /// </summary>
        /// <param name="segmentId">The Strava segment id.</param>
        /// <param name="after">The date after all the efforts have been recorded.</param>
        /// <param name="before">The date before all the efforts have been recorded.</param>
        /// <param name="page">The results page.</param>
        /// <param name="perPage">The number of results shown per page.</param>
        /// <returns>A list of segment efforts</returns>
        public List<SegmentEffort> GetSegmentEffortsByTime(string segmentId, DateTime after, DateTime before, int page, int perPage)
        {
            string getUrl = string.Format("{0}/{1}/all_efforts?start_date_local={2}&end_date_local={3}&page={4}&per_page={5}&access_token={6}",
                Endpoints.Leaderboard,
                segmentId,
                after.ToString("O"),
                before.ToString("O"),
                page,
                perPage,
                Authentication.AccessToken
                );

            string json = WebRequest.SendGet(new Uri(getUrl));

            return Unmarshaller<List<SegmentEffort>>.Unmarshal(json);
        }

        /// <summary>
        /// Gets all efforts on the specified segment of the specified athlete.
        /// </summary>
        /// <param name="segmentId">The Strava segment id.</param>
        /// <param name="athleteId">The Strava athlete id.</param>
        /// <returns>A list of efforts from the athlete on the specified segment.</returns>
        public List<SegmentEffort> GetSegmentEffortsByAthlete(string segmentId, string athleteId)
        {
            List<SegmentEffort> activities = new List<SegmentEffort>();
            int page = 1;
            bool hasEntries = true;

            while (hasEntries)
            {
                List<SegmentEffort> request = GetSegmentEffortsByAthlete(segmentId, athleteId, page++, 200);

                if (request.Count == 0)
                {
                    hasEntries = false;
                }

                foreach (SegmentEffort effort in request)
                {
                    activities.Add(effort);
                }
            }

            return activities;
        }

        /// <summary>
        /// Gets all efforts on the specified segment of the specified athlete.
        /// </summary>
        /// <param name="segmentId">The Strava segment id.</param>
        /// <param name="athleteId">The Strava athlete id.</param>
        /// <param name="page">The results page.</param>
        /// <param name="perPage">The number of results shown per page.</param>
        /// <returns>A list of efforts from the athlete on the specified segment.</returns>
        public List<SegmentEffort> GetSegmentEffortsByAthlete(string segmentId, string athleteId, int page, int perPage)
        {
            string getUrl = string.Format("{0}/{1}/all_efforts?athlete_id={2}&page={3}&per_page={4}&access_token={5}",
                Endpoints.Leaderboard,
                segmentId,
                athleteId,
                page,
                perPage,
                Authentication.AccessToken
                );

            string json = WebRequest.SendGet(new Uri(getUrl));

            return Unmarshaller<List<SegmentEffort>>.Unmarshal(json);
        }
        
        /// <summary>
        /// Gets all efforts on the specified segment of the specified athlete.
        /// </summary>
        /// <param name="segmentId">The Strava segment id.</param> 
        /// <param name="athleteId">The Strava athlete id.</param>
        /// <param name="after">The date after all the efforts have been recorded.</param>
        /// <param name="before">The date before all the efforts have been recorded.</param>
        /// <returns>A list of efforts from the athlete on the specified segment.</returns>
        public List<SegmentEffort> GetSegmentEfforts(string segmentId, string athleteId, DateTime after, DateTime before)
        {
            List<SegmentEffort> activities = new List<SegmentEffort>();
            int page = 1;
            bool hasEntries = true;

            while (hasEntries)
            {
                List<SegmentEffort> request = GetSegmentEfforts(segmentId, athleteId, after, before, page++, 200);

                if (request.Count == 0)
                {
                    hasEntries = false;
                }

                foreach (SegmentEffort effort in request)
                {
                    activities.Add(effort);
                }
            }

            return activities;
        }

        /// <summary>
        /// Gets all efforts on the specified segment of the specified athlete.
        /// </summary>
        /// <param name="segmentId">The Strava segment id.</param> 
        /// <param name="athleteId">The Strava athlete id.</param>
        /// <param name="after">The date after all the efforts have been recorded.</param>
        /// <param name="before">The date before all the efforts have been recorded.</param>
        /// <param name="page">The results page.</param>
        /// <param name="perPage">The number of results shown per page.</param>
        /// <returns>A list of efforts from the athlete on the specified segment.</returns>
        public List<SegmentEffort> GetSegmentEfforts(string segmentId, string athleteId, DateTime after, DateTime before, int page, int perPage)
        {
            string getUrl = string.Format("{0}/{1}/all_efforts?athlete_id={2}&start_date_local={3}&end_date_local={4}&page={5}per_page={6}&access_token={7}",
                Endpoints.Leaderboard,
                segmentId,
                athleteId,
                after.ToString("O"),
                before.ToString("O"),
                page,
                perPage,
                Authentication.AccessToken
                );

            string json = WebRequest.SendGet(new Uri(getUrl));

            return Unmarshaller<List<SegmentEffort>>.Unmarshal(json);
        }

        /// <summary>
        /// Gets all efforts on the specified segment of the specified athlete.
        /// </summary>
        /// <param name="segmentId">The Strava segment id.</param> 
        /// <returns>A list of efforts from the athlete on the specified segment.</returns>
        public List<SegmentEffort> GetSegmentEfforts(string segmentId)
        {
            List<SegmentEffort> activities = new List<SegmentEffort>();
            int page = 1;
            bool hasEntries = true;

            while (hasEntries)
            {
                List<SegmentEffort> request = GetSegmentEfforts(segmentId, page++, 200);

                if (request.Count == 0)
                {
                    hasEntries = false;
                }

                foreach (SegmentEffort effort in request)
                {
                    activities.Add(effort);
                }
            }

            return activities;
        }

        /// <summary>
        /// Gets all efforts on the specified segment of the specified athlete.
        /// </summary>
        /// <param name="segmentId">The Strava segment id.</param>
        /// <param name="page">The results page.</param>
        /// <param name="perPage">The number of results shown per page.</param>
        /// <returns>A list of efforts from the athlete on the specified segment.</returns>
        public List<SegmentEffort> GetSegmentEfforts(string segmentId, int page, int perPage)
        {
            string getUrl = string.Format("{0}/{1}/all_efforts?page={2}per_page={3}&access_token={4}",
                Endpoints.Leaderboard,
                segmentId,
                page,
                perPage,
                Authentication.AccessToken
                );

            string json = WebRequest.SendGet(new Uri(getUrl));

            return Unmarshaller<List<SegmentEffort>>.Unmarshal(json);
        }

        #endregion
    }
}
