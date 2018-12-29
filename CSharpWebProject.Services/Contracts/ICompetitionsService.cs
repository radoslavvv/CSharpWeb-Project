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
        void JoinUser(int competitionId, User user);
        void CreateCompetition(Competition competition);
    }
}