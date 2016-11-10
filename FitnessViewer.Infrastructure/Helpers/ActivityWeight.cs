using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.enums;
using FitnessViewer.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Helpers
{
    public class ActivityWeight
    {
        private UnitOfWork _unitOfWork;
        private long? _activityId;
        private string _userId;

        // update weight for a single activity
        public ActivityWeight(string userId, long activityId)
        {
            _unitOfWork = new UnitOfWork();
            _userId = userId;
            _activityId = activityId;
        }

        // update weight details for all activity for a single user.
        public ActivityWeight(string userId)
        {
            _unitOfWork = new UnitOfWork();
            _userId = userId;
        }

        /// <summary>
        /// Update requested activity weight details.
        /// </summary>
        public void UpdateActivityWeight()
        {
            List<Metric> userWeight = GetUserWeights();

            if (userWeight == null || userWeight.Count() == 0)
                return;

            //// get a list of activities with the weight details currently recorded against each.
            //var userActivities = _unitOfWork.Activity.GetActivities(_userId).Select(a => new { a.Id, a.Start, a.Weight });
            
            //// if a single activity was requested then limit to just that activity.
            //if (_activityId != null)
            //    userActivities = userActivities.Where(a => a.Id == _activityId);

            foreach (var a in GetActivityQuery().ToList())
            {
                decimal?  weightOnActivityDay = GetActivityWeight();

                // if a weight found and it's different to the weight on the activity then update it.
                if ((weightOnActivityDay != null) && (weightOnActivityDay != 0.00M) && (a.Weight != weightOnActivityDay))
                {
                    _unitOfWork.Metrics.UpdateActivityWeight(a.Id, _userId, weightOnActivityDay.Value);
                    _unitOfWork.Complete();
                }
            }
        }

        public IQueryable<dynamic> GetActivityQuery()
        {
            // get a list of activities with the weight details currently recorded against each.
            var userActivities = _unitOfWork.Activity.GetActivities(_userId)
                .Where(a=>_activityId == null ? true : a.Id == _activityId)
                .Select(a => new { a.Id, a.Start, a.Weight });

            //// if a single activity was requested then limit to just that activity.
            //if (_activityId != null)
            //    userActivities = userActivities.Where(a => );

            return userActivities;
        }

        public decimal? GetActivityWeight()
        {
            var activityDetails = GetActivityQuery().ToList();

            if ((activityDetails == null) || (activityDetails.Count() !=1))
                    return null;

            DateTime activityStart = activityDetails[0].Start.Date;

            // get the weight recorded on the nearest date before the activity.
            return GetUserWeights()
                        .Where(m => m.Recorded.Date <= activityStart)
                        .OrderByDescending(m => m.Recorded)
                        .Select(m => m.Value)
                        .FirstOrDefault();
        }

        /// <summary>
        /// Get a complete list of user weight records.
        /// </summary>
        /// <returns></returns>
        private List<Metric> GetUserWeights()
        {   
            return _unitOfWork.Metrics.GetMetrics(_userId, MetricType.Weight);
        }

    }
}
