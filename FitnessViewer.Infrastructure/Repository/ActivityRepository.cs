using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.Models;
using System.Data.Entity;
using System.Linq;
using System.Data.SqlClient;

namespace FitnessViewer.Infrastructure.Repository
{
    internal class ActivityRepository
    {
        private ApplicationDb _context;


        internal ActivityRepository(ApplicationDb context)
        {
            _context = context;
        }

        
        internal IQueryable<Activity> GetActivities(string userId)
        {
            return _context.Activity
                  .Where(a => a.Athlete.UserId == userId)
                  .Include(a => a.ActivityType);

        }

        internal bool DeleteActivityDetails(long activityId)
        {
            try
            {
                _context.Database.ExecuteSqlCommand("dbo.ActivityDeleteDetails @activityId", new SqlParameter("activityid", activityId));
            }
            catch (SqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }

            return true;
        }
        
 

        internal IQueryable<Activity> ActivitiesBySport(string userId, string sport)
        {
            // get activities which fall into the selected weeks.
            var activitiesQuery = _context.Activity
                .Include(r => r.ActivityType)
                .Include(r => r.Athlete)
                .Where(r => r.Athlete.UserId == userId);

            if (sport == "Ride")
                return activitiesQuery.Where(r => r.ActivityType.IsRide);
            else if (sport == "Run")
                return activitiesQuery.Where(r => r.ActivityType.IsRun);
            else if (sport == "Swim")
                return activitiesQuery.Where(r => r.ActivityType.IsSwim);
            else if (sport == "Other")
                return activitiesQuery.Where(r => r.ActivityType.IsOther);
            else
                return activitiesQuery;
        }
    }
}