using Newtonsoft.Json;

namespace Strava.Statistics
{
    /// <summary>
    /// Run totals of the past four weeks.
    /// </summary>
    public class RecentRunTotals
    {
        /// <summary>
        /// Number of activities.
        /// </summary>
        [JsonProperty("count")]
        public int Count { get; set; }

        /// <summary>
        /// Total distance.
        /// </summary>
        [JsonProperty("distance")]
        public double Distance { get; set; }

        /// <summary>
        /// Moving time.
        /// </summary>
        [JsonProperty("moving_time")]
        public int MovingTime { get; set; }

        /// <summary>
        /// Elapsed time.
        /// </summary>
        [JsonProperty("elapsed_time")]
        public int ElapsedTime { get; set; }

        /// <summary>
        /// Elevation gain in metres.
        /// </summary>
        [JsonProperty("elevation_gain")]
        public double ElevationGain { get; set; }

        /// <summary>
        /// Achievement count.
        /// </summary>
        [JsonProperty("achievement_count")]
        public int AchievementCount { get; set; }
    }
}