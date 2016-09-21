using Strava.Athletes;
using Strava.Authentication;
using Strava.Clients;
using System;
using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.Models;

namespace FitnessViewer.Infrastructure.Services
{
    /// <summary>
    /// Used to get data from strava and add/update the information in the database
    /// </summary>
    public class Strava
    {
        private Repository _repo;
        private StravaClient _client;
        private string _userId;
        private long _stravaId;

        public Strava()
        {
            _repo = new Repository();
        }
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="userId">Identity userid</param>
        public Strava(string userId)
        {
            _repo = new Repository();
            _userId = userId;
            string token = _repo.FindAthleteByUserId(userId).Token;

            if (string.IsNullOrEmpty(token))
                throw new ArgumentException("Invalid UserId");

            SetupClient(token);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="stravaId">Strava athlete id</param>
        /// <param name="token">Strava access token</param>
        public Strava(long stravaId, string token)
        {
            _repo = new Repository();
            this._stravaId = stravaId;
            SetupClient(token);
        }

        /// <summary>
        /// Add strava athlete details to data
        /// </summary>
        /// <param name="userId">Indentity userId</param>
        /// <param name="token">Strava access token</param>
        public void AddAthlete(string userId, string token)
        {
            _userId = userId;
            SetupClient(token);
            Athlete athlete = _client.Athletes.GetAthlete();
            InsertAthlete(athlete, userId, token);
        }

        /// <summary>
        /// Update strava athlete details
        /// </summary>
        /// <param name="token">Strava access token</param>
        public void UpdateAthlete(string token)
        {
            Athlete a = _client.Athletes.GetAthlete();
            UpdateAthelete(a, token);           
        }

        private void UpdateAthelete(Athlete athlete, string token)
        {
            StravaAthlete a = _repo.FindAthleteById(this._stravaId);

            if (a == null)
                return;

            a.Id = athlete.Id;            
            a.Token = token;
            UpdateEntityWithStravaDetails(athlete, a);
            _repo.EditAthlete(a);
        }

        private static void UpdateEntityWithStravaDetails(Athlete athlete, StravaAthlete a)
        {
            a.FirstName = athlete.FirstName;
            a.LastName = athlete.LastName;
            a.ProfileMedium = athlete.ProfileMedium;
            a.Profile = athlete.Profile;
            a.City = athlete.City;
            a.State = athlete.State;
            a.Country = athlete.Country;
            a.Sex = athlete.Sex;
            a.Friend = athlete.Friend;
            a.Follower = athlete.Follower;
            a.IsPremium = athlete.IsPremium;
            a.CreatedAt = athlete.CreatedAt;
            a.UpdatedAt = athlete.UpdatedAt;
            a.ApproveFollowers = athlete.ApproveFollowers;
            a.AthleteType = athlete.AthleteType;
            a.DatePreference = athlete.DatePreference;
            a.MeasurementPreference = athlete.MeasurementPreference;
            a.Email = athlete.Email;
            a.FTP = athlete.Ftp;
            a.Weight = athlete.Weight;
        }

        private void SetupClient(string token)
        {
            StaticAuthentication auth = new StaticAuthentication(token);
            _client = new StravaClient(auth);
        }

        private void InsertAthlete(Athlete athlete, string userId, string token)
        {
            var a = new StravaAthlete();
            a.Id = athlete.Id;
            a.UserId = userId;
            a.Token = token;
            UpdateEntityWithStravaDetails(athlete, a);
            _repo.AddAthlete(a);
        }


    }


}