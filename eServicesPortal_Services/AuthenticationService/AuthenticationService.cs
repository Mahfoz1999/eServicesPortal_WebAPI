using eServicesPortal_Commends.Commends.AuthenticationCommends.Commend;
using eServicesPortal_DTO.Auth;
using eServicesPortal_DTO.User;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace eServicesPortal_Services.AuthenticationService;

public class AuthenticationService : IAuthenticationService
{
    private readonly IMediator _mediator;
    public AuthenticationService(IMediator mediator)
    {
        _mediator = mediator;
    }
    public async Task<TokenModel> LogIn(UserLoginDto model)
    {
        TokenModel tokenModel = await _mediator.Send(new LogInCommend(model));
        return tokenModel;
    }

    public async Task<IdentityResult> SignUp(UserSignUpDto formDto)
    {
        IdentityResult identityResult = await _mediator.Send(new SignUpCommend(formDto));
        return identityResult;
    }
}
