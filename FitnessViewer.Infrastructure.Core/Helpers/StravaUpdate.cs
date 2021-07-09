using FitnessViewer.Infrastructure.Core.Data;
using System;
using System.Threading.Tasks;
using FitnessViewer.Infrastructure.Core.Models.ViewModels;
using StravaDotNetActivities = Strava.Activities;
using FitnessViewer.Infrastructure.Core.Models;
using FitnessViewer.Infrastructure.Core.Interfaces;

namespace FitnessViewer.Infrastructure.Core.Helpers
{
    public class StravaUpdate : Strava
    {

        public StravaUpdate(IUnitOfWork uow, string userId) : base(uow, userId)
        { }

        public async Task UpdateActivityAsync(string field, long activityId, string amendedValue)
        {
            StravaDotNetActivities.ActivityParameter updateFieldType;

            switch (field.ToLower())
            {
                case "commute": { updateFieldType = StravaDotNetActivities.ActivityParameter.Commute; break; }
                case "description": { updateFieldType = StravaDotNetActivities.ActivityParameter.Description; break; }
                case "gearid": { updateFieldType = StravaDotNetActivities.ActivityParameter.GearId; break; }
                case "name": { updateFieldType = StravaDotNetActivities.ActivityParameter.Name; break; }
                case "private": { updateFieldType = StravaDotNetActivities.ActivityParameter.Private; break; }
                case "trainer": { updateFieldType = StravaDotNetActivities.ActivityParameter.Trainer; break; }
                default: throw new ArgumentException("Invalid field type");
            }

            StravaDotNetActivities.Activity updatedActivity = await _client.Activities.UpdateActivityAsync(activityId.ToString(), updateFieldType, amendedValue);
            return;
        }

        public async Task  ActivityDetailsUpdate(Activity fvActivity, EditActivityViewModel amendedDetails)
        {
            if (fvActivity.Name != amendedDetails.Name)
                await UpdateActivityAsync("name", amendedDetails.Id, amendedDetails.Name);

            if (fvActivity.IsPrivate != amendedDetails.IsPrivate)
                await UpdateActivityAsync("private", amendedDetails.Id, amendedDetails.IsPrivate ? "true" : "false");

            if (fvActivity.Description != amendedDetails.Description)
                await UpdateActivityAsync("description", amendedDetails.Id, amendedDetails.Description);

            if (fvActivity.IsCommute != amendedDetails.IsCommute)
                await UpdateActivityAsync("commute", amendedDetails.Id, amendedDetails.IsCommute ? "true" : "false");

            fvActivity.Name = amendedDetails.Name;
            fvActivity.IsPrivate = amendedDetails.IsPrivate;
            fvActivity.IsCommute = amendedDetails.IsCommute;
            fvActivity.Description = amendedDetails.Description;

            //        _unitOfWork.Activity.UpdateActivity(fvActivity);
            _unitOfWork.CRUDRepository.Update<Activity>(fvActivity);
            _unitOfWork.Complete();

        }
    }
}
