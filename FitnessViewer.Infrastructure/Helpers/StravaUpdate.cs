using FitnessViewer.Infrastructure.Data;
using System.Threading.Tasks;
using StravaDotNetActivities = Strava.Activities;

namespace FitnessViewer.Infrastructure.Helpers
{
    public class StravaUpdate : Strava
    {

        public StravaUpdate(UnitOfWork uow, string userId) : base(uow, userId)
        { }

        public async Task UpdateActivityNameAsync(long activityId, string newActivityName)
        {
           StravaDotNetActivities.Activity updatedActivity =  await _client.Activities.UpdateActivityAsync(activityId.ToString(), StravaDotNetActivities.ActivityParameter.Name, newActivityName);
           return ;
        }
    }
}
