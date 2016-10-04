using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.enums
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
        Speed = 5
    }
}


