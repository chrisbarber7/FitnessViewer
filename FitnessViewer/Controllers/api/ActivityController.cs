using AutoMapper;
using FitnessViewer.Infrastructure.Data;
using FitnessViewer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using FitnessViewer.Infrastructure.Models;
using System.Web.Http.Results;
using Newtonsoft.Json;
using FitnessViewer.Infrastructure.Models.Dto;


namespace FitnessViewer.Controllers.api
{
    [Authorize]
    public class ActivityController : ApiController
    {
        private Repository _repo;

        public ActivityController()
        {
            _repo = new Repository();
        }

        [HttpGet]
        public IHttpActionResult GetActivities()
        {
            return Ok(new
            {
                data =
                Mapper.Map<IEnumerable<ActivityViewModel>>(_repo.GetActivities(this.User.Identity.GetUserId())).ToList()
            });
        }

       [HttpPost]
        public IHttpActionResult GetRunDistancePerWeek()
        {
            var runData = _repo.ActivityByWeek("Run", DateTime.Now.AddDays(12*7*-1), DateTime.Now);

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

            //return Ok(new { chart });

            return Json(chart);
        }
    }
}
