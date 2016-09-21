using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using FitnessViewer.Core;

namespace FitnessViewer.Infrastructure.Data
{
    public class ApplicationDb : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDb()
            : base("FitnessViewer", throwIfV1Schema: false)
        {
        }

        public static ApplicationDb Create()
        {
            return new ApplicationDb();
        }

        public DbSet<StravaAthlete> StravaAthlete { get; set; }
    }
}