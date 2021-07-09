using FitnessViewer.Infrastructure.Core.Data;
using FitnessViewer.Infrastructure.Core.enums;
using FitnessViewer.Infrastructure.Core.Helpers;
using FitnessViewer.Infrastructure.Core.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
//using System.Data.Entity;
using FitnessViewer.Infrastructure.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FitnessViewer.Infrastructure.Core.Repository
{
    public class LapDtoRepository : DtoRepository, ILapDtoRepository
    {
        internal LapDtoRepository() : base()
        {
        }

        internal LapDtoRepository(ApplicationDb context) : base(context)
        {
        }


        public IEnumerable<LapDto> GetLapStream(long activityId, PeakStreamType streamType)
        {
           // string units = DisplayLabel.PeakStreamTypeUnits(streamType);

            var result = _context.ActivityPeakDetail
              .Where(p => p.ActivityId == activityId && p.StreamType == streamType &&
                            _context.PeakStreamTypeDuration
                                .Where(d => d.PeakStreamType == p.StreamType)
                                .Select(d => d.Duration)
                                .ToList()
                                .Contains(p.Duration)
                    )
              .OrderBy(p => p.Duration)
              .Select(p => new LapDto
              {
                  Id = p.Id,
                  Type = streamType,
                  Selected = false,
                  Name = p.Duration.ToString(),
                  Value = p.Value.ToString(),
                  StartIndex = p.StartIndex.Value,
                  EndIndex = p.EndIndex.Value,
                  StreamStep = p.Activity.StreamStep,
                  SteppedStartIndex = p.StartIndex.Value / p.Activity.StreamStep,
                  SteppedEndIndex = p.EndIndex.Value / p.Activity.StreamStep,
              }).ToList();

            foreach (LapDto l in result)
            {
                l.Name = DisplayLabel.StreamDurationForDisplay(Convert.ToInt32(l.Name));
            }
            return result;
        }

        internal IEnumerable<LapDto> GetBestEffort(long id)
        {
            var results = _context.BestEffort
                .Include(b=>b.Activity)
                .Where(b => b.ActivityId == id)
                .OrderBy(b => b.Distance)
                .ToList();

            return results.Select(l => new LapDto
            {
                Id = l.Id,
                Type = PeakStreamType.PaceByDistance,
                Selected = false,
                Name = l.Name,
                Value = PaceCalculator.RunMinuteMiles(l.Distance, l.ElapsedTime).ToMinSec(),
                StartIndex = l.StartIndex,
                EndIndex = l.EndIndex,
                StreamStep = l.Activity.StreamStep,
                SteppedStartIndex = l.StartIndex / l.Activity.StreamStep,
                SteppedEndIndex = l.EndIndex / l.Activity.StreamStep,
            }).ToList();
        }

        public IEnumerable<LapDto> GetLaps(long activityId)
        {
            // split into two queries - first is Linq to sql, second is linq to objects  - need to split as can't format the TimeSpan on Linq to Sql
            var results = _context.Lap
                .Include(a => a.Activity)
                .Where(l => l.ActivityId == activityId).OrderBy(l => l.LapIndex)
                  .Select(l => new
                  {
                      Id = l.Id,
                      Type = PeakStreamType.Lap,
                      Selected = false,
                      Name = l.Name,
                      ElapsedTime = l.ElapsedTime,
                      StartIndex = l.StartIndex,
                      EndIndex = l.EndIndex,
                      SteppedStartIndex = l.StartIndex / l.Activity.StreamStep,
                      SteppedEndEnd = l.EndIndex / l.Activity.StreamStep,
                      StreamStep = l.Activity.StreamStep,
                      MovingTime = l.MovingTime,
                      LapIndex = l.LapIndex
                  })
                  .OrderBy(l => l.LapIndex)
                  .ToList()
                  .Select(l => new LapDto
                  {
                      Id = l.Id,
                      Type = PeakStreamType.Lap,
                      Selected = false,
                      Name = l.Name,
                      Value = l.ElapsedTime.ToString(@"hh\:mm\:ss"),
                      StartIndex = l.StartIndex,
                      EndIndex = l.EndIndex,
                      StreamStep = l.StreamStep,
                      SteppedStartIndex = l.SteppedStartIndex,
                      SteppedEndIndex = l.SteppedEndEnd,
                      MovingTime = l.MovingTime,
                      LapIndex = l.LapIndex
                  }).ToList();


            // if no laps then return to caller without trying to fix data.
            if (results.Count() == 0)
                return results;


            /*
            Code below is a fix for issue with Strava lap details.   If the activity starts in the users
            privacy zone then the StartIndex/EndIndex for the first lap will exclude any points in the privacy zone.
            This results in lap 1 being short by the time in the private zone and all other laps starting/finishing
            early by the same time. 

            Fix is to work out how many points should be in the first lap from the moving time and deduct the EndIndex
            value.  This will give the number of points missing.

            We can then adjust the end index of lap one and the start/end indexes of all other laps.
            */

            // get details of first lap
            LapDto lap1 = results[0];

            // workout how my points we're short by getting number of points we're expecting from the moving time
            // and deducting the number of points we've actually got.
            int shorageInStream = (int)lap1.MovingTime.TotalSeconds - lap1.EndIndex.Value;

            foreach (LapDto lap in results)
            {
                // for the first lap we don't need to adjust (StartIndex will be 0 and should remain 0).  For
                // other laps adjust.
                if (lap.LapIndex != 1)
                    lap.StartIndex += shorageInStream;

                // always adjust the end index.
                lap.EndIndex += shorageInStream;
            }

            return results;
        }



    }
}
