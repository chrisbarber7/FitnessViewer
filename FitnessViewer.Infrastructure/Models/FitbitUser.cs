using FitnessViewer.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fitbit.Api.Portable.OAuth2;

namespace FitnessViewer.Infrastructure.Models
{
    public class FitbitUser
    {
        // disabling auto identity column to allow use of fitit user id as the key.
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string FitbitUserId { get; set; }

        [Required]
        [MaxLength(128)]
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public string RefreshToken { get; set; }
        public string Token { get; set; }
        public string TokenType { get; set; }

        internal static FitbitUser Create(string userId, OAuth2AccessToken accessToken)
        {
            FitbitUser u = new FitbitUser();
            u.UserId = userId;
            u.FitbitUserId = accessToken.UserId;
            u.RefreshToken = accessToken.RefreshToken;
            u.Token = accessToken.Token;
            u.TokenType = accessToken.TokenType;

            return u;
        }
    }
}
