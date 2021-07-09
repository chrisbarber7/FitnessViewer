using FitnessViewer.Infrastructure.Core.enums;

namespace FitnessViewer.Infrastructure.Core.Models.Dto
{
    public class ZoneValueBase
    {
        public ZoneType ZoneType { get; set; }
        public string ZoneName { get; set; }
        public int StartValue { get; set; }
        public int EndValue { get; set; }
        public int DurationInSeconds { get; set; }
    }
}
