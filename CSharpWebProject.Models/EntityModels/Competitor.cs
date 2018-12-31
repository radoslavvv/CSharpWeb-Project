using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpWebProject.Models.EntityModels
{
    public class Competitor
    {
        public Competitor()
        {
            this.SolveTimes = new List<CompetiveSolveTime>();
        }

        public int Id { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }

        public virtual List<CompetiveSolveTime> SolveTimes { get; set; }

        public virtual CompetiveSolveTime BestTime { get; set; }

        public int CompetitionId { get; set; }
        public virtual Competition Competition { get; set; }

        public bool SubmittedTimes { get; set; }
    }
}
