using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.Models;
using System;
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

        //public void EditAthlete(Athlete a)
        //{
        
            
        //}

        public Athlete FindAthleteById(long id)
        {
            return _context.Athlete.Where(r => r.Id == id).FirstOrDefault();
            //var result = (from r in _context.Athlete where r.Id == id select r).FirstOrDefault();
            //return result;
        }

        public Athlete FindAthleteByUserId(string userId)
        {
            return _context.Athlete.Where(r => r.userId == userId).FirstOrDefault();
            //var result = (from r in _context.Athlete where r.UserId == userId select r).FirstOrDefault();
            //return result;
        }

        //public void RemoveAthlete(int Id)
        //{
        //    throw new NotImplementedException();
        //}
        #endregion
    }
}
