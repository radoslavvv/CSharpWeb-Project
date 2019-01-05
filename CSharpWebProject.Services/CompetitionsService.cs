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
            Competition competiton = this.Context
                .Competitions
                .FirstOrDefault(c => c.Id == id);

            return competiton;
        }


        public List<Competitor> GetCompetitionCompetitors(int id)
        {
            Competition competition = this.Context
                .Competitions
                .FirstOrDefault(c => c.Id == id);

            if (competition == null)
            {
                return new List<Competitor>();
            }

            List<Competitor> competitors = competition
                .Competitors
                .ToList();

            return competitors;
        }


        public bool JoinUser(int competitionId, User user)
        {
            Competition competition = this.Context
                .Competitions
                .FirstOrDefault(c => c.Id == competitionId);

            if (competition == null)
            {
                return false;
            }

            competition.Competitors
                .Add(new Competitor()
                {
                    UserId = user.Id,
                    CompetitionId = competitionId
                });

            this.Context.SaveChanges();

            User dbUser = this.Context
                .Users
                .FirstOrDefault(u => u.UserName == user.UserName);

            if (dbUser == null)
            {
                return false;
            }

            dbUser.Competitions.Add(competition);
            this.Context.SaveChanges();

            return true;
        }

        public void CreateCompetition(Competition competition)
        {
            if (competition != null)
            {
                this.Context.Competitions.Add(competition);
                this.Context.SaveChanges();
            }
        }

        public bool RemoveUser(int id, User user)
        {
            Competition competition = this
                .Context
                .Competitions
                .FirstOrDefault(c => c.Id == id);

            if(competition == null || user == null)
            {
                return false;
            }

            Competitor competitor = competition
                .Competitors
                .FirstOrDefault(c => c.User.UserName == user.UserName);

            if(competitor == null)
            {
                return false;
            }

            competition.Competitors.Remove(competitor);
            user.Competitions.Remove(competition);
            this.Context.SaveChanges();

            return true;
        }


        public bool AddTimes(List<CompetiveSolveTime> solveTimes, string userId, int competitionId)
        {
            Competition competition = this.Context
                .Competitions
                .FirstOrDefault(c => c.Id == competitionId);

            if(competition == null)
            {
                return false;
            }

            Competitor competitor = competition
                .Competitors
                .FirstOrDefault(c => c.UserId == userId);

            if(competitor == null)
            {
                return false;
            }

            CompetiveSolveTime bestTime = solveTimes
                .OrderBy(t => t.Result.TimeOfDay)
                .FirstOrDefault();

            if(bestTime == null)
            {
                return false;
            }

            competitor.SolveTimes = solveTimes;
            competitor.BestTime = bestTime;
            competitor.SubmittedTimes = true;

            this.Context.SaveChanges();
            return true;
        }

        public Competition GetCompetitionByName(string competitionName)
        {
            return this.Context
                .Competitions
                .FirstOrDefault(c => c.Name == competitionName);
        }

        public bool CloseCompetition(int id)
        {
            Competition competition = this.GetCompetitionById(id);

            if(competition == null)
            {
                return false;
            }

            competition.IsOpen = false;
            this.Context.SaveChanges();
            return true;
        }
    }
}
