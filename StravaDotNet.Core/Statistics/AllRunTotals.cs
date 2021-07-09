using Newtonsoft.Json;

namespace Strava.Statistics
{
    /// <summary>
    /// This class represents the data of all your runs.
    /// </summary>
    public class AllRunTotals
    {
        /// <summary>
        /// The number of total runs.
        /// </summary>
        [JsonProperty("count")]
        public int Count { get; set; }

        /// <summary>
        /// The cumulative distance of all your runs.
        /// </summary>
        [JsonProperty("distance")]        
        public int Distance { get; set; }

        /// <summary>
        /// The cumulative moving time of all your runs.
        /// </summary>
        [JsonProperty("moving_time")]
        public int MovingTime { get; set; }

        /// <summary>
        /// The cumulative elapsed time of all your runs.
        /// </summary>
        [JsonProperty("elapsed_time")]
        public int ElapsedTime { get; set; }

        /// <summary>
        /// The cumulative elevation gain of all your runs.
        /// </summary>
        [JsonProperty("elevation_gain")]
        public int ElevationGain { get; set; }
    }
}