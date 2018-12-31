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
            this.Achievements = new HashSet<UserAchievement>();
            this.Competitions = new List<Competition>();
        }

        public virtual ICollection<UserAchievement> Achievements  { get; set; }

        public virtual ICollection<SolveTime> SolveTimes { get; set; }

        public virtual ICollection<Competition> Competitions { get; set; }

    }
}
