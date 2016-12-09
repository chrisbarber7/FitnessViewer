using FitnessViewer.Infrastructure.Intefaces;
using FitnessViewer.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FitnessViewer.Infrastructure.Helpers

{
    /// <summary>
    /// Yearly details for each sport.  YTD information plus sequences (number of consequetive days).
    /// </summary>
    public class YearlyDetails
    {
        private IActivityDtoRepository _repo;
        private List<YearlyDetailsDayInfo> _details;
        int _startYear;

        
        public YearlyDetails()
        {
            _repo = new ActivityDtoRepository();
        }

        public YearlyDetails(IActivityDtoRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Get activity information to build yearly information.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="year"></param>
        public void Populate(string userId, int? year)
        {
            _details = _repo.GetYearToDateInfo(userId, year);
             _startYear = _details.Min(d => d.Date.Year);
            AddMissingDays();
        }

        /// <summary>
        /// Daily sport information (Year To Date distance, sequence).
        /// </summary>
        public List<YearlyDetailsDayInfo> DayInformation
        {
        get
            {
                return _details;
            }
            private set
            {
             
            }
        }

        /// <summary>
        /// Add any missing days which have no activities for a given sport.
        /// </summary>
        private void AddMissingDays()
        {
            AddMissingDays("Ride");
            AddMissingDays("Run");
            AddMissingDays("Swim");
            AddMissingDays("Other");
            //List<YearToDateDayInfo> allDatesRide = new List<YearToDateDayInfo>();
            //List<YearToDateDayInfo> allDatesRun = new List<YearToDateDayInfo>();
            //List<YearToDateDayInfo> allDatesSwim = new List<YearToDateDayInfo>();
            //List<YearToDateDayInfo> allDatesOther = new List<YearToDateDayInfo>();

            //for (DateTime d = new DateTime(_startYear, 01, 01); d <= new DateTime(DateTime.Now.Year, 12, 31); d = d.AddDays(1))
            //{
            //    allDatesRide.Add(new YearToDateDayInfo() { Date = d, Sport = "Ride" });
            //    allDatesRun.Add(new YearToDateDayInfo() { Date = d, Sport = "Run" });
            //    allDatesSwim.Add(new YearToDateDayInfo() { Date = d, Sport = "Swim" });
            //    allDatesOther.Add(new YearToDateDayInfo() { Date = d, Sport = "Other" });
            //}

            //_details = _details
            //    .Union(allDatesRide.Where(e => !_details.Where(x => x.Sport == "Ride").Select(x => x.Date).Contains(e.Date)))
            //    .Union(allDatesRun.Where(e => !_details.Where(x => x.Sport == "Run").Select(x => x.Date).Contains(e.Date)))
            //    .Union(allDatesSwim.Where(e => !_details.Where(x => x.Sport == "Swim").Select(x => x.Date).Contains(e.Date)))
            //    .Union(allDatesOther.Where(e => !_details.Where(x => x.Sport == "Other").Select(x => x.Date).Contains(e.Date)))
            //    .OrderBy(a => a.Date)
            //    .ToList();
        }

        /// <summary>
        /// Add any missing days which have no activities for a given sport.
        /// </summary>
        private void AddMissingDays(string sport)
        {
        //    List<YearToDateDayInfo> addDates = new List<YearToDateDayInfo>();
        //    List<YearToDateDayInfo> allDatesRun = new List<YearToDateDayInfo>();
        //    List<YearToDateDayInfo> allDatesSwim = new List<YearToDateDayInfo>();
         //   List<YearToDateDayInfo> allDatesOther = new List<YearToDateDayInfo>();

            for (DateTime d = new DateTime(_startYear, 01, 01); d <= new DateTime(DateTime.Now.Year, 12, 31); d = d.AddDays(1))
            {
                if (!_details.Any(a=>a.Date == d && a.Sport == sport ))
                   _details.Add(new YearlyDetailsDayInfo() { Date = d, Sport = sport });
           //     allDatesRun.Add(new YearToDateDayInfo() { Date = d, Sport = "Run" });
            //    allDatesSwim.Add(new YearToDateDayInfo() { Date = d, Sport = "Swim" });
             //   allDatesOther.Add(new YearToDateDayInfo() { Date = d, Sport = "Other" });
            }

            //_details = _details
            //    .Union(addDates.Where(e => !_details.Where(x => x.Sport == sport).Select(x => x.Date).Contains(e.Date)))
            //   // .Union(allDatesRun.Where(e => !_details.Where(x => x.Sport == "Run").Select(x => x.Date).Contains(e.Date)))
            //  //  .Union(allDatesSwim.Where(e => !_details.Where(x => x.Sport == "Swim").Select(x => x.Date).Contains(e.Date)))
            //  //  .Union(allDatesOther.Where(e => !_details.Where(x => x.Sport == "Other").Select(x => x.Date).Contains(e.Date)))
            //    .OrderBy(a => a.Date)
            //    .ToList();

        }

        /// <summary>
        /// Calculate Daily information.
        /// </summary>
        public void Calculate()
        {
            Calculate("Ride");
            Calculate("Run");
            Calculate("Swim");
            Calculate("Other");
        }

        /// <summary>
        /// Calculate Daily information for a given sport.
        /// </summary>
        /// <param name="sport"></param>
        private void Calculate(string sport)
        { 

            int sequence = 0;
            decimal runningYTDDistance = 0;

            foreach (YearlyDetailsDayInfo i in _details.Where(a=>a.Sport==sport))
            {
                if (i.Date.Month == 1 && i.Date.Day == 1)
                    runningYTDDistance = 0;

                runningYTDDistance += i.Distance;
                i.YTDDistance = runningYTDDistance;

                if (i.Distance > 0)
                    i.Sequence = ++sequence;
                else
                    sequence = 0;
            }
        }

        /// <summary>
        /// Find max sequnce for a given sport (and optionally year).
        /// </summary>
        /// <param name="sport"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public int MaxSequence(string sport, int? year)
        {
           return _details
                .Where(a => string.IsNullOrEmpty(sport) ? true : a.Sport == sport && year == null ? true : a.Date.Year == year.Value)
                .Max(a => a.Sequence);
            
        }
    }
}
