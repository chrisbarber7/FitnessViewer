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

namespace FitnessViewer.Controllers.api
{
    [Authorize]
    public class ActivityController : ApiController
    {
        private Infrastructure.Data.UnitOfWork _unitOfWork;

        public ActivityController()
        {
            _unitOfWork = new Infrastructure.Data.UnitOfWork();
        }

        [HttpGet]
        public IHttpActionResult GetActivities()
        {
            return Ok(new
            {
                //data =
                //Mapper.Map<IEnumerable<ActivityLapsDto>>(_unitOfWork.Activity.GetActivities(this.User.Identity.GetUserId())).ToList()

                data = _unitOfWork.Activity.GetActivityDto(this.User.Identity.GetUserId())

            });
        }

        [HttpGet]
        public IHttpActionResult GetActivityCoords(string id)
        {
            var coords = _unitOfWork.Activity.GetActivityCoords(Convert.ToInt64(id));
            return Ok(
               coords
            );
        }

        [HttpGet]
        public IHttpActionResult GetActivityStreams(string id)
        {
            var streams = _unitOfWork.Activity.GetActivityStreams(Convert.ToInt64(id));

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


            var data = _unitOfWork.Activity.GetTimeDistanceBySport(this.User.Identity.GetUserId(), 
                                            DateTime.Now.AddDays(daysValue*-1), DateTime.Now);

            List<string> sport = new List<string>();
            List<string> distance = new List<string>();
            List<int> duration = new List<int>();

            foreach (TimeDistanceBySportDto t in data)
            {
                sport.Add(t.Sport);
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
            var runData = _unitOfWork.Activity.ActivityByWeek(this.User.Identity.GetUserId(), id, DateTime.Now.AddDays(12 * 7 * -1), DateTime.Now);

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