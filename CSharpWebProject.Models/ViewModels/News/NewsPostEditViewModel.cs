using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CSharpWebProject.Models.ViewModels
{
    public class NewsPostEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "You must enter a Title!")]
        [MinLength(10)]
        public string Title { get; set; }

        public string AuthorId { get; set; }
        public virtual User Author { get; set; }

        [Required(ErrorMessage = "You must enter Content!")]
        [MaxLength(400)]
        public string Content { get; set; }

        public DateTime Date { get; set; }
    }
}
