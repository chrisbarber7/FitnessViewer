using FitnessViewer.Infrastructure.Core.Data;
using FitnessViewer.Infrastructure.Core.enums;
using FitnessViewer.Infrastructure.Core.Interfaces;
using FitnessViewer.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Core.Helpers
{
    public class ActivityWeight
    {
        private IUnitOfWork _unitOfWork;
        private long? _activityId;
        private string _userId;
        private List<Metric> _userWeights;

        // update weight details for all activity for a single user.
        public ActivityWeight(string userId) : this(userId, null)
        {
     
        }


        // update weight for a single activity
        public ActivityWeight(string userId, long? activityId) 
        {
            _unitOfWork = new UnitOfWork();
            _userId = userId;
            _userWeights = GetUserWeights();
            _activityId = activityId;
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

            var activities = GetActivityQuery().ToList();

            foreach (var a in activities)
            {
                decimal?  weightOnActivityDay = GetActivityWeight(a.Start);

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
            return GetActivityWeight(null);
        }
        public decimal? GetActivityWeight(DateTime? activityStartDate)
        {
            if (activityStartDate == null)
            {
                // if no activity start date provided then look it up from the activity.
                var activityDetails = GetActivityQuery().ToList();

                if ((activityDetails == null) || (activityDetails.Count() != 1))
                    return null;

                activityStartDate = activityDetails[0].Start.Date;
            }
          

            // get the weight recorded on the nearest date before the activity.
            return _userWeights
                        .Where(m => m.Recorded.Date <= activityStartDate)
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
            //return _unitOfWork.Metrics.GetMetrics(_userId, MetricType.Weight);

            return _unitOfWork.CRUDRepository.GetByUserId<Metric>(_userId)
                                             .Where(m => m.MetricType == MetricType.Weight)
                                             .ToList();
        }

    }
}
