using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.enums;
using FitnessViewer.Infrastructure.Models;
using FitnessViewer.Infrastructure.Models.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

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


        public void AddPeakDetail(ActivityPeakDetail peak)
        {
            var existingPeakDetail = _context.ActivityPeakDetail
                .Where(a => a.ActivityId == peak.ActivityId && a.StreamType == peak.StreamType && a.Duration == peak.Duration)
                .FirstOrDefault();

            if (existingPeakDetail != null)
                existingPeakDetail.Value = peak.Value;
            else
                _context.ActivityPeakDetail.Add(peak);
        }


        public void AddPeaks( List<ActivityPeakDetail> peaks)
        {
            if ((peaks == null) || (peaks.Count == 0))
                    return;

            long activityId = peaks[0].ActivityId;
            PeakStreamType type = peaks[0].StreamType;


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

        internal int[] GetPeakStreamTypeDuration(PeakStreamType type)
        {
            return _context.PeakStreamTypeDuration
                            .Where(p => p.PeakStreamType == type && p.Duration != int.MaxValue)
                            .OrderBy(p => p.Duration)
                            .Select(p => p.Duration)
                            .ToArray();
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

            ap.Seconds5 = peaks.Where(p => p.Activity.Start >= earliestDate)
                                .OrderByDescending(p => p.Peak5)
                                .Select(p => new PeaksDto.PeakDetail() { Peak = p.Peak5, ActivityId = p.ActivityId, Description = p.Activity.Name })
                                .FirstOrDefault();

            ap.Minute1 = peaks.Where(p => p.Activity.Start >= earliestDate)
                                .OrderByDescending(p => p.Peak60)
                                .Select(p => new PeaksDto.PeakDetail() { Peak = p.Peak60, ActivityId = p.ActivityId, Description = p.Activity.Name })
                                .FirstOrDefault();

            ap.Minute5 = peaks.Where(p => p.Activity.Start >= earliestDate)
                                .OrderByDescending(p => p.Peak300)
                                .Select(p => new PeaksDto.PeakDetail() { Peak = p.Peak300, ActivityId = p.ActivityId, Description = p.Activity.Name })
                                .FirstOrDefault();

            ap.Minute20 = peaks.Where(p => p.Activity.Start >= earliestDate)
                                .OrderByDescending(p => p.Peak1200)
                                .Select(p => new PeaksDto.PeakDetail() { Peak = p.Peak1200, ActivityId = p.ActivityId, Description = p.Activity.Name })
                                .FirstOrDefault();

            ap.Minute60 = peaks.Where(p => p.Activity.Start >= earliestDate)
                                .OrderByDescending(p => p.Peak3600)
                                .Select(p => new PeaksDto.PeakDetail() { Peak = p.Peak3600, ActivityId = p.ActivityId, Description = p.Activity.Name })
                                .FirstOrDefault();

            return ap;
        }

        #endregion

    }
}
