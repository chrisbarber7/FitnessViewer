using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Test
{
    public class AutoMapperConfiguration
    {
        public static void Initialise()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<Infrastructure.Configuration.AutoMapperConfig.InfrasturtureProfile>();
            });
        }
    }
}
