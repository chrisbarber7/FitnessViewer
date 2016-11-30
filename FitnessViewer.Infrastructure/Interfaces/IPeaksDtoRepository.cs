using System.Collections.Generic;
using FitnessViewer.Infrastructure.enums;
using FitnessViewer.Infrastructure.Models.Dto;

namespace FitnessViewer.Infrastructure.Interfaces
{
    public interface IPeaksDtoRepository
    {
        IEnumerable<PeaksDto> GetPeaks(string userId, PeakStreamType type);
    }
}