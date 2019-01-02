using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CSharpWebProject.Models.EntityModels
{
    public class NewsPost
    {
        public int Id { get; set; }

        [Required]
        [MinLength(25)]
        public string Title { get; set; }

        public string AuthorId { get; set; }
        public virtual User Author { get; set; }

        [Required]
        [MinLength(400)]
        public string Content { get; set; }

        public DateTime Date { get; set; }
    }
}
