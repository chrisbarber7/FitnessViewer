using AutoMapper;
using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.Models.Dto;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;

namespace FitnessViewer.Controllers.api
{
    [Authorize]
    public class NotificationsController : ApiController
    {
        private UnitOfWork _unitOfWork;

        public NotificationsController()
        {
            _unitOfWork = new UnitOfWork();
        }

        [HttpGet]
        public IEnumerable<Notification> GetNewNotifications()
        {
            var userId = User.Identity.GetUserId();
            return _unitOfWork.Notification.GetUserNotifications(userId);
        }
    }
}
