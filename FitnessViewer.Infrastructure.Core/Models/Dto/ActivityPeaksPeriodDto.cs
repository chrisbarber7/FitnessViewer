namespace FitnessViewer.Infrastructure.Core.Models.Dto
{
    public class ActivityPeaksPeriodDto
    {
        public string Period { get; set; }
        public string Label { get; set; }

        public int? Peak5 { get; set; }
        public int? Peak30 { get; set; }
        public int? Peak60 { get; set; }
        public int? Peak300 { get; set; }
        public int? Peak1200 { get; set; }
        public int? Peak3600 { get; set; }
    }
}
