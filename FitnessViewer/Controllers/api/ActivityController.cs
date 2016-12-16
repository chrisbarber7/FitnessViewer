using System;
using System.Collections.Generic;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using FitnessViewer.Infrastructure.Models.Dto;
using FitnessViewer.Infrastructure.Models.Collections;
using FitnessViewer.Infrastructure.Interfaces;
using FitnessViewer.Infrastructure.enums;
using FitnessViewer.Infrastructure.Helpers.Conversions;
using FitnessViewer.Infrastructure.Intefaces;
using static FitnessViewer.Infrastructure.Helpers.DateHelpers;

namespace FitnessViewer.Controllers.api
{
    [Authorize]
    public class ActivityController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IActivityDtoRepository _activityRepo;
        private readonly ICoordsDtoRepository _coordsRepo;
        private readonly IGraphStreamDtoRepository _graphRepo;
        private readonly ITimeDistanceBySportRepository _timeDistanceRepo;
        private readonly IPeriodDtoRepository _periodRepo;

        public ActivityController(IUnitOfWork unitOfWork, 
                                    IActivityDtoRepository activityRepo, 
                                    ICoordsDtoRepository coordsRepo,
                                    IGraphStreamDtoRepository graphRepo,
                                    ITimeDistanceBySportRepository timeDistanceRepo,
                                    IPeriodDtoRepository periodRepo)
        {
            _unitOfWork = unitOfWork;
            _activityRepo = activityRepo;
            _coordsRepo = coordsRepo;
            _graphRepo = graphRepo;
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
        public IHttpActionResult GetActivityCoords(string id)
        {
            return Ok(
                _coordsRepo.GetActivityCoords(Convert.ToInt64(id))
            );
        }

        [HttpGet]
        public IHttpActionResult GetActivityStreams(string id)
        {
            return Json(
                 _graphRepo.GetActivityStreams(Convert.ToInt64(id))
            );
        }
    
        [HttpGet]
        [Route("api/Activity/GetTimeAndDistanceBySport")]
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
                duration.Add(Convert.ToInt32( t.Duration));
            }

            var chart = new
            {
                Sport = sport,
                Duration= duration
            };

            return Json(chart);
        }
        
        [HttpGet]
        [Route("api/Activity/GetPeriodDistance/{type}")]
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
            catch(ArgumentException)
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
        public IHttpActionResult GetPowerCurve(string id)
        {
            var powerCurve = ActivityPeakDetails.LoadForActivity(Convert.ToInt64(id)).GetPowerCurve();

            List<string> duration = new List<string>();
            List<string> watts = new List<string>();

            foreach (PowerCurveDto p in powerCurve)
            {
                duration.Add(p.Duration.ToString());
                watts.Add(p.Watts.ToString());
            }

            var chart = new { 
            Duration=duration,
            Watts = watts
            };
            return Ok(chart);
        }

        /* Code below is to work with commented out code in ViewActivity view to allow inline editing of activity name.
         *  Search for INLINE_EDIT for other commented out code.
         * 
         * */
        //[HttpPost]
        //public string  UpdateDescription([FromBody]string description)
        //{
        //    return description + "Updated!";
        //}
    }
}