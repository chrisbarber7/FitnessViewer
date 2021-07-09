using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Core.enums
{
    public enum SportUnitsDistance
    {
        Invalid = 0,
        Miles = 1,
        KilometerPerHour = 2,
        Meters = 3,
        Yards = 4
    }

    public enum SportUnitsPace
    {
        Invalid = 0,
        MilesPerHour = 1,
        KilometerPerHour = 2,
        MinutePerMile = 3,
        KilometerPerMile = 4,
        MinutePerHundredMeter = 5,
        MinutePerHundredYards = 6
    }

    public enum SportUnitsElevation
    {
        Invalid = 0,
        Feet = 1,
        Meter = 2
    }

}
