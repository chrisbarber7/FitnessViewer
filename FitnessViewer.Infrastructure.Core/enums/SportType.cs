using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Core.enums
{
    public enum SportType
    {
        [Description("All")]
        All=0,
        [Description("Ride")]
        Ride=1,
        [Description("Run")]
        Run =2,
        [Description("Swim")]
        Swim =3,
        [Description("Other")]
        Other =4

    }
}


