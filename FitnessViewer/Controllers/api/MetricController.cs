﻿using FitnessViewer.Infrastructure.Models.Dto;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using static FitnessViewer.Infrastructure.Helpers.DateHelpers;

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
        public IHttpActionResult GetWeightMetrics([FromUri] DateRange dates)
        {
            string userId = this.User.Identity.GetUserId();

            if (!dates.FromDateTime.HasValue)
                return BadRequest("Invalid From Date");

            if (!dates.ToDateTime.HasValue)
                return BadRequest("Invalid To Date");
            
            var metrics = _unitOfWork.Metrics.GetWeightDetails(userId, dates.FromDateTime.Value, dates.ToDateTime.Value);

            List<string> date = new List<string>();
            List<string> weight = new List<string>();
            List<string> ave7day = new List<string>();
            List<string> ave30day = new List<string>();

            foreach (WeightByDayDto w in metrics)
            {
                if ((w.Current != null) && (w.Current != 0))
                    weight.Add(w.Current.ToString());
                else
                    weight.Add(null);

                date.Add(w.Date.ToString("dd MMM"));

                if ((w.Average7Day != null) && (w.Average7Day != 0))
                    ave7day.Add(w.Average7Day.ToString());
                else
                    ave7day.Add(null);

                if ((w.Average30Day != null) && (w.Average30Day != 0))
                    ave30day.Add(w.Average30Day.ToString());
                else
                    ave30day.Add(null);
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
