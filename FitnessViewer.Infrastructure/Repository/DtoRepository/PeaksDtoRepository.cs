using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using FitnessViewer.Infrastructure.enums;
using FitnessViewer.Infrastructure.Models;
using FitnessViewer.Infrastructure.Interfaces;

namespace FitnessViewer.Infrastructure.Repository
{
    public class PeaksDtoRepository : DtoRepository, IPeaksDtoRepository
    {
        public PeaksDtoRepository(ApplicationDb context) : base(context)
        {
        }


        public  string UserId { get; set; }
        /// <summary>
        /// Return Peak information for common time duration
        /// </summary>
        /// <param name="userId">Indentity</param>
        /// <param name="type">Stream Type to analyse</param>
        /// <returns></returns>
        public IEnumerable<PeaksDto> GetPeaks(string userId, PeakStreamType type)
        {

            UserId = userId;

            return new[] { ExtractPeaksByDays(type/*, peaks*/, 7),
                           ExtractPeaksByDays(type/*, peaks*/, 30),
                           ExtractPeaksByDays(type/*, peaks*/, 90),
                           ExtractPeaksByDays(type/*, peaks*/, 365),
                           ExtractPeaksByDays(type/*, peaks*/, int.MaxValue) };
        }

        public  PeaksDto ExtractPeaksByDays(PeakStreamType type, /*IQueryable<ActivityPeaks> peaks,*/ int days)
        {
            // days=int.maxvalue is used for earlist date
            DateTime earliestDate = days == int.MaxValue ? DateTime.MinValue.Date : DateTime.Now.AddDays(days * -1).Date;
            return ExtractPeaks(type, /*peaks,*/ earliestDate, DateTime.Now.Date);
        }

        public  PeaksDto ExtractPeaks(PeakStreamType type, /*IQueryable<ActivityPeaks> peaks,*/ DateTime start, DateTime end )
        {
            if (string.IsNullOrEmpty(UserId))
                throw new ArgumentException("UserId invalid");

            var peaks = _context.ActivityPeak
        .Where(p => p.Activity.Athlete.UserId == UserId && p.StreamType == type)
        .Include(p => p.Activity);


            PeaksDto ap = new PeaksDto();
            ap.PeakType = type;

            // DateTime.MinValue = all activities so need to set days to int.maxValue so that the client can identify it as all days/activities.
            if (start == DateTime.MinValue)
                ap.Days = int.MaxValue;
            else
                ap.Days = Convert.ToInt32(end.Subtract(start).TotalDays);

            ap.DurationPeaks.Add(peaks.Where(p => p.Activity.Start >= start && p.Activity.Start <= end)
                                .OrderByDescending(p => p.Peak5)
                                .Select(p => new PeaksDetailDto() { Duration=5, Peak = p.Peak5, ActivityId = p.ActivityId, Description = p.Activity.Name })
                                .FirstOrDefault());

            ap.DurationPeaks.Add(peaks.Where(p => p.Activity.Start >= start && p.Activity.Start <= end)
                           .OrderByDescending(p => p.Peak60)
                                .Select(p => new PeaksDetailDto() { Duration = 60, Peak = p.Peak60, ActivityId = p.ActivityId, Description = p.Activity.Name })
                                .FirstOrDefault());

            ap.DurationPeaks.Add(peaks.Where(p => p.Activity.Start >= start && p.Activity.Start <= end)
                           .OrderByDescending(p => p.Peak300)
                                .Select(p => new PeaksDetailDto() { Duration=300, Peak = p.Peak300, ActivityId = p.ActivityId, Description = p.Activity.Name })
                                .FirstOrDefault());

            ap.DurationPeaks.Add(peaks.Where(p => p.Activity.Start >= start && p.Activity.Start <= end)
                                .OrderByDescending(p => p.Peak1200)
                                .Select(p => new PeaksDetailDto() { Duration=1200, Peak = p.Peak1200, ActivityId = p.ActivityId, Description = p.Activity.Name })
                                .FirstOrDefault());

            ap.DurationPeaks.Add(peaks.Where(p => p.Activity.Start >= start && p.Activity.Start <= end)
                     .OrderByDescending(p => p.Peak3600)
                                .Select(p => new PeaksDetailDto() { Duration=3600, Peak = p.Peak3600, ActivityId = p.ActivityId, Description = p.Activity.Name })
                                .FirstOrDefault());

            return ap;
        }

    }
}
