using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.Models;
using FitnessViewer.Infrastructure.Models.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Helpers.Analytics
{
    public class BikePower
    {
        /// <summary>
        /// Data can either be held as a list of values(_powerStream) or full Stream information 
        /// (_stream) depending on which constructor is used.
        /// </summary>
        private IEnumerable<int> _powerStream;
        private IEnumerable<Stream> _stream;

        private decimal _normalisedPower;
        private decimal _FTP;

        public BikePower(IEnumerable<Stream> stream, decimal ftp)
        {
            Setup(stream, ftp);
        }

        public BikePower(IEnumerable<int> powerStream, decimal ftp)
        {
            Setup(powerStream, ftp);
        }
        
        public BikePower(IEnumerable<int?> powerStream, decimal ftp)
        {
            if (powerStream.Contains(null))
                throw new ArgumentException("Stream Contains null values");

            Setup(powerStream.Select(s => s.Value), ftp);
        }

        private void Setup(IEnumerable<int> powerStream, decimal ftp)
        {
            _powerStream = powerStream;
            _FTP = ftp;

            // can't calculate NP if less than 30 data points.
            if (_powerStream.Count() > 30)
                _normalisedPower = this.CalculateNormalisedPower();
            else
                _normalisedPower = Convert.ToDecimal(_powerStream.Average());
          
        }

        private void Setup(IEnumerable<Stream> stream, decimal ftp)
        {
            _powerStream = null;
            _stream = stream;
            _FTP = ftp;

            // can't calculate NP if less than 30 data points.
            if (_stream.Count() > 30)
                _normalisedPower = this.CalculateNormalisedPower();
            else
                _normalisedPower = 0.00M;
        }

        private decimal CalculateNormalisedPower()
        {
            /* Formula for Normalised Power...
              
             1) starting at the 30 s mark, calculate a rolling 30 s average (of the preceeding time points, obviously). 
             2) raise all the values obtained in step #1 to the 4th power. 
             3) take the average of all of the values obtained in step #2. 
             4) take the 4th root of the value obtained in step #3. 
              
              */
           

            double runningAverage = 0;
            int recordsAveraged = 0;

            Queue<int> rollingValues = new Queue<int>();

            // calcualte based on which whether values held in Stream collection or just values as int collection
            if (_powerStream != null)
            {
                foreach (int i in _powerStream)
                {
                    rollingValues.Enqueue(i);

                    if (rollingValues.Count < 30)
                        continue;

                    runningAverage += Math.Pow(rollingValues.Average(), 4);
                    recordsAveraged++;

                    rollingValues.Dequeue();
                }
            }
            else
            {
                foreach (Stream s in _stream)
                {
                    rollingValues.Enqueue(s.Watts.HasValue ? s.Watts.Value : 0);

                    if (rollingValues.Count < 30)
                        continue;

                    runningAverage += Math.Pow(rollingValues.Average(), 4);
                    recordsAveraged++;

                    rollingValues.Dequeue();
                }
            }

            // get the average of all values stored (step 3)
             double ave = runningAverage / recordsAveraged;

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

            int sec = _powerStream != null ? _powerStream.Count() : _stream.Count();
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
