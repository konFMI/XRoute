using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace XRoute.Models
{
    public class Route
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Route name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Start location is required")]
        public string StartLocation { get; set; }

        [Required(ErrorMessage = "End location is required")]
        public string EndLocation { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public string Category { get; set; } // Public Transport or Intercity Transportation

        [Required(ErrorMessage = "Representative username is required")]
        public string RepresentativeUsername { get; set; }

        public DateTime DateAdded { get; set; }

        // Add a default constructor to initialize non-nullable properties
        public Route()
        {
            Name = string.Empty;
            StartLocation = string.Empty;
            EndLocation = string.Empty;
            Category = string.Empty;
            RepresentativeUsername = string.Empty;
        }
    }
}
