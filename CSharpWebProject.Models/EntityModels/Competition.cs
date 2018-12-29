using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpWebProject.Models.EntityModels
{
   public class Competition
    {
        public Competition()
        {
            this.Competitors = new List<Competitor>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public virtual List<Competitor> Competitors { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Sponsor { get; set; }

        public bool IsOpen { get; set; }
    }
}
