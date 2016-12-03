using FitnessViewer.Infrastructure.Models.Collections;
using FitnessViewer.Infrastructure.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Helpers.Analytics
{
    public class HeartRateTss
    {
        private readonly ActivityStreams _activity;
        
        public HeartRateTss(ActivityStreams activity)
        {
            _activity = activity;
        }

        private List<HeartRateTSSZone> bikeHrZones = new List<HeartRateTSSZone>()
        {
           new HeartRateTSSZone() { ZoneType= enums.ZoneType.BikeHeartRate, StartValue=0, EndValue=80, TSSPerHour=30, ZoneName="Zone 1" },
           new HeartRateTSSZone() { ZoneType= enums.ZoneType.BikeHeartRate, StartValue=81, EndValue=89, TSSPerHour=55, ZoneName="Zone 2" },
           new HeartRateTSSZone() { ZoneType= enums.ZoneType.BikeHeartRate, StartValue=0, EndValue=90, TSSPerHour=93, ZoneName="Zone 3" },
           new HeartRateTSSZone() { ZoneType= enums.ZoneType.BikeHeartRate, StartValue=0, EndValue=94, TSSPerHour=99, ZoneName="Zone 4" },
           new HeartRateTSSZone() { ZoneType= enums.ZoneType.BikeHeartRate, StartValue=0, EndValue=100, TSSPerHour=102, ZoneName="Zone 5a" },
           new HeartRateTSSZone() { ZoneType= enums.ZoneType.BikeHeartRate, StartValue=0, EndValue=103, TSSPerHour=105, ZoneName="Zone 5b" },
           new HeartRateTSSZone() { ZoneType= enums.ZoneType.BikeHeartRate, StartValue=0, EndValue=106, TSSPerHour=999, ZoneName="Zone 5c" }
        };
        
        // Zone 1 Less than 81% of LTHR (20-40 TSS per hour)
        // Zone 2 81% to 89% of LTHR (50-60 TSS per hour)
        // Zone 3 90% to 93% of LTHR (70 TSS per hour)
        // Zone 4 94% to 99% of LTHR (80 TSS per hour)
        // Zone 5a 100% to 102% of LTHR (100 TSS per hour)
        // Zone 5b 103% to 106% of LTHR (120 TSS per hour)
        // Zone 5c More than 106% of LTHR (140 TSS per hour)


        public class HeartRateTSSZone : ZoneValueBase
        {
            public decimal TSSPerHour { get; set; }
            public decimal TSS { get; set; }
        }
    }
}
