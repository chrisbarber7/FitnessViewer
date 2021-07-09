using System;
using Newtonsoft.Json;

namespace Strava.Activities
{
    /// <summary>
    /// Best effort summaries   
    /// </summary>
    public class BestEffort
    {
        /// <summary>
        /// Reference to activity
        /// </summary>
        [JsonProperty(IsReference = true, PropertyName = "activity")]
        public BestEffortActivity Activity { get; set; }

        /// <summary>
        /// Reference to athelete
        /// </summary>
        [JsonProperty(IsReference = true, PropertyName = "athlete")]
        public BestEffortAthlete Athlete { get; set; }

        /// <summary>
        /// The segment effort's id.
        /// </summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>
        /// Indicates level of detail.
        /// </summary>
        [JsonProperty("resource_state")]
        public int ResourceState { get; set; }

        /// <summary>
        /// Name of effort (400k, 1 mile, etc etc)
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Elapsed time in seconds.
        /// </summary>
        [JsonProperty("elapsed_time")]
        public int ElapsedTime { get; set; }

        /// <summary>
        /// Moving time in seconds.
        /// </summary>
        [JsonProperty("moving_time")]
        public int MovingTime { get; set; }

        /// <summary>
        /// Start date and time.
        /// </summary>
        [JsonProperty("start_date")]
        public String StartDate { get; set; }

        /// <summary>
        /// Local start date and time.
        /// </summary>
        [JsonProperty("start_date_local")]
        public String StartDateLocal { get; set; }

        /// <summary>
        /// Distance in meters.
        /// </summary>
        [JsonProperty("distance")]
        public float Distance { get; set; }

        /// <summary>
        /// The activity stream index of the start of this effort
        /// </summary>
        [JsonProperty("start_index")]
        public int StartIndex { get; set; }

        /// <summary>
        /// The activity stream index of the end of this effort
        /// </summary>
        [JsonProperty("end_index")]
        public int EndIndex { get; set; }
    }
}