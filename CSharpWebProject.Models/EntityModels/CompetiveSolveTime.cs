using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpWebProject.Models.EntityModels
{
    public class CompetiveSolveTime
    {
        public int Id { get; set; }

        public DateTime Result { get; set; }

        public string Type { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }

        public int CompetitorId { get; set; }
        public virtual Competitor Competitor { get; set; }

        public DateTime Date { get; set; }
    }
}
