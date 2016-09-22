using FitnessViewer.Infrastructure.Models;
using FitnessViewer.Infrastructure.Interfaces;
using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace FitnessViewer.Infrastructure.Data
{
    public class Repository : IRepository
    {
        ApplicationDb context;

        public Repository()
        {
            context = new ApplicationDb();
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        #region Athlete
        public void AddAthlete(StravaAthlete a)
        {
            context.StravaAthlete.Add(a);
            context.SaveChanges();
        }
    
        public void EditAthlete(StravaAthlete a)
        {
            context.SaveChanges();
        }

        public StravaAthlete FindAthleteById(long id)
        {
            var result = (from r in context.StravaAthlete where r.Id == id select r).FirstOrDefault();
            return result;
        }

        public StravaAthlete FindAthleteByUserId(string userId)
        {
            var result = (from r in context.StravaAthlete where r.UserId == userId select r).FirstOrDefault();
            return result;
        }

        public IEnumerable GetAthletes()
        {
            throw new NotImplementedException();
        }

        public void RemoveAthlete(int Id)
        {
            throw new NotImplementedException();
        }
        #endregion


        #region Queue
            public void AddQueueItem(string userId)

        {
            this.AddQueueItem(userId, null);
        }

        public void AddQueueItem(string userId, long? activityId)
        {
            StravaQueue q = new StravaQueue() { UserId = userId, Added = DateTime.Now, Processed = false, StravaActivityId=activityId };
            context.Queue.Add(q);
            context.SaveChanges();
        }

        public IEnumerable GetQueue()
        {
            return context.Queue.Where(x => !x.Processed).ToList();
        }


        public void RemoveQueueItem(int id)
        {
            StravaQueue q = context.Queue.Find(id);
            q.Processed = true;
            q.ProcessedAt = DateTime.Now;
            context.SaveChanges();

        }

        public IEnumerable FindQueueItemByUserId(string userId)
        {
            return context.Queue.Where(x => x.UserId == userId).ToList();
        }
        #endregion

        #region activity
        public void AddActivity(StravaActivity a)
        {
            context.StravaActivity.Add(a);
        
        }

        public void AddActivity(IEnumerable<StravaActivity> activities)
        {
            context.StravaActivity.AddRange(activities);
        }

        public StravaActivity GetActivity(long activityId)
        {
            return context.StravaActivity.Where(a => a.Id == activityId).FirstOrDefault();
        }
        #endregion

        #region Best Effort
        public void AddBestEffort(StravaBestEffort e)
        {
            context.StravaBestEffort.Add(e);
        }
        #endregion

        #region streams
        public void AddSteam(IEnumerable<StravaStream> s)
        {
            context.StravaStream.AddRange(s);
            context.SaveChanges();
                    }
        #endregion

    }
}
