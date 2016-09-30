using AutoMapper;
using FitnessViewer.Infrastructure.Repository;
using FitnessViewer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using FitnessViewer.Infrastructure.Models.Dto;


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
        public IHttpActionResult GetRunDistancePerWeek(string id)
        {
            var runData = _unitOfWork.Activity.ActivityByWeek(id, DateTime.Now.AddDays(12*7*-1), DateTime.Now);

            List<string> period = new List<string>();
            List<string> distance = new List<string>();
            List<string> number = new List<string>();

            foreach (ActivityByPeriod a in runData)
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
