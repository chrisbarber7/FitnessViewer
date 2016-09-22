using FitnessViewer.Infrastructure.Models;
using System.Collections;
using System.Collections.Generic;

namespace FitnessViewer.Infrastructure.Interfaces
{
    public interface IRepository
    {
        void SaveChanges();

        // athlete
        void AddAthlete(StravaAthlete a);
        void EditAthlete(StravaAthlete a);
        void RemoveAthlete(int Id);
        IEnumerable GetAthletes();
        StravaAthlete FindAthleteById(long id);
        StravaAthlete FindAthleteByUserId(string userId);

        // queue
        void AddQueueItem(string userId);
        void AddQueueItem(string userId, long? activityId);
        IEnumerable GetQueue();
        IEnumerable FindQueueItemByUserId(string userId);
        void RemoveQueueItem(int id);

        // activity
        void AddActivity(StravaActivity a);
        void AddActivity(IEnumerable<StravaActivity> activities);
        StravaActivity GetActivity(long activityId);

        // Run Best Effort
        void AddBestEffort(StravaBestEffort e);

        // stream
        void AddSteam(IEnumerable<StravaStream> s);
    }

}
