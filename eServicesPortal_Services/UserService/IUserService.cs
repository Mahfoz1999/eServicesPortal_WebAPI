using eServicesPortal_DTO.User;

namespace eServicesPortal_Services.UserService;

public interface IUserService
{
    public Task<IEnumerable<UserDto>> GetAllUsers();
    public Task<UserDto> GetCurrentUser();
}
