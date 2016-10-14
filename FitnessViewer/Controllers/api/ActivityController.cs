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
                data =
                Mapper.Map<IEnumerable<ActivityViewModel>>(_unitOfWork.Activity.GetActivities(this.User.Identity.GetUserId())).ToList()
            });
        }

        [HttpPost]
        public IHttpActionResult GetActivityCoords(string id)
        {
            var coords = _unitOfWork.Activity.GetActivityCoords(Convert.ToInt64(id));
            return Ok(
               coords
            );
        }

        [HttpPost]
        public IHttpActionResult GetActivityStreams(string id)
        {
            var streams = _unitOfWork.Activity.GetActivityStreams(Convert.ToInt64(id));

            return Json(
               streams
            );
        }

        [HttpPost]
        public IHttpActionResult GetRunDistancePerWeek(string id)
        {
            var runData = _unitOfWork.Activity.ActivityByWeek(this.User.Identity.GetUserId(), id, DateTime.Now.AddDays(12 * 7 * -1), DateTime.Now);

            List<string> period = new List<string>();
            List<string> distance = new List<string>();
            List<string> number = new List<string>();

            foreach (ActivityByPeriodDto a in runData)
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

   
    }

}