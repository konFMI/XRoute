using System.ComponentModel.DataAnnotations;

namespace XRoute.ViewModels
{
    public class RegisterNormalUserViewModel
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(100, ErrorMessage = "Username cannot be longer than 100 characters")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [StringLength(100, ErrorMessage = "Email cannot be longer than 100 characters")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
        public string Password { get; set; }

        public string? ProfilePictureUrl { get; set; }

        public RegisterNormalUserViewModel()
        {
            Username = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
        }
    }
}
