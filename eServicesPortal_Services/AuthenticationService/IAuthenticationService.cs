using eServicesPortal_DTO.Auth;
using eServicesPortal_DTO.User;
using Microsoft.AspNetCore.Identity;

namespace eServicesPortal_Services.AuthenticationService;

public interface IAuthenticationService
{
    public Task<IdentityResult> SignUp(UserSignUpDto formDto);
    public Task<TokenModel> LogIn(UserLoginDto model);
}
