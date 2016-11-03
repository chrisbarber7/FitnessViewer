using FitnessViewer.Infrastructure.enums;
using System;

namespace FitnessViewer.Infrastructure.Models.Dto
{
    public class ZoneValueDto
    {
        public ZoneType ZoneType { get; set; }
        public string ZoneName { get; set; }
        public int StartValue { get; set; }
        public int EndValue { get; set; }
        public int DurationInSeconds { get; set; }

        public TimeSpan Duration
        {
            get
            {
                return TimeSpan.FromSeconds(this.DurationInSeconds);
            }
            private set { }
        }

        public string ZoneLabel
        {
            get
            {
                string suffix = "";
                switch (ZoneType)
                {
                    case ZoneType.BikeHeartRate: { suffix = "bpm"; break; }
                    case ZoneType.RunHeartRate: { suffix = "bpm"; break; }
                    case ZoneType.BikePower: { suffix = "w"; break; }
                    case ZoneType.RunPace: { suffix = "m/mi"; break; }
                }
                if (ZoneType == ZoneType.RunPace)
                {
                    if (this.EndValue != int.MaxValue)
                        return string.Format("{0} ({1}-{2}{3})",
                            this.ZoneName,
                           TimeSpan.FromSeconds(this.StartValue).ToString(),
                          TimeSpan.FromSeconds(this.EndValue).ToString(),
                            suffix);
                    else
                        return string.Format("{0} ({1}{2}+)",
                            this.ZoneName,
                          TimeSpan.FromSeconds(this.StartValue).ToString(),
                            suffix);
                }
                else
                {

                    if (this.EndValue != int.MaxValue)
                        return string.Format("{0} ({1}-{2}{3})",
                            this.ZoneName,
                            this.StartValue.ToString(),
                            this.EndValue.ToString(),
                            suffix);
                    else
                        return string.Format("{0} ({1}{2}+)",
                            this.ZoneName,
                            this.StartValue.ToString(),
                            suffix);
                }
            }
            private set { }
        }

        internal static ZoneValueDto CreateDefault(ZoneType zoneType)
        {
            return new ZoneValueDto()
            {
                ZoneType = zoneType,
                ZoneName = "Default",
                StartValue = 0,
                EndValue = int.MaxValue
            };
        }
    }
}
