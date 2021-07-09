using Newtonsoft.Json;

namespace Strava.Statistics
{
    /// <summary>
    /// This class represents the datra of all your rides.
    /// </summary>
    public class AllRideTotals
    {
        /// <summary>
        /// The total count of all your rides.
        /// </summary>
        [JsonProperty("count")]
        public int Count { get; set; }

        /// <summary>
        /// The cumulative distance of all your rides.
        /// </summary>
        [JsonProperty("distance")]
        public int Distance { get; set; }

        /// <summary>
        /// The cumulative moving time of all your rides.
        /// </summary>
        [JsonProperty("moving_time")]
        public int MovingTime { get; set; }

        /// <summary>
        /// The cumulative elapsed time of all your rides.
        /// </summary>
        [JsonProperty("elapsed_time")]
        public int ElapsedTime { get; set; }

        /// <summary>
        /// The cumulative elevation gain of all your rides.
        /// </summary>
        [JsonProperty("elevation_gain")]
        public int ElevationGain { get; set; }
    }
}