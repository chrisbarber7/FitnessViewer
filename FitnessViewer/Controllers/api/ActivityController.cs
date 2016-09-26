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

 
        public IHttpActionResult Get()
        {
       

            return Json(new { data =
                Mapper.Map<IEnumerable<ActivityViewModel>>(_repo.GetActivities(this.User.Identity.GetUserId())).ToList() });
        }
    }
}
