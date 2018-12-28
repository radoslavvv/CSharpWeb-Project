using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpWebProject.Data;
using CSharpWebProject.Models;

namespace CSharpWebProject.Services
{
    public class UsersService : Service, IUsersService
    {
        public UsersService(ApplicationDbContext context) 
            : base(context)
        {
        }

        public string GetUserIdByUsername(string username)
        {
            User user = this.Context.Users.FirstOrDefault(u => u.UserName == username);

            if(user != null)
            {
                string userId = user.Id;

                return userId;
            }

            return null;
        }
    }
}
