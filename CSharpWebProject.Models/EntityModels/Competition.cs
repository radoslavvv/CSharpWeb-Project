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

        [Required(ErrorMessage ="You must enter a Competition Name!")]
        [MinLength(10)]
        public string Name { get; set; }

        [Required(ErrorMessage ="You must enter Competition Description!")]
        [MinLength(55)]
        public string Description { get; set; }

        public virtual List<Competitor> Competitors { get; set; }

        [Required(ErrorMessage = "You must enter Competition Start Date!")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "You must enter Competition End Date!")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "You must enter Competition Sponsor!")]
        public string Sponsor { get; set; }

        [Required]
        public bool IsOpen { get; set; }
    }
}
