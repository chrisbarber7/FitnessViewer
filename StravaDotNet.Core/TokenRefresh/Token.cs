using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strava.TokenRefresh
{
    public class Token
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public int ExpiresAt { get; set; }
        public int ExpiresIn { get; set; }
    }
}
