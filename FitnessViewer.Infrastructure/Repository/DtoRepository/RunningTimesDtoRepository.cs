using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.Interfaces;
using FitnessViewer.Infrastructure.Models.Dto;
using System.Collections.Generic;
using System.Linq;

namespace FitnessViewer.Infrastructure.Repository
{
    public class RunningTimesDtoRepository : DtoRepository, IRunningTimesDtoRepository
    {
        public RunningTimesDtoRepository(ApplicationDb context) : base(context)
        {
        }

        public IEnumerable<RunningTimesDto> GetBestTimes(string userId)
        {
            // temp solution.  Plan is to have a user preferences table which will hold the users favourite distances which will
            // replace this hard coded list.
            List<decimal> favouriteDistances = new List<decimal>()
            {
                805.00M,
                1000.00M,
                1609.00M,
                5000.00M,
                10000.00M,
                21097.00M,
                42195.00M
            };

            // get a list of best times
            var times = from t in _context.BestEffort
                        join act in _context.Activity on t.ActivityId equals act.Id
                        join a in _context.Athlete on act.AthleteId equals a.Id
                        join fav in favouriteDistances on t.Distance equals fav
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
                          join e in _context.BestEffort on t.BestEffortId equals e.Id
                          join a in _context.Activity on e.ActivityId equals a.Id
                          orderby t.Time
                          select new RunningTimesDto
                          {
                              ActivityName = a.Name,
                              ActivityDate = a.StartDateLocal,
                              DistanceName = t.DistanceName,
                              Distance = e.Distance,
                              Time = t.Time,
                              ActivityId = e.ActivityId
                          };

            return results.ToList();
        }
    }
}
