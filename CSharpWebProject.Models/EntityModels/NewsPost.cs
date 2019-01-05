using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CSharpWebProject.Models.EntityModels
{
    public class NewsPost
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "You must enter a Title!")]
        [MinLength(10, ErrorMessage ="The Title must be at least 10 characters!")]
        public string Title { get; set; }

        public string AuthorId { get; set; }
        public virtual User Author { get; set; }

        [Required(ErrorMessage = "You must enter Content!")]
        [MaxLength(400, ErrorMessage = "The max lenght of Content must be 400 characters!")]
        public string Content { get; set; }

        public DateTime Date { get; set; }
    }
}
