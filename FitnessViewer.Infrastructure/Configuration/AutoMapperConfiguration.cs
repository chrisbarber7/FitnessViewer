using AutoMapper;
using FitnessViewer.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Configuration
{



    public static class AutoMapperConfig
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg => {
                cfg.AddProfile<InfrasturtureProfile>();
                });    
        }

        public class InfrasturtureProfile : Profile
        {
            public InfrasturtureProfile()
            {
                CreateMap<Athlete, Strava.Athletes.Athlete>().ReverseMap();
           //     CreateMap<Activity, Strava.Activities.Activity>().ReverseMap();
             

            }
        }
    }
}





    