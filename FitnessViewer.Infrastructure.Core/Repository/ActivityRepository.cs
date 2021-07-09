﻿using FitnessViewer.Infrastructure.Core.Data;
using FitnessViewer.Infrastructure.Core.Models;
//using System.Data.Entity;
using System.Linq;
using System.Data.SqlClient;
using FitnessViewer.Infrastructure.Core.enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace FitnessViewer.Infrastructure.Core.Repository
{
    public class ActivityRepository
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

        public bool DeleteActivityDetails(long activityId)
        {
         //   throw new System.NotImplementedException();
            try
            {
                _context.Database.ExecuteSqlRaw("exec dbo.ActivityDeleteDetails @activityId", new SqlParameter("activityid", activityId));
            }
            catch (SqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }

            return true;
        }
        
 

        internal IQueryable<Activity> ActivitiesBySport(string userId, SportType sport)
        {
            // get activities which fall into the selected weeks.
            var activitiesQuery = _context.Activity
                .Include(r => r.ActivityType)
                .Include(r => r.Athlete)
                .Where(r => r.Athlete.UserId == userId);

            if (sport == SportType.Ride)
                return activitiesQuery.Where(r => r.ActivityType.IsRide);
            else if (sport == SportType.Run)
                return activitiesQuery.Where(r => r.ActivityType.IsRun);
            else if (sport == SportType.Swim)
                return activitiesQuery.Where(r => r.ActivityType.IsSwim);
            else if (sport == SportType.Other)
                return activitiesQuery.Where(r => r.ActivityType.IsOther);
            else
                return activitiesQuery;
        }
    }
}