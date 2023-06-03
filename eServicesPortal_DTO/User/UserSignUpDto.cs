using eServicesPortal_Models.Models;

namespace eServicesPortal_DTO.User;

public record UserSignUpDto
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Mobile { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public UserRoles UserRole { get; set; }
}
