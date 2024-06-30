using System;
using System.ComponentModel.DataAnnotations;

namespace XRoute.Models
{
    public class News
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title must be less than 100 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Content is required")]
        public string Content { get; set; }

        public DateTime DatePosted { get; set; }

        public string RepresentativeUsername { get; set; }

        public News()
        {
            Title = string.Empty;
            Content = string.Empty;
            RepresentativeUsername = string.Empty;
        }
    }
}
