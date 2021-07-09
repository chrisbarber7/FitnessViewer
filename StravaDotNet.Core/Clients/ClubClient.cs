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
using Strava.Activities;
using Strava.Api;
using Strava.Athletes;
using Strava.Authentication;
using Strava.Clubs;
using Strava.Common;
using Strava.Http;

namespace Strava.Clients
{
    /// <summary>
    /// Used to receive information about clubs from Strava.
    /// </summary>
    public class ClubClient : BaseClient
    {
        /// <summary>
        /// Initializes a new instance of the ClubClient class.
        /// </summary>
        /// <param name="auth">IAuthentication object that contains a valid access token.</param>
        public ClubClient(IAuthentication auth) : base(auth) { }

        #region Async

        /// <summary>
        /// Gets the club which the specified id.
        /// </summary>
        /// <param name="clubId">The id of the club.</param>
        /// <returns>The Club object containing detailed information about the club.</returns>
        public async Task<Club> GetClubAsync(string clubId)
        {
            string getUrl = string.Format("{0}/{1}?access_token={2}", Endpoints.Club, clubId, Authentication.AccessToken);
            string json = await WebRequest.SendGetAsync(new Uri(getUrl));

            return Unmarshaller<Club>.Unmarshal(json);
        }

        /// <summary>
        /// Gets a list of clubs in which the currently authenticated athlete is a member of.
        /// </summary>
        /// <returns>The list of clubs in which the currently authenticated user is a member of.</returns>
        public async Task<List<ClubSummary>> GetClubsAsync()
        {
            string getUrl = string.Format("{0}?access_token={1}", Endpoints.Clubs, Authentication.AccessToken);
            string json = await WebRequest.SendGetAsync(new Uri(getUrl));

            return Unmarshaller<List<ClubSummary>>.Unmarshal(json);
        }

        /// <summary>
        /// Gets the members of the specified club.
        /// </summary>
        /// <param name="clubId">The club's id.</param>
        /// <returns>The club's members.</returns>
        public async Task<List<AthleteSummary>> GetClubMembersAsync(string clubId)
        {
            string getUrl = string.Format("{0}/{1}/members?access_token={2}", Endpoints.Club, clubId, Authentication.AccessToken);
            string json = await WebRequest.SendGetAsync(new Uri(getUrl));

            return Unmarshaller<List<AthleteSummary>>.Unmarshal(json);
        }

        /// <summary>
        /// Gets a list of the latest club activities.
        /// </summary>
        /// <param name="clubId">Id of the club you want to get the activities of.</param>
        /// <returns></returns>
        public async Task<List<ActivitySummary>> GetLatestClubActivitiesAsync(string clubId)
        {
            string getUrl = string.Format("{0}/{1}/activities?access_token={2}", Endpoints.Club, clubId, Authentication.AccessToken);
            string json = await WebRequest.SendGetAsync(new Uri(getUrl));

            return Unmarshaller<List<ActivitySummary>>.Unmarshal(json);
        }

        /// <summary>
        /// Gets a list of the latest club activities.
        /// </summary>
        /// <param name="clubId">Id of the club you want to get the activities of.</param>
        /// <param name="page">The page of the activities list.</param>
        /// <param name="perPage">Specifies how many activities shpuld be shown per page.</param>
        /// <returns></returns>
        public async Task<List<ActivitySummary>> GetLatestClubActivitiesAsync(string clubId, int page, int perPage)
        {
            string getUrl = string.Format("{0}/{1}/activities?page={2}&per_page={3}&access_token={4}",
                Endpoints.Club,
                clubId,
                page,
                perPage,
                Authentication.AccessToken);
            string json = await WebRequest.SendGetAsync(new Uri(getUrl));

            return Unmarshaller<List<ActivitySummary>>.Unmarshal(json);
        }

        #endregion

        #region Sync

        /// <summary>
        /// Gets the club which the specified id.
        /// </summary>
        /// <param name="clubId">The id of the club.</param>
        /// <returns>The Club object containing detailed information about the club.</returns>
        public Club GetClub(string clubId)
        {
            string getUrl = string.Format("{0}/{1}?access_token={2}", Endpoints.Club, clubId, Authentication.AccessToken);
            string json = WebRequest.SendGet(new Uri(getUrl));

            return Unmarshaller<Club>.Unmarshal(json);
        }

        /// <summary>
        /// Join a Strava Club.
        /// </summary>
        /// <param name="clubId">The Strava Id of the club you want to join.</param>
        public async void JoinClub(string clubId)
        {
            string postUrl = string.Format("https://www.strava.com/api/v3/clubs/{0}/join?access_token={1}", clubId, Authentication.AccessToken);
            await WebRequest.SendPostAsync(new Uri(postUrl));
        }

        /// <summary>
        /// Leave a Strava Club.
        /// </summary>
        /// <param name="clubId">The Strava Id of the club you want to leave.</param>
        public async void LeaveClub(string clubId)
        {
            string postUrl = string.Format("https://www.strava.com/api/v3/clubs/{0}/leave?access_token={1}", clubId, Authentication.AccessToken);
            await WebRequest.SendPostAsync(new Uri(postUrl));
        }

        /// <summary>
        /// Gets a list of clubs in which the currently authenticated athlete is a member of.
        /// </summary>
        /// <returns>The list of clubs in which the currently authenticated user is a member of.</returns>
        public List<ClubSummary> GetClubs()
        {
            string getUrl = string.Format("{0}?access_token={1}", Endpoints.Clubs, Authentication.AccessToken);
            string json = WebRequest.SendGet(new Uri(getUrl));

            return Unmarshaller<List<ClubSummary>>.Unmarshal(json);
        }

        /// <summary>
        /// Gets the members of the specified club.
        /// </summary>
        /// <param name="clubId">The club's id.</param>
        /// <returns>The club's members.</returns>
        public List<AthleteSummary> GetClubMembers(string clubId)
        {
            string getUrl = string.Format("{0}/{1}/members?access_token={2}", Endpoints.Club, clubId, Authentication.AccessToken);
            string json = WebRequest.SendGet(new Uri(getUrl));

            return Unmarshaller<List<AthleteSummary>>.Unmarshal(json);
        }

        /// <summary>
        /// Gets a list of the latest club activities.
        /// </summary>
        /// <param name="clubId">Id of the club you want to get the activities of.</param>
        /// <returns></returns>
        public List<ActivitySummary> GetLatestClubActivities(string clubId)
        {
            string getUrl = string.Format("{0}/{1}/activities?access_token={2}", Endpoints.Club, clubId, Authentication.AccessToken);
            string json = WebRequest.SendGet(new Uri(getUrl));

            return Unmarshaller<List<ActivitySummary>>.Unmarshal(json);
        }

        /// <summary>
        /// Gets a list of the latest club activities.
        /// </summary>
        /// <param name="clubId">Id of the club you want to get the activities of.</param>
        /// <param name="page">The page of the activities list.</param>
        /// <param name="perPage">Specifies how many activities shpuld be shown per page.</param>
        /// <returns></returns>
        public List<ActivitySummary> GetLatestClubActivities(string clubId, int page, int perPage)
        {
            string getUrl = string.Format("{0}/{1}/activities?page={2}&per_page={3}&access_token={4}",
                Endpoints.Club,
                clubId,
                page,
                perPage,
                Authentication.AccessToken);
            string json = WebRequest.SendGet(new Uri(getUrl));

            return Unmarshaller<List<ActivitySummary>>.Unmarshal(json);
        }

        /// <summary>
        /// Gets a list of all the events of a specified club.
        /// </summary>
        /// <param name="clubId"></param>
        /// <returns></returns>
        public List<ClubEvent> GetClubEvents(string clubId)
        {
            string getUrl = string.Format("{0}/{1}/group_events?access_token={2}",
                Endpoints.Club,
                clubId,
                Authentication.AccessToken);
            string json = WebRequest.SendGet(new Uri(getUrl));

            return Unmarshaller<List<ClubEvent>>.Unmarshal(json);
        }

        #endregion
    }
}
