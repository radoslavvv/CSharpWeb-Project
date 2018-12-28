namespace CSharpWebProject.Models
{
    using CSharpWebProject.Models.EntityModels;
    using Microsoft.AspNetCore.Identity;
    using System;
    using System.Collections.Generic;

    public class User : IdentityUser
    {
        public User()
        {
            this.SolveTimes = new List<SolveTime>();
            this.Achievements = new List<Achievement>();
        }

        public List<Achievement> Achievements  { get; set; }

        public List<SolveTime> SolveTimes { get; set; }
    }
}
