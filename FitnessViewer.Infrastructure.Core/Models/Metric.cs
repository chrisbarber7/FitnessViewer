using FitnessViewer.Infrastructure.Core.Data;
using FitnessViewer.Infrastructure.Core.enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Fitbit.Models;
using FitnessViewer.Infrastructure.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FitnessViewer.Infrastructure.Core.Models
{

    [Index(nameof(UserId), nameof(Recorded), nameof(MetricType), IsUnique = true, Name = "IX_Metric_UserIdRecorded")]
    public class Metric : Entity<int>, IEntity<int>, IUserEntity
    {
   //     public int Id { get; set; }

        [Required]
        [MaxLength(450)]
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

             public MetricType MetricType { get; set; }

        public DateTime Recorded { get; set; }
        public decimal Value { get; set; }

        public bool IsManual { get; set; }

        public static Metric CreateMetric(string userId, MetricType type, DateTime dateTime, decimal value, bool isManual)
        {
            if (type == MetricType.Invalid)
                throw new ArgumentException("Invalid MetricType.");

            Metric m = new Metric();
            m.UserId = userId;
            m.MetricType = type;
            m.Recorded = dateTime;
            m.Value = value;
            m.IsManual = isManual;

            return m;
        }
    }
}
