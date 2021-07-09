using System;
using System.Collections.Generic;
using FitnessViewer.Infrastructure.Core.enums;
using FitnessViewer.Infrastructure.Core.Models;

namespace FitnessViewer.Infrastructure.Core.Interfaces
{
    public interface ISettingsRepository
    {
        List<ZoneRange> GetUserZoneRanges(string userId);
        IEnumerable<Zone> GetUserZones(string userId, ZoneType zone);
        IEnumerable<Zone> GetUserZones(string userId);
    }
}