using AutoMapper;
using StravaDotNetAthletes = Strava.Athletes;
using StravaDotNetGear = Strava.Gear;
using System.Collections.Generic;
using FitnessViewer.Infrastructure.Models;
using System;

namespace FitnessViewer.Infrastructure.Helpers
{
    public class StravaAthlete : Strava
    {
        public StravaAthlete() : base()
        {
        }


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
            StravaDotNetAthletes.Athlete athlete = _client.Athletes.GetAthlete();

            Athlete a = Mapper.Map<Athlete>(athlete);
            a.UserId = userId;
            a.Token = token;

            //     _unitOfWork.Athlete.AddAthlete(a);
            _unitOfWork.CRUDRepository.Add<Athlete>(a);

            UpdateBikes(a.Id, athlete.Bikes);
            UpdateShoes(a.Id, athlete.Shoes);

            // add user to the strava download queue for background downloading of activities.
            DownloadQueue.CreateQueueJob(userId, enums.DownloadType.Strava).Save();

            _unitOfWork.Complete();

        }

        /// <summary>
        /// Update strava athlete details
        /// </summary>
        /// <param name="token">Strava access token</param>
        public void UpdateAthlete(string token)
        {
            try
            {
                StravaDotNetAthletes.Athlete stravaAthleteDetails = _client.Athletes.GetAthlete();

                Athlete fitnessViewerAthlete = _unitOfWork.CRUDRepository.GetByKey<Athlete>(this._stravaId);

                if (fitnessViewerAthlete == null)
                    return;

                LogActivity("Updating Athlete", fitnessViewerAthlete);

                // update athlete details from strava.
                Mapper.Map(stravaAthleteDetails, fitnessViewerAthlete);

                // get latest access token.
                fitnessViewerAthlete.Token = token;

                _unitOfWork.CRUDRepository.Update<Athlete>(fitnessViewerAthlete);

                // update bike and shoe details.
                UpdateBikes(stravaAthleteDetails.Id, stravaAthleteDetails.Bikes);
                UpdateShoes(stravaAthleteDetails.Id, stravaAthleteDetails.Shoes);

                _unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);

            }
        }

        private void UpdateBikes(long athleteId, List<StravaDotNetGear.Bike> bikes)
        {
            foreach (StravaDotNetGear.Bike b in bikes)
            {
                Gear g = Gear.CreateBike(b.Id, athleteId);

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


        private void UpdateShoes(long athleteId, List<StravaDotNetGear.Shoes> shoes)
        {
            foreach (StravaDotNetGear.Shoes s in shoes)
            {
                Gear g = Gear.CreateShoe(s.Id, athleteId);
                g.Distance = Convert.ToDecimal(s.Distance);
                g.FrameType = enums.BikeType.Default;
                g.IsPrimary = s.IsPrimary;
                g.Name = s.Name;
                g.ResourceState = s.ResourceState;
                _unitOfWork.CRUDRepository.AddOrUpdate<Gear>(g);
                //  _unitOfWork.Activity.AddOrUpdateGear(g);
            }
        }

    }
}
