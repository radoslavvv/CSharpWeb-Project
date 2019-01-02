using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [Required]
        [MinLength(15)]
        public string Name { get; set; }

        [Required]
        [MinLength(55)]
        public string Description { get; set; }

        public virtual List<Competitor> Competitors { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        [MinLength(15)]
        public string Sponsor { get; set; }

        [Required]
        public bool IsOpen { get; set; }
    }
}
