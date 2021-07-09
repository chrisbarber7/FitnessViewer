using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Core.enums
{
    /// <summary>
    /// System from which download will occur
    /// </summary>
    public enum DownloadType
    {
        Invalid =0,
        Strava=1,
        Fitbit=2,
        CalculateActivityStats=3
    }
}
