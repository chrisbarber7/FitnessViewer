using System;
using System.Collections.Generic;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using FitnessViewer.Infrastructure.Models.Dto;
using FitnessViewer.Infrastructure.Models.Collections;
using FitnessViewer.Infrastructure.Interfaces;
using FitnessViewer.Infrastructure.enums;
using FitnessViewer.Infrastructure.Helpers.Conversions;
using static FitnessViewer.Infrastructure.Helpers.DateHelpers;
using FitnessViewer.Infrastructure.Helpers;
using System.Linq;

namespace FitnessViewer.Controllers.api
{
    public class AthleteController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IActivityDtoRepository _activityRepo;
        private readonly ITimeDistanceBySportRepository _timeDistanceRepo;
        private readonly IPeriodDtoRepository _periodRepo;

        public AthleteController(IUnitOfWork unitOfWork,
                                    IActivityDtoRepository activityRepo,
                                    ITimeDistanceBySportRepository timeDistanceRepo,
                                    IPeriodDtoRepository periodRepo)
        {
            _unitOfWork = unitOfWork;
            _activityRepo = activityRepo;
            _timeDistanceRepo = timeDistanceRepo;
            _periodRepo = periodRepo;
        }

        [HttpGet]
        public IHttpActionResult GetActivities()
        {
            return Ok(new
            {
                data = _activityRepo.GetActivityDto(this.User.Identity.GetUserId())
            });
        }

        [HttpGet]
        [Route("api/Athlete/GetPeriodDistance/{sport}/{type}")]
        public IHttpActionResult GetPeriodDistance(string sport, string type, [FromUri] DateRange dates)
        {
            if (!dates.FromDateTime.HasValue)
                return BadRequest("Invalid From Date");

            if (!dates.ToDateTime.HasValue)
                return BadRequest("Invalid To Date");

            SportType sportType;
            try
            {
                sportType = EnumConversion.GetEnumFromDescription<SportType>(sport);
            }
            catch (ArgumentException)
            {
                return BadRequest("Invalid Sport Type");
            }

            var runData = _periodRepo.ActivityByWeek(this.User.Identity.GetUserId(), sportType, dates.FromDateTime.Value.Date, dates.ToDateTime.Value.Date);

            List<string> period = new List<string>();
            List<string> distance = new List<string>();
            List<string> number = new List<string>();

            foreach (PeriodDto a in runData)
            {
                period.Add(a.Label);

                if (type.ToLower() == "total")
                    distance.Add(a.TotalDistance.ToString());
                else
                    distance.Add(a.MaximumDistance.ToString());

                number.Add(a.Number.ToString());
            }

            var chart = new
            {
                Period = period,
                distance = distance,
                number = number,
            };

            return Json(chart);
        }



        [HttpGet]
        [Route("api/Athlete/GetTimeAndDistanceBySport")]
        public IHttpActionResult GetTimeAndDistanceBySport([FromUri] DateRange dates)
        {
            if (!dates.FromDateTime.HasValue)
                return BadRequest("Invalid From Date");

            if (!dates.ToDateTime.HasValue)
                return BadRequest("Invalid To Date");

            var data = _timeDistanceRepo.GetTimeDistanceBySport(this.User.Identity.GetUserId(),
                                                                dates.FromDateTime.Value,
                                                                dates.ToDateTime.Value);

            //List<string> sport = new List<string>();
            //List<string> distance = new List<string>();
            //List<int> duration = new List<int>();

            //foreach (TimeDistanceBySportDto t in data)
            //{
            //    sport.Add(t.SportLabel);
            //    duration.Add(Convert.ToInt32(t.Duration));
            //}

            var chart = new
            {
                Sport =data.Select(a=>a.SportLabel).ToArray(),
                Duration = data.Select(a=>Convert.ToInt32(a.Duration)).ToArray()
            };

            return Json(chart);
        }



        [HttpGet]
        [Route("api/Athlete/GetTrainingLoad/{type}")]
        public IHttpActionResult GetTrainingLoad(string type, [FromUri] DateRange dates)
        {
            if (!dates.FromDateTime.HasValue)
                return BadRequest("Invalid From Date");

            if (!dates.ToDateTime.HasValue)
                return BadRequest("Invalid To Date");

            SportType sport;
            try
            {
                sport = EnumConversion.GetEnumFromDescription<SportType>(type);
            }
            catch (ArgumentException)
            {
                return BadRequest("Invalid Sport Type");
            }

            TrainingLoad _trainingLoad;
            _trainingLoad = new TrainingLoad(_activityRepo);
            // need to go back the highest number of days we're interested in plus a yearfor LongTerm training load duration
            // and an extra day to get a seed value.   Add an extra day to the end to hold current form.
            _trainingLoad.Setup(this.User.Identity.GetUserId(), dates.FromDateTime.Value.AddDays(-365), dates.ToDateTime.Value.AddDays(1));
            _trainingLoad.Calculate(sport);

            var requiredDayValues = _trainingLoad.DayValues.Where(d => d.Date >= dates.FromDateTime.Value && d.Date <= dates.ToDateTime.Value).ToList();


            var chart = new
            {
                Date = requiredDayValues.Select(a => a.Date.ToShortDateString()).ToArray(),
                LongTermLoad = requiredDayValues.Select(a => a.ShortTermLoad.ToString()).ToArray(),
                ShortTermLoad = requiredDayValues.Select(a => a.LongTermLoad.ToString()).ToArray(),
                TSS = requiredDayValues.Select(a => a.TSS.ToString()).ToArray()
            };
            return Json(chart);
        }

        [HttpGet]
        [Route("api/Athlete/GetPeaksByMonth/{sport}")]
        public IHttpActionResult GetPeaksByMonth(string sport, [FromUri] DateRange dates)
        {
            if (!dates.FromDateTime.HasValue)
                return BadRequest("Invalid From Date");

            if (!dates.ToDateTime.HasValue)
                return BadRequest("Invalid To Date");

            SportType sportType;
            try
            {
                sportType = EnumConversion.GetEnumFromDescription<SportType>(sport);
            }
            catch (ArgumentException)
            {
                return BadRequest("Invalid Sport Type");
            }

            var peaksData = _periodRepo.PeaksByMonth(this.User.Identity.GetUserId(), dates.FromDateTime.Value.Date, dates.ToDateTime.Value.Date);

            var chart = new
            {
                Period = peaksData.Select(a => a.Label).ToArray(),
                Peak5 = peaksData.Select(a=>a.Peak5.ToString()).ToArray(),
                Peak30 = peaksData.Select(a => a.Peak30.ToString()).ToArray(),
                Peak60 = peaksData.Select(a => a.Peak60.ToString()).ToArray(),
                Peak300 = peaksData.Select(a => a.Peak300.ToString()).ToArray(),
                Peak1200 = peaksData.Select(a => a.Peak1200.ToString()).ToArray(),
                Peak3600 = peaksData.Select(a => a.Peak3600.ToString()).ToArray()
            };

            return Json(chart);
        }


        [HttpGet]
        [Route("api/Athlete/GetPowerCurve")]
        public IHttpActionResult GetPowerCurve( [FromUri] DateRange dates)
        {
            if (!dates.FromDateTime.HasValue)
                return BadRequest("Invalid From Date");

            if (!dates.ToDateTime.HasValue)
                return BadRequest("Invalid To Date");

            var powerCurve = _periodRepo.PowerCurve(this.User.Identity.GetUserId(), dates.FromDateTime.Value.Date, dates.ToDateTime.Value.Date);
            
            var chart = new
            {
                Duration =  powerCurve.Select(a => DisplayLabel.ShortStreamDurationForDisplay( a.Duration)).ToArray(),
                Watts = powerCurve.Select(a=>a.Watts.ToString()).ToArray()
            };
            return Ok(chart);
        }

    }
}
