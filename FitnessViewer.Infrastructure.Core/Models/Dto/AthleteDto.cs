using AutoMapper;
using FitnessViewer.Infrastructure.Core.Configuration;
using FitnessViewer.Infrastructure.Core.Data;
using FitnessViewer.Infrastructure.Core.Helpers;
using FitnessViewer.Infrastructure.Core.Interfaces;
using System;
using System.Linq;

namespace FitnessViewer.Infrastructure.Core.Models.Dto
{
    public class AthleteDto
    {
        public static AthleteDto Create(IUnitOfWork uow, string userId)
        {
            Athlete a = uow.CRUDRepository.GetByUserId<Athlete>(userId, o => o.AthleteSetting).FirstOrDefault();

            if (a == null)
                return null;

            AthleteDto dto = ObjectMapper.Mapper.Map<AthleteDto>(a);
            dto.SetupDateRange();
            return dto;
        }

        public static AthleteDto CreateFromAthlete(Athlete fvAthlete)
        {
            return ObjectMapper.Mapper.Map<AthleteDto>(fvAthlete);
        }

        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserId { get; set; }

        public AthleteSetting AthleteSetting { get; set; }


        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        internal void SetupDateRange()
        {
            DashboardDateRange range = DashboardDateRange.CreateAndCalulcate(AthleteSetting);
            Start = range.Start;
            End = range.End;
        }
    }
}
