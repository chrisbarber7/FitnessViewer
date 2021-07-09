using FitnessViewer.Infrastructure.Core.Interfaces;
using Strava.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Core.Helpers
{
    /// <summary>
    /// Get a list of related activities
    /// </summary>
    public class StravaRelatatedActivity : Strava
    {
        public StravaRelatatedActivity(IUnitOfWork uow, string userId) : base(uow, userId)
        {
        }

        /// <summary>
        /// Related Activities
        /// </summary>
        /// <param name="activityId">Activity Id for which to find related activities</param>
        /// <returns></returns>
        public async Task<List<long>> GetRelatedActivityAsync( long activityId)
        {
            List<long> activities = await _client.Activities.GetReleatedActivityAsync(activityId.ToString());
            return activities;
        }
    }
}
