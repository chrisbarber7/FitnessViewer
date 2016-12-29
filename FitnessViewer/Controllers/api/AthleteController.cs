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
        [Route("api/Athlete/GetPeriodDistance/{type}")]
        public IHttpActionResult GetPeriodDistance(string type, [FromUri] DateRange dates)
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

            var runData = _periodRepo.ActivityByWeek(this.User.Identity.GetUserId(), sport, dates.FromDateTime.Value.Date, dates.ToDateTime.Value.Date);

            List<string> period = new List<string>();
            List<string> distance = new List<string>();
            List<string> number = new List<string>();

            foreach (PeriodDto a in runData)
            {
                period.Add(a.Label);
                distance.Add(a.TotalDistance.ToString());
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

            List<string> sport = new List<string>();
            List<string> distance = new List<string>();
            List<int> duration = new List<int>();

            foreach (TimeDistanceBySportDto t in data)
            {
                sport.Add(t.SportLabel);
                duration.Add(Convert.ToInt32(t.Duration));
            }

            var chart = new
            {
                Sport = sport,
                Duration = duration
            };

            return Json(chart);
        }


    }
}
