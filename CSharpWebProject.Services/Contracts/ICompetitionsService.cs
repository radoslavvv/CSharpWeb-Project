using System.Collections.Generic;
using CSharpWebProject.Models;
using CSharpWebProject.Models.EntityModels;

namespace CSharpWebProject.Services
{
    public interface ICompetitionsService
    {
        ICollection<Competition> GetAllClosedCompetitions();
        ICollection<Competition> GetAllOpenCompetitions();
        Competition GetCompetitionById(int id);
        List<Competitor> GetCompetitionCompetitors(int id);
        bool JoinUser(int competitionId, User user);
        void CreateCompetition(Competition competition);
        bool RemoveUser(int id, User user);
        bool AddTimes(List<CompetiveSolveTime> solveTimes, string userId, int competitionId);
        Competition GetCompetitionByName(string competitionName);
        bool CloseCompetition(int id);
    }
}