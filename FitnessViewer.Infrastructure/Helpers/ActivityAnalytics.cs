using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Helpers
{
    public class ActivityAnalytics
    {
        private IEnumerable<int> _powerStream;
        private decimal _normalisedPower;
        private decimal _FTP;

        public ActivityAnalytics(IEnumerable<int> powerStream, decimal ftp)
        {
            Setup(powerStream, ftp);
        }


        public ActivityAnalytics(IEnumerable<int?> powerStream, decimal ftp)
        {
            if (powerStream.Contains(null))
                throw new ArgumentException("Stream Contains null values");

            Setup(powerStream.Select(s => s.Value), ftp);
        }

        private void Setup(IEnumerable<int> powerStream, decimal ftp)
        {
            _powerStream = powerStream;
            _normalisedPower = this.CalculateNormalisedPower();
            _FTP = ftp;
        }
     

        private decimal CalculateNormalisedPower()
        {
            /* Formula for Normalised Power...
              
             1) starting at the 30 s mark, calculate a rolling 30 s average (of the preceeding time points, obviously). 
             2) raise all the values obtained in step #1 to the 4th power. 
             3) take the average of all of the values obtained in step #2. 
             4) take the 4th root of the value obtained in step #3. 
              
              */
           
            List<double> averageRaisedTo4thPower = new List<double>();

            for (int count = 29; count <= _powerStream.Count(); count++)
            {
                // get the averagge for the past 30 seconds (step 1)
                double rollingAve = _powerStream.Skip(count - 30).Take(30).Average();

                // raised to 4th Power (step 2)
                double averageRaisedToPower4 = Math.Pow(rollingAve, 4);

                // store.
                averageRaisedTo4thPower.Add(averageRaisedToPower4);
            }

            // get the average of all values stored (step 3)
            double ave = averageRaisedTo4thPower.Sum() / averageRaisedTo4thPower.Count();

            // step 4 (4th root).
            double result = Math.Pow(ave, 0.25);

            return (decimal)Math.Round(result,3);
        }

        public decimal IntensityFactor()
        {
            return _normalisedPower / _FTP;

        }

        public decimal TSS()
        {
            /*TSS = (sec x NP® x IF®)/(FTP x 3600) x 100

                ...where

                “sec” is duration of the workout in seconds,
                “NP” is Normalized Power® (don’t worry about this for now),
                “IF” is Intensity Factor® (a percentage of your FTP; in other words how intense the effort was),
                “FTP” is Functional Threshold Power (your best average power for a one-hour race or test),
                and “3600” is the number of seconds in an hour.
            
             */

            int sec = _powerStream.Count();
            decimal intensityFactor = IntensityFactor();
            decimal tss = (sec * _normalisedPower * IntensityFactor()) / (_FTP * 3600) * 100;

            return tss;
        }

        public decimal NP()
        {
            return _normalisedPower;
        }

    }
}
