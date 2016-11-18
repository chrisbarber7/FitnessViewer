using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.Models;
using System.Linq;

namespace FitnessViewer.Infrastructure.Repository
{
    public class AthleteRepository
    {
        private ApplicationDb _context;

        public AthleteRepository(ApplicationDb context)
        {
            _context = context;
        }

        #region Athlete
        public void AddAthlete(Athlete a)
        {
            _context.Athlete.Add(a);
        }

        public Athlete FindAthleteById(long id)
        {
            return _context.Athlete.Where(r => r.Id == id).FirstOrDefault();
        }

        public Athlete FindAthleteByUserId(string userId)
        {
            return _context.Athlete.Where(r => r.UserId == userId).FirstOrDefault();
        }
        #endregion
    }
}
