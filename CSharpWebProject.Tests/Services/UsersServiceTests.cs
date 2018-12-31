using CSharpWebProject.Data;
using CSharpWebProject.Models;
using CSharpWebProject.Services;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpWebProject.Tests.Services
{
    public class UsersServiceTests
    {
        [Test]
        public void GetUserIdByUsernameReturnsRightId()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString())
               .Options;

            ApplicationDbContext dbContext = new ApplicationDbContext(options);
            string username = "Username";
            string id = "SomeId";

            User user = new User()
            {
                Id = id,
                UserName = username
            };
            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            UsersService usersService = new UsersService(dbContext);
            string resultId = usersService.GetUserIdByUsername(username);

            Assert.AreEqual(id, resultId);
        }

        [Test]
        public void GetUserIdByUsernameReturnsNullWhenUserNotFound()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString())
               .Options;

            ApplicationDbContext dbContext = new ApplicationDbContext(options);
            string username = "Username";
            string id = "SomeId";

            User user = new User()
            {
                Id = id,
                UserName = username
            };
            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            UsersService usersService = new UsersService(dbContext);
            string resultId = usersService.GetUserIdByUsername("Username1");

            Assert.IsNull(resultId);
        }


        [Test]
        public void GetUserByUserNameReturnsRightResult()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString())
               .Options;

            ApplicationDbContext dbContext = new ApplicationDbContext(options);
            string username = "Username";
            string id = "SomeId";

            User user = new User()
            {
                Id = id,
                UserName = username
            };
            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            UsersService usersService = new UsersService(dbContext);
            User resultUser = usersService.GetUserByUsername("Username");

            Assert.AreEqual(user, user);
        }

        [Test]
        public void GetUserByUserNameReturnNullWhenUserDoesntExist()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString())
               .Options;

            ApplicationDbContext dbContext = new ApplicationDbContext(options);
            string username = "Username";
            string id = "SomeId";

            User user = new User()
            {
                Id = id,
                UserName = username
            };
            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            UsersService usersService = new UsersService(dbContext);
            User resultUser = usersService.GetUserByUsername("Username1");

            Assert.IsNull(resultUser);
        }

    }
}
