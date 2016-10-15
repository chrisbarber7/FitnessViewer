using FitnessViewer.Infrastructure.Models.Dto;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FitnessViewer.Controllers.api
{
    [Authorize]
    public class MetricController : ApiController
    {
        private Infrastructure.Data.UnitOfWork _unitOfWork;

        public MetricController()
        {
            _unitOfWork = new Infrastructure.Data.UnitOfWork();
        }

        [HttpGet]
        public IHttpActionResult Get30DayWeight()
        {
            string userId = this.User.Identity.GetUserId();
            var metrics = _unitOfWork.Metrics.GetWeightDetails(userId, 30);

            List<string> date = new List<string>();
            List<string> weight = new List<string>();
            List<string> ave7day = new List<string>();
            List<string> ave30day = new List<string>();

            foreach (WeightByDayDto w in metrics)
            {
                weight.Add(w.Current.ToString());
                date.Add(w.Date.ToShortDateString());
                ave7day.Add(w.Average7Day.ToString());
                ave30day.Add(w.Average30Day.ToString());
            }

            var chart = new
            {
                Date = date,
                Weight = weight,
                Ave7Day = ave7day,
                Ave30day = ave30day
            };

            return Json(chart);
        }
    }
}
