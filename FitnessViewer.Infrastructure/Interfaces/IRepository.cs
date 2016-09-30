//using FitnessViewer.Infrastructure.Helpers;
//using FitnessViewer.Infrastructure.Models;
//using FitnessViewer.Infrastructure.Models.Dto;
//using System;
//using System.Collections;
//using System.Collections.Generic;

//namespace FitnessViewer.Infrastructure.Interfaces
//{
//    public interface IRepository
//    {
//        void SaveChanges();

//        // athlete
//        void AddAthlete(Athlete a);
//        void EditAthlete(Athlete a);
//        void RemoveAthlete(int Id);
//        IEnumerable GetAthletes();
//        Athlete FindAthleteById(long id);
//        Athlete FindAthleteByUserId(string userId);

//        // queue
//        void AddQueueItem(string userId);
//        void AddQueueItem(string userId, long? activityId);
//        IEnumerable GetQueue();
//        IEnumerable FindQueueItemByUserId(string userId);
//        void RemoveQueueItem(int id);
//        void QueueItemMarkHasError(int id);

//        // activity
//        void AddActivity(Activity a);
//        void AddActivity(IEnumerable<Activity> activities);
//        Activity GetActivity(long activityId);
//        IEnumerable<Activity> GetActivities(string userId);
//        IEnumerable<ActivityByPeriod> ActivityByWeek(string activityType, DateTime start, DateTime end);

//        // Run Best Effort
//        void AddBestEffort(BestEffort e);

//        // stream
//        void AddSteam(IEnumerable<Stream> s);

//        // peaks
//        void AddPeak(long activityId, PeakStreamType type, List<PeakDetail> peaks);
//        IEnumerable<AthletePeaks> GetPeaks(string userId, PeakStreamType type);
//        IEnumerable<RunningTimes> GetBestTimes(string userId);

//        // gear
//        void AddOrUpdateGear(Gear g);

//        // calendar
//        void AddCalendarDates(List<Calendar> dates);
//        IEnumerable<Calendar> GetCalendar();    
        
//        // measurements.
//        void AddMeasurement(Measurement m);
//    }

//}
