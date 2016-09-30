using AutoMapper;
using FitnessViewer.Infrastructure.Models;
using FitnessViewer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static FitnessViewer.Infrastructure.Configuration.AutoMapperConfig;

namespace FitnessViewer.App_Start
{
    public static class AutoMapperConfig
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg => {
                cfg.CreateMap<Activity, ActivityViewModel>()
                        .ForMember(dest => dest.Date, opts => opts.MapFrom(src => src.StartDateLocal.Date.ToShortDateString()));

                cfg.AddProfile<InfrasturtureProfile>();
                        }
                        );
        }
    }
}