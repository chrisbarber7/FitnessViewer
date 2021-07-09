using Newtonsoft.Json;

namespace Strava.Statistics
{
    /// <summary>
    /// This class contains statistics about your recent rides.
    /// </summary>
    public class RecentRideTotals
    {
        /// <summary>
        /// The number of rides.
        /// </summary>
        [JsonProperty("count")]
        public int Count { get; set; }

        /// <summary>
        /// The total distance.
        /// </summary>
        [JsonProperty("distance")]
        public double Distance { get; set; }

        /// <summary>
        /// The total time moved.
        /// </summary>
        [JsonProperty("moving_time")]
        public int MovingTime { get; set; }

        /// <summary>
        /// The total time elapsed.
        /// </summary>
        [JsonProperty("elapsed_time")]
        public int ElapsedTime { get; set; }

        /// <summary>
        /// The total elevation gained.
        /// </summary>
        [JsonProperty("elevation_gain")]
        public double ElevationGain { get; set; }

        /// <summary>
        /// The number of achievements.
        /// </summary>
        [JsonProperty("achievement_count")]
        public int AchievementCount { get; set; }
    }
}