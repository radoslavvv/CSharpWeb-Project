using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpWebProject.Models.EntityModels
{
    public class UserSolveTime
    {
        public string UserId { get; set; }
        public User User { get; set; }

        public int SolveTimeId { get; set; }
        public SolveTime SolveTime { get; set; }
    }
}
