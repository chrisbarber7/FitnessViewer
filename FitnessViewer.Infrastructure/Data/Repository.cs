using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitnessViewer.Core;
using FitnessViewer.Core.Interfaces;

namespace FitnessViewer.Infrastructure.Data
{
    public class Repository : IRepository
    {
        ApplicationDb context;

        public Repository()
        {
            context = new ApplicationDb();
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        public void AddAthlete(StravaAthlete a)
        {
            context.StravaAthlete.Add(a);
            context.SaveChanges();
        }

        public void EditAthlete(StravaAthlete a)
        {
            context.SaveChanges();
        }

        public StravaAthlete FindAthleteById(long id)
        {
            var result = (from r in context.StravaAthlete where r.Id == id select r).FirstOrDefault();
            return result;
        }

        public StravaAthlete FindAthleteByUserId(string userId)
        {
            var result = (from r in context.StravaAthlete where r.UserId == userId select r).FirstOrDefault();
            return result;
        }

        public IEnumerable GetAthletes()
        {
            throw new NotImplementedException();
        }

        public void RemoveAthlete(int Id)
        {
            throw new NotImplementedException();
        }
    }
}
