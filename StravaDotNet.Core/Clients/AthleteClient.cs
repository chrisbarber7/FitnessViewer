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
using Strava.Athletes;
using Strava.Authentication;
using Strava.Common;
using Strava.Http;

namespace Strava.Clients
{
    /// <summary>
    /// Used to receive information about an athlete from Strava.
    /// </summary>
    public class AthleteClient : BaseClient
    {
        /// <summary>
        /// Initializes a new instance of the AthleteClient class.
        /// </summary>
        /// <param name="auth">The IAuthentication object containing a valid access token.</param>
        public AthleteClient(IAuthentication auth) : base(auth) { }

        #region Async

        /// <summary>
        /// Asynchronously receives the currently authenticated athlete.
        /// </summary>
        /// <returns>The currently authenticated athlete.</returns>
        public async Task<Athlete> GetAthleteAsync()
        {
            string getUrl = string.Format("{0}?access_token={1}", Endpoints.Athlete, Authentication.AccessToken);
            string json = await WebRequest.SendGetAsync(new Uri(getUrl));

            return Unmarshaller<Athlete>.Unmarshal(json);
        }

        /// <summary>
        /// Asynchronously receives the currently authenticated athlete.
        /// </summary>
        /// <param name="athleteId">The Strava Id of the athlete.</param>
        /// <returns>The AthleteSummary object of the athlete.</returns>
        public async Task<AthleteSummary> GetAthleteAsync(string athleteId)
        {
            string getUrl = string.Format("{0}/{1}?access_token={2}", Endpoints.Athletes, athleteId, Authentication.AccessToken);
            string json = await WebRequest.SendGetAsync(new Uri(getUrl));

            return Unmarshaller<AthleteSummary>.Unmarshal(json);
        }

        /// <summary>
        /// Gets the friends of the currently authenticated athlete.
        /// </summary>
        /// <returns>A list of the friends of the currently authenticated athlete.</returns>
        public async Task<List<AthleteSummary>> GetFriendsAsync()
        {
            string getUrl = string.Format("{0}/friends?access_token={1}", Endpoints.Athlete, Authentication.AccessToken);
            string json = await WebRequest.SendGetAsync(new Uri(getUrl));

            return Unmarshaller<List<AthleteSummary>>.Unmarshal(json);
        }

        /// <summary>
        /// Gets a list of friends of an athlete.
        /// </summary>
        /// <param name="athleteId">The Strava athlete id.</param>
        /// <returns>The list of friends of the athlete.</returns>
        public async Task<List<AthleteSummary>> GetFriendsAsync(string athleteId)
        {
            string getUrl = string.Format("{0}/friends?access_token={1}", Endpoints.Athlete, Authentication.AccessToken);
            string json = await WebRequest.SendGetAsync(new Uri(getUrl));

            return Unmarshaller<List<AthleteSummary>>.Unmarshal(json);
        }

        /// <summary>
        /// Gets all the followers of the currently authenticated athlete.
        /// </summary>
        /// <returns>A list of athletes that follow the currently authenticated athlete.</returns>
        public async Task<List<AthleteSummary>> GetFollowersAsync()
        {
            string getUrl = string.Format("{0}?access_token={1}", Endpoints.Follower, Authentication.AccessToken);
            string json = await WebRequest.SendGetAsync(new Uri(getUrl));

            return Unmarshaller<List<AthleteSummary>>.Unmarshal(json);
        }

        /// <summary>
        /// Get a list of athletes that follow an athlete.
        /// </summary>
        /// <param name="athleteId">The Strava athlete id.</param>
        /// <returns>A list of athletes that follow the specified athlete.</returns>
        public async Task<List<AthleteSummary>> GetFollowersAsync(string athleteId)
        {
            string getUrl = string.Format("{0}/{1}/followers?access_token={2}", Endpoints.Followers, athleteId, Authentication.AccessToken);
            string json = await WebRequest.SendGetAsync(new Uri(getUrl));

            return Unmarshaller<List<AthleteSummary>>.Unmarshal(json);
        }

        /// <summary>
        /// Get a list of athletes that both you and the specified athlete are following.
        /// </summary>
        /// <param name="athleteId">The Strava athlete id.</param>
        /// <returns>A list of athletes that both you and the specified athlete are following.</returns>
        public async Task<List<AthleteSummary>> GetBothFollowingAsync(string athleteId)
        {
            string getUrl = string.Format("{0}/{1}/both-following?access_token={2}", Endpoints.Followers, athleteId, Authentication.AccessToken);
            string json = await WebRequest.SendGetAsync(new Uri(getUrl));

            return Unmarshaller<List<AthleteSummary>>.Unmarshal(json);
        }

        /// <summary>
        /// Updates the specified parameter of an athlete.
        /// </summary>
        /// <param name="parameter">The parameter that is being updated.</param>
        /// <param name="value">The value to update to.</param>
        /// <returns>Athlete object of the currently authenticated athlete with the updated parameter.</returns>
        public async Task<Athlete> UpdateAthleteAsync(AthleteParameter parameter, string value)
        {
            string putUrl = string.Empty;

            switch (parameter)
            {
                case AthleteParameter.City:
                    putUrl = string.Format("{0}?city={1}&access_token={2}", Endpoints.Athlete, value, Authentication.AccessToken);
                    break;
                case AthleteParameter.Country:
                    putUrl = string.Format("{0}?country={1}&access_token={2}", Endpoints.Athlete, value, Authentication.AccessToken);
                    break;
                case AthleteParameter.State:
                    putUrl = string.Format("{0}?state={1}&access_token={2}", Endpoints.Athlete, value, Authentication.AccessToken);
                    break;
                case AthleteParameter.Weight:
                    putUrl = string.Format("{0}?weight={1}&access_token={2}", Endpoints.Athlete, value, Authentication.AccessToken);
                    break;
            }

            string json = await WebRequest.SendPutAsync(new Uri(putUrl));

            return Unmarshaller<Athlete>.Unmarshal(json);
        }

        /// <summary>
        /// Updates the sex of the currently authenticated athlete.
        /// </summary>
        /// <param name="gender">The gender to update to.</param>
        /// <returns>The currently authenticated athlete.</returns>
        public async Task<Athlete> UpdateAthleteSex(Gender gender)
        {
            string putUrl = string.Format("{0}?sex={1}&access_token={2}", Endpoints.Athlete, gender.ToString().Substring(0, 1), Authentication.AccessToken);
            string json = await WebRequest.SendPutAsync(new Uri(putUrl));

            return Unmarshaller<Athlete>.Unmarshal(json);
        }

        #endregion

        #region Sync

        /// <summary>
        /// Receives the currently authenticated athlete.
        /// </summary>
        /// <returns>The currently authenticated athlete.</returns>
        public Athlete GetAthlete()
        {
            string getUrl = string.Format("{0}?access_token={1}", Endpoints.Athlete, Authentication.AccessToken);
            string json = WebRequest.SendGet(new Uri(getUrl));

            return Unmarshaller<Athlete>.Unmarshal(json);
        }

        /// <summary>
        /// Receives a Strava athlete.
        /// </summary>
        /// <param name="athleteId">The Strava Id of the athlete.</param>
        /// <returns>The AthleteSummary object of the athlete.</returns>
        public AthleteSummary GetAthlete(string athleteId)
        {
            string getUrl = string.Format("{0}/{1}?access_token={2}", Endpoints.Athletes, athleteId, Authentication.AccessToken);
            string json = WebRequest.SendGet(new Uri(getUrl));

            return Unmarshaller<AthleteSummary>.Unmarshal(json);
        }

        /// <summary>
        /// Gets the friends of the currently authenticated athlete.
        /// </summary>
        /// <returns>A list of the friends of the currently authenticated athlete.</returns>
        public List<AthleteSummary> GetFriends()
        {
            string getUrl = string.Format("{0}/friends?access_token={1}", Endpoints.Athlete, Authentication.AccessToken);
            string json = WebRequest.SendGet(new Uri(getUrl));

            return Unmarshaller<List<AthleteSummary>>.Unmarshal(json);
        }

        /// <summary>
        /// Gets a list of friends of an athlete.
        /// </summary>
        /// <param name="athleteId">The Strava athlete id.</param>
        /// <returns>The list of friends of the athlete.</returns>
        public List<AthleteSummary> GetFriends(string athleteId)
        {
            string getUrl = string.Format("{0}/friends?access_token={1}", Endpoints.Athlete, Authentication.AccessToken);
            string json = WebRequest.SendGet(new Uri(getUrl));

            return Unmarshaller<List<AthleteSummary>>.Unmarshal(json);
        }

        /// <summary>
        /// Gets all the followers of the currently authenticated athlete.
        /// </summary>
        /// <returns>A list of athletes that follow the currently authenticated athlete.</returns>
        public List<AthleteSummary> GetFollowers()
        {
            string getUrl = string.Format("{0}?access_token={1}", Endpoints.Follower, Authentication.AccessToken);
            string json = WebRequest.SendGet(new Uri(getUrl));

            return Unmarshaller<List<AthleteSummary>>.Unmarshal(json);
        }

        /// <summary>
        /// Get a list of athletes that follow an athlete.
        /// </summary>
        /// <param name="athleteId">The Strava athlete id.</param>
        /// <returns>A list of athletes that follow the specified athlete.</returns>
        public List<AthleteSummary> GetFollowers(string athleteId)
        {
            string getUrl = string.Format("{0}/{1}/followers?access_token={2}", Endpoints.Followers, athleteId, Authentication.AccessToken);
            string json = WebRequest.SendGet(new Uri(getUrl));

            return Unmarshaller<List<AthleteSummary>>.Unmarshal(json);
        }

        /// <summary>
        /// Get a list of athletes that both you and the specified athlete are following.
        /// </summary>
        /// <param name="athleteId">The Strava athlete id.</param>
        /// <returns>A list of athletes that both you and the specified athlete are following.</returns>
        public List<AthleteSummary> GetBothFollowing(string athleteId)
        {
            string getUrl = string.Format("{0}/{1}/both-following?access_token={2}", Endpoints.Followers, athleteId, Authentication.AccessToken);
            string json = WebRequest.SendGet(new Uri(getUrl));

            return Unmarshaller<List<AthleteSummary>>.Unmarshal(json);
        }

        /// <summary>
        /// Updates the specified parameter of an athlete.
        /// </summary>
        /// <param name="parameter">The parameter that is being updated.</param>
        /// <param name="value">The value to update to.</param>
        /// <returns>Athlete object of the currently authenticated athlete with the updated parameter.</returns>
        public Athlete UpdateAthlete(AthleteParameter parameter, string value)
        {
            string putUrl = string.Empty;

            switch (parameter)
            {
                case AthleteParameter.City:
                    putUrl = string.Format("{0}?city={1}&access_token={2}", Endpoints.Athlete, value, Authentication.AccessToken);
                    break;
                case AthleteParameter.Country:
                    putUrl = string.Format("{0}?country={1}&access_token={2}", Endpoints.Athlete, value, Authentication.AccessToken);
                    break;
                case AthleteParameter.State:
                    putUrl = string.Format("{0}?state={1}&access_token={2}", Endpoints.Athlete, value, Authentication.AccessToken);
                    break;
                case AthleteParameter.Weight:
                    putUrl = string.Format("{0}?weight={1}&access_token={2}", Endpoints.Athlete, value, Authentication.AccessToken);
                    break;
            }

            string json = WebRequest.SendPut(new Uri(putUrl));

            return Unmarshaller<Athlete>.Unmarshal(json);
        }

        #endregion
    }
}
