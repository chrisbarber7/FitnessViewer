using FitnessViewer.Infrastructure.Core.enums;
using FitnessViewer.Infrastructure.Core.Interfaces;
using FitnessViewer.Infrastructure.Core.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Core.Helpers
{
    /// <summary>
    /// Calculate Short/Long Term training load/stress
    /// </summary>
    public class TrainingLoad
    {
        private DateTime _end;
        private DateTime _start;
        private string _userId;
        private readonly IActivityDtoRepository _repo;

        private double _shortTermDays { get; set; }
        private double _longTermDays { get; set; }

        private decimal _shortTermSeed { get; set; }
        private decimal _loadTermSeed { get; set; }

        /// <summary>
        /// Number of days to use for short term
        /// </summary>
        public double ShortTermDays
        {
            get { return _shortTermDays; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Invalid ShortTermDays");
                _shortTermDays = value;
            }
        }

        // number of days to use for long term.
        public double LongTermDays
        {
            get { return _longTermDays; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Invalid LongTermDays");

                _longTermDays = value;
            }
        }

        /// <summary>
        /// Starting/Seed value for short term stress.
        /// </summary>
        public decimal ShortTermSeed
        {
            get { return _shortTermSeed; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Invalid ShortTermSeed");

                _shortTermSeed = value;
            }
        }

        /// <summary>
        /// Starting/Seed value for long term load
        /// </summary>
        public decimal LongTermSeed
        {
            get { return _loadTermSeed; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Invalid LongTermSeed");

                _loadTermSeed = value;
            }
        }

        public List<TrainingLoadDay> DayValues { get; private set; }

        public TrainingLoad()
        {
            _repo = new ActivityDtoRepository();
            Init();
        }

        public TrainingLoad(IActivityDtoRepository repo)
        {
            _repo = repo;
            Init();
        }

        private void Init()
        {
            _loadTermSeed = 0;
            _shortTermSeed = 0;
            _shortTermDays = 7;
            _longTermDays = 42;
        }
        public void Setup(string userId, DateTime start, DateTime end)
        {
            if (end < start)
                throw new ArgumentException("Invalid Start/End Dates");

            _userId = userId;
            _start = start.Date;
            _end = end.Date;
            InitialiseDayValues();
        }

        public void Calculate(SportType sport)
        {
          
            PopulateDailyTSS(sport);
            Calculate();
        }

        private void PopulateDailyTSS(SportType sport)
        {
            var dailyValues = _repo.GetDailyTSS(_userId, sport, _start, _end);

            foreach (TrainingLoadDay day in DayValues)
            {
                decimal? tss = dailyValues.Where(k => k.Key == day.Date).Select(k => k.Value).FirstOrDefault();

                if (tss != null)
                    day.TSS = tss.Value;
            }
        }

        /// <summary>
        /// Do the actual calculations.  Based on formulas from spreadsheet at 
        /// http://www.coachcox.co.uk/2012/03/30/how-to-plan-a-season-using-the-performance-management-chart/
        /// Descriptions at http://home.trainingpeaks.com/blog/article/what-is-the-performance-management-chart
        /// </summary>
        private void Calculate()
        {
            decimal previousShortTerm = ShortTermSeed;
            decimal previousLongTerm = LongTermSeed;

            decimal shortTermEXP = Convert.ToDecimal(Math.Exp(-1 / _shortTermDays));
            decimal longTermEXP = Convert.ToDecimal(Math.Exp(-1 / _longTermDays));
            
            foreach (TrainingLoadDay day in DayValues)
            {
                previousShortTerm = day.ShortTermLoad = day.TSS * (1 - shortTermEXP) + previousShortTerm * shortTermEXP;
                previousLongTerm = day.LongTermLoad = day.TSS * (1 - longTermEXP) + previousLongTerm * longTermEXP;
            }
        }

        private void InitialiseDayValues()
        {
            DayValues = new List<TrainingLoadDay>();
            for (DateTime date = _start; date <= _end; date = date.AddDays(1))
                DayValues.Add(new TrainingLoadDay(date));
        }

        public string Json(DateTime start, DateTime end)
        {
            // overall list maybe larger than the section we need to filter.
            var results = DayValues.Where(d => d.Date >= start && d.Date <= end).ToList();

            return JsonConvert.SerializeObject(DayValues);
        }
    }

    public class TrainingLoadDay
    {
        public TrainingLoadDay(DateTime date)
        {
            Date = date;
        }

        private decimal _shortTermLoad;
        private decimal _longTermLoad;

        public DateTime Date { get; set; }
        public decimal TSS { get; set; }
        public decimal ShortTermLoad
        {
            get { return Math.Round(_shortTermLoad, 2); }
            set { _shortTermLoad = value; }
        }
        public decimal LongTermLoad
        {
            get { return Math.Round(_longTermLoad, 2); }
            set { _longTermLoad = value; }
        }
        public decimal Form
        {
            get { return LongTermLoad - ShortTermLoad; }
            private set { }
        }
    }
}
