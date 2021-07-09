using FitnessViewer.Infrastructure.Core.enums;
using FitnessViewer.Infrastructure.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Core.Models.Dto
{
    public class MinMaxAve
    {
        public MinMaxAve(StreamType streamType)
        {
            StreamType = streamType;
            HasStream = false;
            Suffix = StreamTypeHelper.Units(streamType);
            Name = StreamTypeHelper.Name(streamType);
            Priority = StreamTypeHelper.Priority(streamType);
        }

        internal decimal Min { get; set; }
        internal decimal Max { get; set; }
        internal decimal Ave { get; set; }
        public string Suffix { get; set; }
        public bool HasStream { get; set; }
        public StreamType StreamType { get; set; }
        public string Name { get; set; }

        internal byte Priority { get; set; }

        public string GetMinimum()
        {
            return StreamTypeHelper.ConvertToUserUnits(StreamType, Min);
        }

        public string GetMaximum()
        {
            return StreamTypeHelper.ConvertToUserUnits(StreamType, Max);
        }

        public string GetAverage()
        {
            return StreamTypeHelper.ConvertToUserUnits(StreamType, Ave);
        }
    }
}
