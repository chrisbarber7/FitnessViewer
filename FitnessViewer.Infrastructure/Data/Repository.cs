using FitnessViewer.Infrastructure.Models;
using FitnessViewer.Infrastructure.Interfaces;
using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using FitnessViewer.Infrastructure.Helpers;
using System.Data.Entity;
using FitnessViewer.Infrastructure.Models.Dto;
using System.Data.Entity.Migrations;

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
            try
            {
                context.SaveChanges();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        #region Athlete
        public void AddAthlete(Athlete a)
        {
            context.Athlete.Add(a);
            context.SaveChanges();
        }

        public void EditAthlete(Athlete a)
        {
            context.SaveChanges();
        }

        public Athlete FindAthleteById(long id)
        {
            var result = (from r in context.Athlete where r.Id == id select r).FirstOrDefault();
            return result;
        }

        public Athlete FindAthleteByUserId(string userId)
        {
            var result = (from r in context.Athlete where r.UserId == userId select r).FirstOrDefault();
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
            DownloadQueue q = new DownloadQueue() { UserId = userId, Added = DateTime.Now, Processed = false, ActivityId = activityId };
            context.Queue.Add(q);
            context.SaveChanges();
        }

        public IEnumerable GetQueue()
        {
            return context.Queue.Where(x => !x.Processed).ToList();
        }


        public void RemoveQueueItem(int id)
        {
            DownloadQueue q = context.Queue.Find(id);
            q.Processed = true;
            q.ProcessedAt = DateTime.Now;
            q.HasError = false;
            context.SaveChanges();

        }

        public void QueueItemMarkHasError(int id)
        {
            DownloadQueue q = context.Queue.Find(id);
            q.HasError = true;
            context.SaveChanges();

        }

        public IEnumerable FindQueueItemByUserId(string userId)
        {
            return context.Queue.Where(x => x.UserId == userId).ToList();
        }
        #endregion

        #region activity
        public void AddActivity(Activity a)
        {
            context.Activity.Add(a);

        }

        public void AddActivity(IEnumerable<Activity> activities)
        {
            context.Activity.AddRange(activities);
        }

        public Activity GetActivity(long activityId)
        {
            return context.Activity.Where(a => a.Id == activityId)
                .Include(a => a.ActivityType)
                .FirstOrDefault();
        }

        public IEnumerable<Activity> GetActivities(string userId)
        {
            return context.Activity
                  .Where(a => a.Athlete.UserId == userId)
                  .Include(a => a.ActivityType)
                  .ToList();

        }
        #endregion

        #region Best Effort
        public void AddBestEffort(BestEffort e)
        {
            context.BestEffort.Add(e);
        }
        #endregion

        #region streams
        public void AddSteam(IEnumerable<Stream> s)
        {
            context.Stream.AddRange(s);
            context.SaveChanges();
        }

        #endregion

        #region peaks

        public void AddPeak(long activityId, PeakStreamType type, List<PeakDetail> peaks)
        {
            ActivityPeaks stravaPeak = new ActivityPeaks() { ActivityId = activityId, PeakType = (byte)type };

            foreach (PeakDetail d in peaks)
            {
                switch (d.Duration)
                {
                    case 5: { stravaPeak.Peak5 = d.Value; break; }
                    case 10: { stravaPeak.Peak10 = d.Value; break; }
                    case 30: { stravaPeak.Peak30 = d.Value; break; }
                    case 60: { stravaPeak.Peak60 = d.Value; break; }
                    case 120: { stravaPeak.Peak120 = d.Value; break; }
                    case 300: { stravaPeak.Peak300 = d.Value; break; }
                    case 360: { stravaPeak.Peak360 = d.Value; break; }
                    case 600: { stravaPeak.Peak600 = d.Value; break; }
                    case 720: { stravaPeak.Peak720 = d.Value; break; }
                    case 1200: { stravaPeak.Peak1200 = d.Value; break; }
                    case 1800: { stravaPeak.Peak1800 = d.Value; break; }
                    case 3600: { stravaPeak.Peak3600 = d.Value; break; }
                    case int.MaxValue: { stravaPeak.PeakDuration = d.Value; break; }
                }
            }

            context.ActivityPeak.Add(stravaPeak);
            context.SaveChanges();

        }
        /// <summary>
        /// Return Peak information for common time duration
        /// </summary>
        /// <param name="userId">Indentity</param>
        /// <param name="type">Stream Type to analyse</param>
        /// <returns></returns>
        public IEnumerable<AthletePeaks> GetPeaks(string userId, PeakStreamType type)
        {
            var peaks = context.ActivityPeak
                  .Where(p => p.Activity.Athlete.UserId == userId && p.PeakType == (byte)type)
                  .Include(p => p.Activity);

            List<AthletePeaks> ap = new List<AthletePeaks>();
            ap.Add(ExtractPeaksByDays(type, peaks, 7));
            ap.Add(ExtractPeaksByDays(type, peaks, 30));
            ap.Add(ExtractPeaksByDays(type, peaks, 90));
            ap.Add(ExtractPeaksByDays(type, peaks, 365));
            ap.Add(ExtractPeaksByDays(type, peaks, int.MaxValue));
            return ap;
        }

        private static AthletePeaks ExtractPeaksByDays(PeakStreamType type, IQueryable<ActivityPeaks> peaks, int days)
        {
            // days=int.maxvalue is used for earlist date
            DateTime earliestDate = days == int.MaxValue ? DateTime.MinValue : DateTime.Now.AddDays(days * -1);

            AthletePeaks ap = new AthletePeaks();
            ap.PeakType = type;
            ap.Days = days;

            ap.Seconds5 = peaks.Where(p => p.Activity.StartDateLocal >= earliestDate)
                                .OrderByDescending(p => p.Peak5)
                                .Select(p => new AthletePeaks.AthletePeaksDetails() { Peak = p.Peak5, ActivityId = p.ActivityId, Description = p.Activity.Name })
                                .FirstOrDefault();

            ap.Minute1 = peaks.Where(p => p.Activity.StartDateLocal >= earliestDate)
                                .OrderByDescending(p => p.Peak60)
                                .Select(p => new AthletePeaks.AthletePeaksDetails() { Peak = p.Peak60, ActivityId = p.ActivityId, Description = p.Activity.Name })
                                .FirstOrDefault();

            ap.Minute5 = peaks.Where(p => p.Activity.StartDateLocal >= earliestDate)
                                .OrderByDescending(p => p.Peak300)
                                .Select(p => new AthletePeaks.AthletePeaksDetails() { Peak = p.Peak300, ActivityId = p.ActivityId, Description = p.Activity.Name })
                                .FirstOrDefault();

            ap.Minute20 = peaks.Where(p => p.Activity.StartDateLocal >= earliestDate)
                                .OrderByDescending(p => p.Peak1200)
                                .Select(p => new AthletePeaks.AthletePeaksDetails() { Peak = p.Peak1200, ActivityId = p.ActivityId, Description = p.Activity.Name })
                                .FirstOrDefault();

            ap.Minute60 = peaks.Where(p => p.Activity.StartDateLocal >= earliestDate)
                                .OrderByDescending(p => p.Peak3600)
                                .Select(p => new AthletePeaks.AthletePeaksDetails() { Peak = p.Peak3600, ActivityId = p.ActivityId, Description = p.Activity.Name })
                                .FirstOrDefault();

            return ap;
        }

        #endregion

        public IEnumerable<RunningTimes> GetBestTimes(string userId)
        {
            // get a list of best times
            var times = from t in context.BestEffort
                        join act in context.Activity on t.ActivityId equals act.Id
                        join a in context.Athlete on act.AthleteId equals a.Id
                        where a.UserId == userId
                        group t by t.Name into dptgrp
                        let fastestTime = dptgrp.Min(x => x.ElapsedTime)
                        select new
                        {
                            DistanceName = dptgrp.Key,
                            BestEffortId = dptgrp.FirstOrDefault(y => y.ElapsedTime == fastestTime).Id,
                            Time = fastestTime

                        };
         
            // join to other table to get full info.
            var results = from t in times
                          join e in context.BestEffort on t.BestEffortId equals e.Id
                          join a in context.Activity on e.ActivityId equals a.Id
                          orderby t.Time
                          select new RunningTimes
                          {
                              ActivityName = a.Name,
                              ActivityDate = a.StartDateLocal,
                              DistanceName = t.DistanceName,
                              Distance = e.Distance,
                              Time = t.Time,
                              ActivityId =e.ActivityId
                          };

            return results.ToList();
        }

        public void AddOrUpdateGear(Gear g)
        {
            context.Gear.AddOrUpdate(g);
            context.SaveChanges();
        }

        public void AddCalendarDates(List<Calendar> dates)
        {
            context.Calendar.AddRange(dates);
        }

        public IEnumerable<Calendar> GetCalendar()
        {
            return context.Calendar ;
        }
    }
}
