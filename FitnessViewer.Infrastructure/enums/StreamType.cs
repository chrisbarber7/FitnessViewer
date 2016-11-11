using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.enums
{
    [Flags]
    public enum StreamType
    {
        Time = 1,
        Latitude = 2,
        Longitude = 4,
        Distance = 8,
        Altitude = 16,
        Velocity = 32,
        Heartrate = 64,
        Cadence = 128,
        Watts = 256,
        Temp = 512,
        Moving = 1024,
        Gradient = 2048,
        Pace = 4096
    }
}
