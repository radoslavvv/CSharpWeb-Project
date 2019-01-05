using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CSharpWebProject.Models.ViewModels.Competitions
{
    public class CompetitionCreateViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "You must enter a Competition Name!")]
        [MinLength(10, ErrorMessage = "The Name must be at least 10 characters!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "You must enter Competition Description!")]
        [MinLength(55, ErrorMessage = "The Description must be at least 55 characters!")]
        public string Description { get; set; }

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
