using CSharpWebProject.Data;
using CSharpWebProject.Models;
using CSharpWebProject.Models.EntityModels;
using CSharpWebProject.Services;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpWebProject.Tests.Services
{
    public class CompetitionsServiceTests
    {
        private ApplicationDbContext dbContext;
        private CompetitionsService competitionsService;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
             .UseInMemoryDatabase(Guid.NewGuid().ToString())
             .Options;

            this.dbContext = new ApplicationDbContext(options);
            this.competitionsService = new CompetitionsService(dbContext);

            User user = new User()
            {
                Id = "id",
                UserName = "username"
            };


            this.dbContext.Users.Add(user);
            this.dbContext.SaveChanges();
        }

        [Test]
        public void GetAllOpenCompetitionsReturnsCorrectResult()
        {
            Competition competition = new Competition()
            {
                IsOpen = true
            };
            this.dbContext.Competitions.Add(competition);
            this.dbContext.SaveChanges();

            int openCompetitionsCount = this.competitionsService.GetAllOpenCompetitions().Count;

            Assert.AreEqual(1, openCompetitionsCount);
        }

        [Test]
        public void GetAllClosedCompetitionsReturnsCorrectResult()
        {
            Competition competition = new Competition()
            {
                IsOpen = false
            };
            this.dbContext.Competitions.Add(competition);
            this.dbContext.SaveChanges();

            int openCompetitionsCount = this.competitionsService.GetAllClosedCompetitions().Count;

            Assert.AreEqual(1, openCompetitionsCount);
        }

        [Test]
        public void GetCompetitionByIdReturnsCorrectResult()
        {
            int competitionId = 1;
            Competition competition = new Competition()
            {
                Id = competitionId
            };
            this.dbContext.Competitions.Add(competition);
            this.dbContext.SaveChanges();

            Competition result = this.competitionsService.GetCompetitionById(competitionId);

            Assert.AreEqual(competition, result);
        }

        [Test]
        public void GetCompetitionByIdReturnsNullWhenCompetitionDoesntExist()
        {
            int competitionId = 1;
            Competition competition = new Competition()
            {
                Id = competitionId
            };
            this.dbContext.Competitions.Add(competition);
            this.dbContext.SaveChanges();

            Competition result = this.competitionsService.GetCompetitionById(15);

            Assert.AreEqual(null, result);
        }

        [Test]
        public void GetCompetitionCompetitorsReturnsCorrectResult()
        {
            int competitionId = 1;
            Competition competition = new Competition()
            {
                Id = competitionId,
                Competitors = new List<Competitor>()
                {
                    new Competitor()
                    {

                    }
                }
            };

            this.dbContext.Competitions.Add(competition);
            this.dbContext.SaveChanges();

            int competitorsCount = this.competitionsService.GetCompetitionCompetitors(competitionId).Count;
            Assert.AreEqual(1, competitorsCount);
        }

        [Test]
        public void CreatecompetitionCreatesCompeititon()
        {
            int competitionId = 1;
            Competition competition = new Competition()
            {
                Id = competitionId,
            };

            this.competitionsService.CreateCompetition(competition);

            int competitionsCount = this.dbContext.Competitions.Count();
            Assert.AreEqual(1, competitionsCount);
        }

        [Test]
        public void GetCompetitionByName()
        {
            string competitionName = "name";
            Competition competition = new Competition()
            {
                Name = competitionName
            };

            this.dbContext.Competitions.Add(competition);
            this.dbContext.SaveChanges();

            Competition result = this.competitionsService.GetCompetitionByName(competitionName);


            Assert.AreEqual(competition, result);
        }

        [Test]
        public void CloseCompetitionActuallyClosesCompetition()
        {
            int competitionId = 1;
            Competition competition = new Competition()
            {
                Id = competitionId,
            };

            this.dbContext.Competitions.Add(competition);
            this.dbContext.SaveChanges();

            this.competitionsService.CloseCompetition(competitionId);

            int closedCompetitionsCount = this.dbContext.Competitions.Where(c => !c.IsOpen).Count();
            Assert.AreEqual(1, closedCompetitionsCount);
        }

        [Test]
        public void JoinUserActuallyAddsTheUser()
        {
            int competitionId = 1;
            User user = new User()
            {
                Id = "userid"
            };
            Competition competition = new Competition()
            {
                Id = 1
            };

            this.dbContext.Competitions.Add(competition);
            this.dbContext.SaveChanges();

            this.competitionsService.JoinUser(competitionId, user);

            int competitors = this.dbContext.Competitions.FirstOrDefault(c => c.Id == competitionId).Competitors.Count;
            Assert.AreEqual(1, competitors);
        }

        [Test]
        public void JoinUserReturnsTrue()
        {
            int competitionId = 1;
            User user = new User()
            {
                Id = "userid"
            };

            Competition competition = new Competition()
            {
                Id = 1
            };

            this.dbContext.Users.Add(user);
            this.dbContext.Competitions.Add(competition);
            this.dbContext.SaveChanges();

            bool result=  this.competitionsService.JoinUser(competitionId, user);
            Assert.AreEqual(true, result);
        }

        [Test]
        public void JoinUserReturnsFalseIfCompetitionIsInvalid()
        {
            User user = new User()
            {
                Id = "userid"
            };

            Competition competition = new Competition()
            {
                Id = 1
            };

            this.dbContext.Users.Add(user);
            this.dbContext.Competitions.Add(competition);
            this.dbContext.SaveChanges();

            bool result = this.competitionsService.JoinUser(11555, user);
            Assert.AreEqual(false, result);
        }


        [Test]
        public void JoinUserReturnsFalseIfUserIsNotInTheDatabase()
        {
            User user = new User()
            {
                Id = "userid"
            };

            Competition competition = new Competition()
            {
                Id = 1
            };
            this.dbContext.Competitions.Add(competition);
            this.dbContext.SaveChanges();

            bool result = this.competitionsService.JoinUser(11555, user);
            Assert.AreEqual(false, result);
        }

        [Test]
        public void RemoveUserActuallyRemovesUser()
        {
            int competitonId = 1;
            User user = new User()
            {
                Id = "userid"
            };

            Competition competition = new Competition()
            {
                Id = competitonId
            };

            this.dbContext.Users.Add(user);
            this.dbContext.Competitions.Add(competition);
            this.dbContext.SaveChanges();

            this.competitionsService.JoinUser(competitonId, user);
            this.competitionsService.RemoveUser(competitonId, user);

            int competitors = this.dbContext.Competitions.FirstOrDefault(c => c.Id == competitonId).Competitors.Count;
            Assert.AreEqual(0, competitors);
        }

        [Test]
        public void RemoveUserReturnsFalseIfUserIsNull()
        {
            int competitonId = 1;
            User user = null;

            Competition competition = new Competition()
            {
                Id = competitonId
            };

            this.dbContext.Competitions.Add(competition);
            this.dbContext.SaveChanges();

            bool result = this.competitionsService.RemoveUser(competitonId, user);
           
            Assert.AreEqual(false, result);
        }

        [Test]
        public void RemoveUserReturnsFalseIfCompetitionIsInvalid()
        {
            int competitonId = 1;
            User user = new User()
            {
                Id = "userid"
            };

            Competition competition = new Competition()
            {
                Id = competitonId
            };

            this.dbContext.Competitions.Add(competition);
            this.dbContext.SaveChanges();

            bool result = this.competitionsService.RemoveUser(4489489, user);

            Assert.AreEqual(false, result);
        }

        [Test]
        public void RemoveUserReturnsFalseIfUserIsNotInCompetition()
        {
            int competitonId = 1;
            User user = new User()
            {
                Id = "userid"
            };

            Competition competition = new Competition()
            {
                Id = competitonId
            };

            this.dbContext.Users.Add(user);
            this.dbContext.Competitions.Add(competition);
            this.dbContext.SaveChanges();

            bool result = this.competitionsService.RemoveUser(competitonId, user);

            Assert.AreEqual(false, result);
        }

        [Test]
        public void RemoveUserReturnsTrueWhenUserIsRemoved()
        {
            int competitonId = 1;
            User user = new User()
            {
                Id = "userid"
            };

            Competition competition = new Competition()
            {
                Id = competitonId
            };

            this.dbContext.Users.Add(user);
            this.dbContext.Competitions.Add(competition);
            this.dbContext.SaveChanges();

            this.competitionsService.JoinUser(competitonId, user);
            bool result = this.competitionsService.RemoveUser(competitonId, user);

            Assert.AreEqual(true, result);
        }
    }
}
