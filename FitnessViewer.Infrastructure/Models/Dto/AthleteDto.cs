using FitnessViewer.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Models.Dto
{
    public class AthleteDto
    {
        public static AthleteDto Create(UnitOfWork uow, string userId)
        {
            Athlete a = uow.Athlete.FindAthleteByUserId(userId);

            if (a == null)
                return null;

            AthleteDto dto = new AthleteDto() {
                Id = a.Id,
                FirstName = a.FirstName,
                LastName = a.LastName
            };

            return dto;
        }

        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
