using Microsoft.AspNetCore.Identity;

namespace eServicesPortal_Models.Models;

public class User : IdentityUser
{
    public required string FullName { get; set; }
    public required string Mobile { get; set; }
    public string UserRole { get; set; } = string.Empty;
}
