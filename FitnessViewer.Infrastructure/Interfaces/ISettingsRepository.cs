using System;
using System.Collections.Generic;
using FitnessViewer.Infrastructure.enums;
using FitnessViewer.Infrastructure.Models;

namespace FitnessViewer.Infrastructure.Interfaces
{
    public interface ISettingsRepository
    {
        List<ZoneRange> GetUserZoneRanges(string userId);
        IEnumerable<Zone> GetUserZones(string userId, ZoneType zone);
    }
}