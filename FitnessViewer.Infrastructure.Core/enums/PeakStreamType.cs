using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Core.enums
{
    /// <summary>
    /// Type of stream (used to set standard duration periods)
    /// </summary>
    public enum PeakStreamType
    {
        Power = 1,
        HeartRate = 2,
        Cadence = 3,
        Lap = 4,
        Speed = 5,
        Elevation = 6,
        PaceByDistance = 7
    }
}


