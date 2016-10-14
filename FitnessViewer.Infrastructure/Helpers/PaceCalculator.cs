using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Helpers
{
    public class PaceCalculator
    {
        public static TimeSpan RunMinuteMiles(decimal distance, TimeSpan time)
        {
            double totalSeconds = time.TotalSeconds;
            double distanceInMiles = Convert.ToDouble(distance*MetreDistance.METRE_TO_MILE);
            double averagePaceInSecondsPerMile = Math.Round(totalSeconds/ distanceInMiles) ;

            TimeSpan minPerMile = TimeSpan.FromSeconds(averagePaceInSecondsPerMile);

            return minPerMile;
        }
    }
}
