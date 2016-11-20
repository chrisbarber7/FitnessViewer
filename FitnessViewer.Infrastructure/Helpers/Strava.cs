using System;
using FitnessViewer.Infrastructure.Repository;
using FitnessViewer.Infrastructure.Models;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;
using System.Linq;

// Strava.DotNet
using StravaDotNetAthletes = Strava.Athletes;
using StravaDotNetAuthentication = Strava.Authentication;
using StravaDotNetClient = Strava.Clients;
using StravaDotNetStreams = Strava.Streams;
using StravaDotNetActivities = Strava.Activities;
using StravaDotNetApi = Strava.Api;
using StravaDotNetGear = Strava.Gear;
using AutoMapper;
using FitnessViewer.Infrastructure.enums;
using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.Interfaces;

namespace FitnessViewer.Infrastructure.Helpers
{
    /// <summary>
    /// Used to get data from strava and add/update the information in the database
    /// </summary>
    public class Strava
    {
        protected IUnitOfWork _unitOfWork;
        protected StravaDotNetClient.StravaClient _client;
        protected string _userId;
        protected long _stravaId;
        protected int _stravaLimitDelay;
       
        public Strava()
        {
            _unitOfWork = new Data.UnitOfWork();
            StravaDotNetApi.Limits.UsageChanged += Limits_UsageChanged;
        }

        private void Limits_UsageChanged(object sender, StravaDotNetApi.UsageChangedEventArgs e)
        {
             // if getting close to short term limit introduce delays.
            if (e.Usage.ShortTerm < 550)
                _stravaLimitDelay = 100;
            else if (e.Usage.ShortTerm <=575)
                _stravaLimitDelay = 30000;
            else if (e.Usage.ShortTerm <= 590)
                _stravaLimitDelay = 45000;
            else
                _stravaLimitDelay = 60000;
        }
        
        /// <summary>
        /// Constructor for a given identity user id (token looked up)
        /// </summary>
        /// <param name="userId">Identity userid</param>
        public Strava(IUnitOfWork uow, string userId)
        {
            _unitOfWork = uow;
            _userId = userId;
            StravaDotNetApi.Limits.UsageChanged += Limits_UsageChanged;

            //   string token = _unitOfWork.Athlete.FindAthleteByUserId(userId).Token;

            var athlete = _unitOfWork.CRUDRepository.GetByUserId<Athlete>(userId).FirstOrDefault();

            if (athlete == null)
                throw new ArgumentException("Athlete Not Found.");

            string token = athlete.Token;

            if (string.IsNullOrEmpty(token))
                throw new ArgumentException("Invalid UserId");

            SetupClient(token);
        }

      

        /// <summary>
        /// Create Strava client used for downloading information
        /// </summary>
        /// <param name="token">Strava Access Token</param>
        protected void SetupClient(string token)
        {
            StravaDotNetAuthentication.StaticAuthentication auth = new StravaDotNetAuthentication.StaticAuthentication(token);
            _client = new StravaDotNetClient.StravaClient(auth);
        }

        protected void LogActivity(string action, Athlete fitnessViewerAthlete)
        {

            string log = string.Format("{0} : {1} ({2} {3}) ", action,
                                                               fitnessViewerAthlete.Id,
                                                               fitnessViewerAthlete.FirstName,
                                                               fitnessViewerAthlete.LastName);

            System.Diagnostics.Debug.WriteLine(log);
            Console.WriteLine(log);
        }

        protected void LogActivity(string action, Activity activity )
        {

            string log = string.Format("{0} : {1} - {2}", action,
                                                          activity.Id,
                                                          activity.Name);
                                                                                
            System.Diagnostics.Debug.WriteLine(log);
            Console.WriteLine(log);
        }

        protected void StravaPause(Athlete fvAthelete)
        {
            if (_stravaLimitDelay > 100)
                LogActivity(string.Format("Pausing for {0}ms", _stravaLimitDelay.ToString()), fvAthelete);

            System.Threading.Thread.Sleep(_stravaLimitDelay);
        }


        protected void StravaPause(Activity fvActivity)
        {
            if (_stravaLimitDelay > 100)
                LogActivity(string.Format("Pausing for {0}ms", _stravaLimitDelay.ToString()), fvActivity);

            System.Threading.Thread.Sleep(_stravaLimitDelay);
        }

        protected void AddNotification(Notification n)
        {
            _unitOfWork.Notification.Add(new UserNotification(_userId, n));
            _unitOfWork.Complete();
        }
    }
}