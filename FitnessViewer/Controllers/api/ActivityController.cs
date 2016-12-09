using AutoMapper;
using FitnessViewer.Infrastructure.Repository;
using FitnessViewer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using FitnessViewer.Infrastructure.Models.Dto;
using System.Net.Http;
using FitnessViewer.Infrastructure.Models.Collections;
using FitnessViewer.Infrastructure.Interfaces;
using FitnessViewer.Infrastructure.enums;
using FitnessViewer.Infrastructure.Helpers.Conversions;

namespace FitnessViewer.Controllers.api
{
    [Authorize]
    public class ActivityController : ApiController
    {
        private IUnitOfWork _unitOfWork;

        public ActivityController()
        {
            _unitOfWork = new Infrastructure.Data.UnitOfWork();
        }

        [HttpGet]
        public IHttpActionResult GetActivities()
        {
            ActivityDtoRepository repo = new ActivityDtoRepository();

            return Ok(new
            {
                //data =
                //Mapper.Map<IEnumerable<ActivityLapsDto>>(_unitOfWork.Activity.GetActivities(this.User.Identity.GetUserId())).ToList()


              

                data = repo.GetActivityDto(this.User.Identity.GetUserId())

            });
        }

        [HttpGet]
        public IHttpActionResult GetActivityCoords(string id)
        {
            CoordsDtoRepository repo = new CoordsDtoRepository();

            var coords = repo.GetActivityCoords(Convert.ToInt64(id));
            return Ok(
               coords
            );
        }

        [HttpGet]
        public IHttpActionResult GetActivityStreams(string id)
        {
            GraphStreamDtoRepository dtoRepo = new GraphStreamDtoRepository();

            var streams = dtoRepo.GetActivityStreams(Convert.ToInt64(id));

            return Json(
               streams
            );
        }

        [HttpGet]
        public IHttpActionResult GetTimeAndDistanceBySport(string id)
        {

            int daysValue;

            if (!int.TryParse(id, out daysValue))
                return BadRequest("Invalid Days Parameter");

            TimeDistanceBySportRepository repo = new TimeDistanceBySportRepository();

            var data = repo.GetTimeDistanceBySport(this.User.Identity.GetUserId(), 
                                            DateTime.Now.AddDays(daysValue*-1), DateTime.Now);

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
        public IHttpActionResult GetRunDistancePerWeek(string id)
        {
            SportType sport;
            try
            {
                 sport = EnumConversion.GetEnumFromDescription<SportType>(id);
            }
            catch(ArgumentException)
            {
                return BadRequest("Invalid Sport Type");
            }

            PeriodDtoRepository repo = new PeriodDtoRepository();
            var runData = repo.ActivityByWeek(this.User.Identity.GetUserId(), sport, DateTime.Now.AddDays(12 * 7 * -1), DateTime.Now);

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