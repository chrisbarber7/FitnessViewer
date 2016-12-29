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
    [Authorize]
    public class ActivityController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICoordsDtoRepository _coordsRepo;
        private readonly IGraphStreamDtoRepository _graphRepo;
      
        public ActivityController(IUnitOfWork unitOfWork, 
                                    ICoordsDtoRepository coordsRepo,
                                    IGraphStreamDtoRepository graphRepo)
        {
            _unitOfWork = unitOfWork;
            _coordsRepo = coordsRepo;
            _graphRepo = graphRepo;
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