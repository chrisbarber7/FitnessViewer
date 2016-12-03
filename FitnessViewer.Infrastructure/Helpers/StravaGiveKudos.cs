using FitnessViewer.Infrastructure.Interfaces;
using System.Collections.Generic;

namespace FitnessViewer.Infrastructure.Helpers
{
    public class StravaGiveKudos : Strava
    {
        /// <summary>
        /// Give Kudos to a single or list of activities.
        /// </summary>
        /// <param name="uow">Unit Of Work</param>
        /// <param name="userId">ASP.NET Identity Id</param>
        public StravaGiveKudos(IUnitOfWork uow, string userId) : base(uow, userId)
        {   
        }

        /// <summary>
        /// Give Kudos for a single activity.
        /// </summary>
        /// <param name="activityId">Activity Id</param>
        public void GiveKudos(long activityId)
        {
            _client.Activities.GiveKudos(activityId.ToString());
        }

        /// <summary>
        /// Give Kudos to a list of activities
        /// </summary>
        /// <param name="activityIds">List of activities to kudos</param>
        public void GiveKudos(List<long> activityIds)
        {
            foreach (long id in activityIds)
                GiveKudos(id);
        }
    }
}
