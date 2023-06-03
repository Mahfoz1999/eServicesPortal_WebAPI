namespace eServicesPortal_DTO.User;

public record UserDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Mobile { get; set; }
    public string UserRole { get; set; }
}
