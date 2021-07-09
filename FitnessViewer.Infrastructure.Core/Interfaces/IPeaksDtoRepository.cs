using System.Collections.Generic;
using FitnessViewer.Infrastructure.Core.enums;
using FitnessViewer.Infrastructure.Core.Models.Dto;

namespace FitnessViewer.Infrastructure.Core.Interfaces
{
    public interface IPeaksDtoRepository
    {
        IEnumerable<PeaksDto> GetPeaks(string userId, PeakStreamType type);
    }
}