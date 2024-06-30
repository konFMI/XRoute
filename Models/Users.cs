using System.ComponentModel.DataAnnotations;

public class User
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Username is required")]
    [StringLength(100, ErrorMessage = "Username cannot be longer than 100 characters")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [StringLength(100, ErrorMessage = "Email cannot be longer than 100 characters")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
    public string PasswordHash { get; set; }

    public string PasswordSalt { get; set; }

    [Required(ErrorMessage = "Role is required")]
    public string Role { get; set; } // Administrator, Normal, Representative

    public string? ProfilePictureUrl { get; set; } // Nullable for optional profile picture

    public string? CompanyName { get; set; } // Nullable for Representatives only

    public string? CompanyAddress { get; set; } // Nullable for Representatives only


    // Add a default constructor to initialize non-nullable properties
    public User()
    {
        Username = string.Empty;
        Email = string.Empty;
        PasswordHash = string.Empty;
        PasswordSalt = string.Empty;
        Role = string.Empty;
    }
}
