using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.enums;
using FitnessViewer.Infrastructure.Helpers;
using FitnessViewer.Infrastructure.Models;
using FitnessViewer.Infrastructure.Models.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Repository
{
    public class AnalysisRepository
    {
        private ApplicationDb _context;

        public AnalysisRepository(ApplicationDb context)
        {
            _context = context;
        }

        #region peaks

        public void AddPeak(long activityId, PeakStreamType type, List<ActivityPeakDetail> peaks)
        {
            var existingPeaks = _context.ActivityPeak.Where(a => a.ActivityId == activityId && a.StreamType == type).ToList();
            if (existingPeaks.Count > 0)
                _context.ActivityPeak.RemoveRange(existingPeaks);

            var existingPeakDetail = _context.ActivityPeakDetail.Where(a => a.ActivityId == activityId && a.StreamType == type).ToList();
            if (existingPeakDetail.Count > 0)
                _context.ActivityPeakDetail.RemoveRange(existingPeakDetail);

            ActivityPeaks stravaPeak = new ActivityPeaks() { ActivityId = activityId, StreamType = type };

            foreach (ActivityPeakDetail d in peaks)
            {
                switch (d.Duration)
                {
                    case 5: { stravaPeak.Peak5 = d.Value; break; }
                    case 10: { stravaPeak.Peak10 = d.Value; break; }
                    case 30: { stravaPeak.Peak30 = d.Value; break; }
                    case 60: { stravaPeak.Peak60 = d.Value; break; }
                    case 120: { stravaPeak.Peak120 = d.Value; break; }
                    case 300: { stravaPeak.Peak300 = d.Value; break; }
                    case 360: { stravaPeak.Peak360 = d.Value; break; }
                    case 600: { stravaPeak.Peak600 = d.Value; break; }
                    case 720: { stravaPeak.Peak720 = d.Value; break; }
                    case 1200: { stravaPeak.Peak1200 = d.Value; break; }
                    case 1800: { stravaPeak.Peak1800 = d.Value; break; }
                    case 3600: { stravaPeak.Peak3600 = d.Value; break; }
                    case int.MaxValue: { stravaPeak.PeakDuration = d.Value; break; }
                }

                _context.ActivityPeakDetail.Add(d);
            }

            _context.ActivityPeak.Add(stravaPeak);
        }
        /// <summary>
        /// Return Peak information for common time duration
        /// </summary>
        /// <param name="userId">Indentity</param>
        /// <param name="type">Stream Type to analyse</param>
        /// <returns></returns>
        public IEnumerable<AthletePeaksDto> GetPeaks(string userId, PeakStreamType type)
        {
            var peaks = _context.ActivityPeak
                  .Where(p => p.Activity.Athlete.UserId == userId && p.StreamType == type)
                  .Include(p => p.Activity);

            List<AthletePeaksDto> ap = new List<AthletePeaksDto>();
            ap.Add(ExtractPeaksByDays(type, peaks, 7));
            ap.Add(ExtractPeaksByDays(type, peaks, 30));
            ap.Add(ExtractPeaksByDays(type, peaks, 90));
            ap.Add(ExtractPeaksByDays(type, peaks, 365));
            ap.Add(ExtractPeaksByDays(type, peaks, int.MaxValue));
            return ap;
        }

        private static AthletePeaksDto ExtractPeaksByDays(PeakStreamType type, IQueryable<ActivityPeaks> peaks, int days)
        {
            // days=int.maxvalue is used for earlist date
            DateTime earliestDate = days == int.MaxValue ? DateTime.MinValue : DateTime.Now.AddDays(days * -1);

            AthletePeaksDto ap = new AthletePeaksDto();
            ap.PeakType = type;
            ap.Days = days;

            ap.Seconds5 = peaks.Where(p => p.Activity.StartDateLocal >= earliestDate)
                                .OrderByDescending(p => p.Peak5)
                                .Select(p => new AthletePeaksDto.AthletePeaksDetails() { Peak = p.Peak5, ActivityId = p.ActivityId, Description = p.Activity.Name })
                                .FirstOrDefault();

            ap.Minute1 = peaks.Where(p => p.Activity.StartDateLocal >= earliestDate)
                                .OrderByDescending(p => p.Peak60)
                                .Select(p => new AthletePeaksDto.AthletePeaksDetails() { Peak = p.Peak60, ActivityId = p.ActivityId, Description = p.Activity.Name })
                                .FirstOrDefault();

            ap.Minute5 = peaks.Where(p => p.Activity.StartDateLocal >= earliestDate)
                                .OrderByDescending(p => p.Peak300)
                                .Select(p => new AthletePeaksDto.AthletePeaksDetails() { Peak = p.Peak300, ActivityId = p.ActivityId, Description = p.Activity.Name })
                                .FirstOrDefault();

            ap.Minute20 = peaks.Where(p => p.Activity.StartDateLocal >= earliestDate)
                                .OrderByDescending(p => p.Peak1200)
                                .Select(p => new AthletePeaksDto.AthletePeaksDetails() { Peak = p.Peak1200, ActivityId = p.ActivityId, Description = p.Activity.Name })
                                .FirstOrDefault();

            ap.Minute60 = peaks.Where(p => p.Activity.StartDateLocal >= earliestDate)
                                .OrderByDescending(p => p.Peak3600)
                                .Select(p => new AthletePeaksDto.AthletePeaksDetails() { Peak = p.Peak3600, ActivityId = p.ActivityId, Description = p.Activity.Name })
                                .FirstOrDefault();

            return ap;
        }

        #endregion

    }
}
