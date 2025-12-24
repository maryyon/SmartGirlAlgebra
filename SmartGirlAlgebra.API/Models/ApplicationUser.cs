using Microsoft.AspNetCore.Identity;

namespace SmartGirlAlgebra.API.Models;

public class ApplicationUser : IdentityUser
{
    public string? DisplayName { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation property
    public UserProgress? Progress { get; set; }
}

