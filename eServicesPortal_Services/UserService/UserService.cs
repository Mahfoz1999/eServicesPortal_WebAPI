using eServicesPortal_Commends.Commends.UserCommends.Query;
using eServicesPortal_DTO.User;
using eServicesPortal_Models.Models;
using MediatR;

namespace eServicesPortal_Services.UserService;

public class UserService : IUserService
{
    private readonly IMediator _mediator;
    public UserService(IMediator mediator)
    {
        _mediator = mediator;
    }
    public Task<IEnumerable<UserDto>> GetAllUsers()
    {
        throw new NotImplementedException();
    }

    public async Task<UserDto> GetCurrentUser()
    {
        User user = await _mediator.Send(new GetCurrentUserQuery());
        UserDto userDto = new()
        {
            Id = Guid.Parse(user.Id),
            FullName = user.FullName,
            Email = user.Email,
            Mobile = user.Mobile,
            UserRole = user.UserRole
        };
        return userDto;
    }
}
