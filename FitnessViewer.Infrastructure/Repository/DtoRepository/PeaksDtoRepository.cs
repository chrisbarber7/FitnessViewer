using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using FitnessViewer.Infrastructure.enums;
using FitnessViewer.Infrastructure.Models;

namespace FitnessViewer.Infrastructure.Repository
{
    public class PeaksDtoRepository : DtoRepository
    {
        public PeaksDtoRepository(ApplicationDb context) : base(context)
        {
        }

        /// <summary>
        /// Return Peak information for common time duration
        /// </summary>
        /// <param name="userId">Indentity</param>
        /// <param name="type">Stream Type to analyse</param>
        /// <returns></returns>
        public IEnumerable<PeaksDto> GetPeaks(string userId, PeakStreamType type)
        {
            var peaks = _context.ActivityPeak
                  .Where(p => p.Activity.Athlete.UserId == userId && p.StreamType == type)
                  .Include(p => p.Activity);

            return new[] { ExtractPeaksByDays(type, peaks, 7),
                           ExtractPeaksByDays(type, peaks, 30),
                           ExtractPeaksByDays(type, peaks, 90),
                           ExtractPeaksByDays(type, peaks, 365),
                           ExtractPeaksByDays(type, peaks, int.MaxValue) };
        }

        private static PeaksDto ExtractPeaksByDays(PeakStreamType type, IQueryable<ActivityPeaks> peaks, int days)
        {
            // days=int.maxvalue is used for earlist date
            DateTime earliestDate = days == int.MaxValue ? DateTime.MinValue.Date : DateTime.Now.AddDays(days * -1).Date;

            PeaksDto ap = new PeaksDto();
            ap.PeakType = type;
            ap.Days = days;

            ap.DurationPeaks.Add(peaks.Where(p => p.Activity.Start >= earliestDate)
                                .OrderByDescending(p => p.Peak5)
                                .Select(p => new PeaksDto.PeaksDtoDetail() { Duration=5, Peak = p.Peak5, ActivityId = p.ActivityId, Description = p.Activity.Name })
                                .FirstOrDefault());

            ap.DurationPeaks.Add(peaks.Where(p => p.Activity.Start >= earliestDate)
                                .OrderByDescending(p => p.Peak60)
                                .Select(p => new PeaksDto.PeaksDtoDetail() { Duration = 60, Peak = p.Peak60, ActivityId = p.ActivityId, Description = p.Activity.Name })
                                .FirstOrDefault());

            ap.DurationPeaks.Add(peaks.Where(p => p.Activity.Start >= earliestDate)
                                .OrderByDescending(p => p.Peak300)
                                .Select(p => new PeaksDto.PeaksDtoDetail() { Duration=300, Peak = p.Peak300, ActivityId = p.ActivityId, Description = p.Activity.Name })
                                .FirstOrDefault());

            ap.DurationPeaks.Add(peaks.Where(p => p.Activity.Start >= earliestDate)
                                .OrderByDescending(p => p.Peak1200)
                                .Select(p => new PeaksDto.PeaksDtoDetail() { Duration=1200, Peak = p.Peak1200, ActivityId = p.ActivityId, Description = p.Activity.Name })
                                .FirstOrDefault());

            ap.DurationPeaks.Add(peaks.Where(p => p.Activity.Start >= earliestDate)
                                .OrderByDescending(p => p.Peak3600)
                                .Select(p => new PeaksDto.PeaksDtoDetail() { Duration=3600, Peak = p.Peak3600, ActivityId = p.ActivityId, Description = p.Activity.Name })
                                .FirstOrDefault());

            return ap;
        }

    }
}
