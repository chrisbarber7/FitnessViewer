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
            Mapper.Initialize(cfg => 
            {
           
                cfg.AddProfile<InfrasturtureProfile>();
                        }
                        );
        }
    }
}