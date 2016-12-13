using AutoMapper;
using StravaDotNetAthletes = Strava.Athletes;
using StravaDotNetGear = Strava.Gear;
using FitnessViewer.Infrastructure.Models;
using System;

namespace FitnessViewer.Infrastructure.Helpers
{
    public class StravaAthlete : Strava
    {
        public StravaAthlete() : base()
        {
        }

        private StravaDotNetAthletes.Athlete stravaAthleteDetails;
        private Athlete fitnessViewerAthlete;

        /// <summary>
        /// Constructor for creating a new athlete.
        /// </summary>
        /// <param name="stravaAthleteId">Strava athlete id</param>
        /// <param name="token">Strava access token</param>
        public StravaAthlete(long stravaAthleteId, string token)
        {
            _unitOfWork = new Data.UnitOfWork();
            this._stravaId = stravaAthleteId;
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
            base.SetupClient(token);
            stravaAthleteDetails = _client.Athletes.GetAthlete();

            fitnessViewerAthlete = Mapper.Map<Athlete>(stravaAthleteDetails);
            fitnessViewerAthlete.UserId = userId;
            fitnessViewerAthlete.Token = token;

            _unitOfWork.CRUDRepository.Add<Athlete>(fitnessViewerAthlete);

            UpdateUserConfig();

            // add user to the strava download queue for background downloading of activities.
            DownloadQueue.CreateQueueJob(userId, enums.DownloadType.Strava).Save();

            _unitOfWork.Complete();

        }


        private void UpdateUserConfig()
        {
            UpdateBikes();
            UpdateShoes();

            SystemSettings.CheckSettings(fitnessViewerAthlete.UserId, stravaAthleteDetails.Ftp);
        }

        /// <summary>
        /// Update strava athlete details
        /// </summary>
        /// <param name="token">Strava access token</param>
        public bool UpdateAthlete(string token)
        {
            try
            {
               stravaAthleteDetails = _client.Athletes.GetAthlete();

                 fitnessViewerAthlete = _unitOfWork.CRUDRepository.GetByKey<Athlete>(this._stravaId);

                if (fitnessViewerAthlete == null)
                    return false;

                LogActivity("Updating Athlete", fitnessViewerAthlete);

                // update athlete details from strava.
                Mapper.Map(stravaAthleteDetails, fitnessViewerAthlete);

                // get latest access token.
                fitnessViewerAthlete.Token = token;

                _unitOfWork.CRUDRepository.Update<Athlete>(fitnessViewerAthlete);

                UpdateUserConfig();

                _unitOfWork.Complete();

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }
        }

        private void UpdateBikes()
        {
            foreach (StravaDotNetGear.Bike b in stravaAthleteDetails.Bikes)
            {
                Gear g = Gear.CreateBike(b.Id, stravaAthleteDetails.Id);

                g.Brand = b.Brand;
                g.Description = b.Description;
                g.Distance = Convert.ToDecimal(b.Distance);
                switch (b.FrameType)
                {
                    case StravaDotNetGear.BikeType.Cross: { g.FrameType = enums.BikeType.Cross; break; }
                    case StravaDotNetGear.BikeType.Mountain: { g.FrameType = enums.BikeType.Mountain; break; }
                    case StravaDotNetGear.BikeType.Road: { g.FrameType = enums.BikeType.Road; break; }
                    case StravaDotNetGear.BikeType.Timetrial: { g.FrameType = enums.BikeType.Timetrial; break; }
                }
                g.IsPrimary = b.IsPrimary;
                g.Model = b.Model;
                g.Name = b.Name;
                g.ResourceState = b.ResourceState;

                _unitOfWork.CRUDRepository.AddOrUpdate<Gear>(g);
            }
        }


        private void UpdateShoes()
        {
            foreach (StravaDotNetGear.Shoes s in stravaAthleteDetails.Shoes)
            {
                Gear g = Gear.CreateShoe(s.Id, stravaAthleteDetails.Id);
                g.Distance = Convert.ToDecimal(s.Distance);
                g.FrameType = enums.BikeType.Default;
                g.IsPrimary = s.IsPrimary;
                g.Name = s.Name;
                g.ResourceState = s.ResourceState;
                _unitOfWork.CRUDRepository.AddOrUpdate<Gear>(g);
            }
        }
    }
}
