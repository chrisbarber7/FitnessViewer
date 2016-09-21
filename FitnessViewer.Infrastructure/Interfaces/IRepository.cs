using FitnessViewer.Infrastructure.Models;
using System.Collections;

namespace FitnessViewer.Inftastructure.Interfaces
{
    public interface IRepository
    {
        void SaveChanges();

        void AddAthlete(StravaAthlete a);
        void EditAthlete(StravaAthlete a);
        void RemoveAthlete(int Id);
        IEnumerable GetAthletes();
        StravaAthlete FindAthleteById(long id);
        StravaAthlete FindAthleteByUserId(string userId);

    }
}
