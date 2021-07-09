using FitnessViewer.Infrastructure.Core.Data;
using FitnessViewer.Infrastructure.Core.enums;
using FitnessViewer.Infrastructure.Core.Interfaces;
using FitnessViewer.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Core.Helpers
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
            Interfaces.IUnitOfWork uow = new Data.UnitOfWork();

            return uow.CRUDRepository.GetAll<PeakStreamTypeDuration>()
                .Where(p => p.PeakStreamType == _streamType && p.Duration != int.MaxValue)
                            .OrderBy(p => p.Duration)
                            .Select(p => p.Duration)
                            .ToArray();

//            return uow.Analysis.GetPeakStreamTypeDuration(_streamType);



        //internal int[] GetPeakStreamTypeDuration(PeakStreamType type)
        //{
        //    return _context.PeakStreamTypeDuration
        //                    .Where(p => p.PeakStreamType == type && p.Duration != int.MaxValue)
        //                    .OrderBy(p => p.Duration)
        //                    .Select(p => p.Duration)
        //                    .ToArray();
        //}

    }

    private int[] SetupPowerCurveDurations(int streamSize)
        {
            List<int> durations = new List<int>();

            // under 1 minutes = every second
            for (int x = 1; x <= 60; x++)
                durations.Add(x);

            // 1 mins to 2 mins = every 5 seconds
            for (int x = 60; x <= 119; x = x + 5)
                durations.Add(x);

            // 2 mins to 5 mins = every 10 seconds
            for (int x = 120; x <= (5 * 60) - 1; x = x + 10)
                durations.Add(x);

            // 5 mins to 10 mins = every 30 seconds.
            for (int x = (5 * 60); x <= (10 * 60) - 1; x = x + 30)
                durations.Add(x);

            // 10 mins to 1 hour = every 1 minute
            for (int x = (10 * 60); x <= (1 * 60 * 60) - 1; x = x + 60)
                durations.Add(x);

            // anything over every 5 mins
            //for (int x = (60 * 60); x <= (3 * 60 * 60) - 1; x = x + 300)
                for (int x = ( 60 * 60); x <= streamSize; x = x + 300)
                durations.Add(x);
            
            // include any duration less than event time (and int.MaxValue for full event).
            _standardDurations = durations.Where(d => d <= streamSize || d == int.MaxValue).ToArray();

            return _standardDurations;
        }
    }
}
