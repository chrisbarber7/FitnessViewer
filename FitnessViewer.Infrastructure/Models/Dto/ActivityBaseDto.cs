using FitnessViewer.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Models.Dto
{
    public class ActivityBaseDto
    {
        public static ActivityBaseDto CreateFromActivity( Activity fvActivity)
        {
            ActivityBaseDto m = new ActivityBaseDto();

            m.Id = fvActivity.Id;
            m.Name = fvActivity.Name;
            m.ActivityTypeId = fvActivity.ActivityTypeId;
            m.DetailsDownloaded = true;
            m.Distance = fvActivity.ActivityType.IsSwim ? fvActivity.Distance : fvActivity.Distance.ToMiles();
            m.AverageSpeed = 0;
            m.AveragePace = PaceCalculator.RunMinuteMiles(fvActivity.Distance, fvActivity.ElapsedTime.Value);
            m.ElevationGain = fvActivity.ElevationGain.ToFeet();
            m.Date = fvActivity.StartDateLocal.ToShortDateString();
            m.MovingTime = fvActivity.MovingTime.Value;
       
            return m;
        }

        public long Id { get; set; }
        public bool DetailsDownloaded { get; set; }
        public string Name { get; set; }
        public decimal Distance { get; set; }
        public decimal AverageSpeed { get; set; }
        public TimeSpan AveragePace { get; set; }
        public decimal ElevationGain { get; set; }
        public string Date { get; set; }
        public DateTime StartDateLocal { get; set; }

        public TimeSpan MovingTime { get; set; }

        public string ActivityTypeId { get; set; }

    }
}


