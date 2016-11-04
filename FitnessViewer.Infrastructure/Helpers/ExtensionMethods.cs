using FitnessViewer.Infrastructure.Helpers.Conversions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Helpers
{
   public static class ExtensionMethods
    {
        public static decimal ToMiles(this decimal distanceInMetres)
        {
            return Distance.MetersToMiles(distanceInMetres);
        }

        public static decimal ToFeet(this decimal distanceInMetres)
        {
            return Distance.MetersToFeet(distanceInMetres);
        }
    }
}

