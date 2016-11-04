using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Helpers
{
    public class PeakDuration
    {
        private PeakStreamType _streamType;
        private int[] _standardDurations;

        public static PeakDuration Create(PeakStreamType type)
        {
            return new PeakDuration(type, null);
        }

        public static PeakDuration CreatePowerCurveDurations(int? streamSize)
        {
            return new PeakDuration(PeakStreamType.Power, streamSize);
        }

     
        private PeakDuration(PeakStreamType streamType, int? streamSize)
        {
            _streamType = streamType;

            if (streamSize == null)
                _standardDurations = SetupDurations();        
            else
                _standardDurations = SetupPowerCurveDurations(streamSize.Value);
        }


        public int[] Durations
        {
            get { return _standardDurations; }
        }

        private int[] SetupDurations()
        {
            UnitOfWork uow = new UnitOfWork();
            return uow.Analysis.GetPeakStreamTypeDuration(_streamType);
        }

        private int[] SetupPowerCurveDurations(int streamSize)
        {
            List<int> durations = new List<int>();

            // under 3 minutes = every second
            for (int x = 1; x <= 179; x++)
                durations.Add(x);

            // 3 mins to 5 mins = every 2 seconds
            for (int x = 180; x <= 299; x = x + 2)
                durations.Add(x);

            // 5 mins to 30 mins = every 5 seconds
            for (int x = 300; x <= (30 * 60) - 1; x = x + 5)
                durations.Add(x);

            // 30 mins to 1 hour = every 30 seconds.
            for (int x = (30 * 60); x <= (60 * 60) - 1; x = x + 30)
                durations.Add(x);

            // one hour to two hours = every 60 seconds.
            for (int x = (60 * 60); x <= (2 * 60 * 60) - 1; x = x + 60)
                durations.Add(x);

            // anything over every 5 mins
            for (int x = (2 * 60 * 60); x <= (3 * 60 * 60) - 1; x = x + 300)
                durations.Add(x);
            
            // include any duration less than event time (and int.MaxValue for full event).
            _standardDurations = durations.Where(d => d <= streamSize || d == int.MaxValue).ToArray();

            return _standardDurations;
        }
    }
}
