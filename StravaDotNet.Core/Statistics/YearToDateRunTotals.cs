using Newtonsoft.Json;

namespace Strava.Statistics
{
    /// <summary>
    /// This class contains all the statistics for running in this year.
    /// </summary>
    public class YearToDateRunTotals
    {
        /// <summary>
        /// The number of runs.
        /// </summary>
        [JsonProperty("count")]        
        public int Count { get; set; }

        /// <summary>
        /// The Distance.
        /// </summary>
        [JsonProperty("distance")]
        public int Distance { get; set; }

        /// <summary>
        /// The total time you were moving.
        /// </summary>
        [JsonProperty("moving_time")]
        public int MovingTime { get; set; }

        /// <summary>
        /// The total elapsed time.
        /// </summary>
        [JsonProperty("elapsed_time")]
        public int ElapsedTime { get; set; }

        /// <summary>
        /// The total elevation gain.
        /// </summary>
        [JsonProperty("elevation_gain")]
        public int ElevationGain { get; set; }
    }
}