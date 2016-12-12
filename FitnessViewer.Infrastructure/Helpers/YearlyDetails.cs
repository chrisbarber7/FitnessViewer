using FitnessViewer.Infrastructure.enums;
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
            AddMissingDays(SportType.Ride);
            AddMissingDays(SportType.Run);
            AddMissingDays(SportType.Swim);
            AddMissingDays(SportType.Other);
        }

        /// <summary>
        /// Add any missing days which have no activities for a given sport.
        /// </summary>
        private void AddMissingDays(SportType sport)
        {
            List<YearlyDetailsDayInfo> allDates = new List<YearlyDetailsDayInfo>();

            for (DateTime d = new DateTime(_startYear, 01, 01); d <= new DateTime(DateTime.Now.Year, 12, 31); d = d.AddDays(1))
                allDates.Add(new YearlyDetailsDayInfo() { Date = d, Sport = sport });

            // add in any missing dates by Union
            _details = _details.Union(allDates, new YearlyDetailsDayInfo()).ToList();
        }
        
        /// <summary>
        /// Calculate Daily information.
        /// </summary>
        public void Calculate()
        {
            Calculate(SportType.Ride);
            Calculate(SportType.Run);
            Calculate(SportType.Swim);
            Calculate(SportType.Other);
        }

        /// <summary>
        /// Calculate Daily information for a given sport.
        /// </summary>
        /// <param name="sport"></param>
        private void Calculate(SportType sport)
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
        public int MaxSequence(SportType sport, int? year)
        {
        
            return _details
                    .Where(a => sport == SportType.All ? true : a.Sport == sport)
                    .Where(a => year == null ? true : a.Date.Year == year.Value)
                    .Select(a=>a.Sequence)
                    .DefaultIfEmpty()
                    .Max();
            
        }

        /// <summary>
        /// Find daily average distance for a given sport/year
        /// </summary>
        /// <param name="sport"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public decimal DailyAverage(SportType sport, int? year)
        {
            return _details
                    .Where(a => sport == SportType.All ? true : a.Sport == sport )
                    .Where(a=> year == null ? true : a.Date.Year == year.Value)
                    .Select(a=>a.Distance)
                    .DefaultIfEmpty()
                    .Average();
        }
    }
}
