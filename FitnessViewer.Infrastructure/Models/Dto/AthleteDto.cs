using AutoMapper;
using FitnessViewer.Infrastructure.Data;
using System.Linq;

namespace FitnessViewer.Infrastructure.Models.Dto
{
    public class AthleteDto
    {
        public static AthleteDto Create(UnitOfWork uow, string userId)
        {
            Athlete a = uow.CRUDRepository.GetByUserId<Athlete>(userId).FirstOrDefault();

            if (a == null)
                return null;

            AthleteDto dto = Mapper.Map<AthleteDto>(a);
            //AthleteDto dto = new AthleteDto() {
            //    Id = a.Id,
            //    FirstName = a.FirstName,
            //    LastName = a.LastName
            //};

            return dto;
        }

        public static AthleteDto CreateFromAthlete(Athlete fvAthlete)
        {
            return AutoMapper.Mapper.Map<AthleteDto>(fvAthlete);
        }

        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserId { get; set; }
    }
}
