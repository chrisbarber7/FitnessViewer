using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fitbit.Models;

namespace FitnessViewer.Infrastructure.Models
{
    public class Metric
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(128)]
        [ForeignKey("User")]
        [Index("IX_Metric_UserIdRecorded", 1, IsUnique = true)]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        [Index("IX_Metric_UserIdRecorded", 3, IsUnique = true)]
        public MetricType MetricType { get; set; }

        [Index("IX_Metric_UserIdRecorded", 2, IsUnique = true)]
        public DateTime Recorded { get; set; }
        public decimal Value { get; set; }



        internal static Metric CreateMetric(string userId, MetricType type, DateTime dateTime, decimal value)
        {
            if (type == MetricType.Invalid)
                throw new ArgumentException("Invalid MetricType.");

            Metric m = new Metric();
            m.UserId = userId;
            m.MetricType = type;
            m.Recorded = dateTime;
            m.Value = value;

            return m;
        }
    }
}
