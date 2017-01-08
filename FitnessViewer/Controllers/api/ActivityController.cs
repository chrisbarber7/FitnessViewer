using System;
using System.Web.Http;
using FitnessViewer.Infrastructure.Models.Collections;
using FitnessViewer.Infrastructure.Interfaces;
using System.Linq;
using FitnessViewer.Infrastructure.Helpers;

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
            var chart = new {
                Duration = powerCurve.Select(a => DisplayLabel.ShortStreamDurationForDisplay(a.Duration)).ToArray(),
                Watts = powerCurve.Select(a=>a.Watts.ToString()).ToArray()
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