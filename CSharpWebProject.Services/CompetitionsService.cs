using CSharpWebProject.Data;
using CSharpWebProject.Models;
using CSharpWebProject.Models.EntityModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpWebProject.Services
{
    public class CompetitionsService : Service, ICompetitionsService
    {
        public CompetitionsService(ApplicationDbContext context) : base(context)
        {
        }

        public ICollection<Competition> GetAllOpenCompetitions()
        {
            List<Competition> openCompetitions = this.Context
                .Competitions
                .Where(c => c.IsOpen)
                .ToList();

            return openCompetitions;
        }

        public ICollection<Competition> GetAllClosedCompetitions()
        {
            List<Competition> closedCompetitions = this.Context
                .Competitions
                .Where(c => !c.IsOpen)
                .ToList();

            return closedCompetitions;
        }

        public Competition GetCompetitionById(int id)
        {
            Competition competiton = this.Context.Competitions.FirstOrDefault(c => c.Id == id);

            return competiton;
        }

        public List<Competitor> GetCompetitionCompetitors(int id)
        {
            List<Competitor> competitors = this.Context
                .Competitions
                .FirstOrDefault(c => c.Id == id)
                .Competitors
                .ToList();

            return competitors;
        }

        public void JoinUser(int competitionId, User user)
        {
            this.Context
                .Competitions
                .FirstOrDefault(c => c.Id == competitionId)
                .Competitors
                .Add(new Competitor()
                {
                    User = user,
                });

            this.Context.SaveChanges();
        }

        public void CreateCompetition(Competition competition)
        {
            this.Context.Competitions.Add(competition);
            this.Context.SaveChanges();
        }
    }
}
