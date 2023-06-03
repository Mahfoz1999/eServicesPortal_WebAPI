using eServicesPortal_Commends.Commends.AuthenticationCommends.Commend;
using eServicesPortal_Commends.Commends.UserCommends.Query;
using eServicesPortal_Models.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace eServicesPortal_Commends.Commends.AuthenticationCommends.CommendHandler;

public class UpdateMobileCommendHandler : IRequestHandler<UpdateMobileCommend, IdentityResult>
{
    private readonly UserManager<User> _userManager;
    private readonly IMediator _mediator;
    public UpdateMobileCommendHandler(UserManager<User> userManager, IMediator mediator)
    {
        _userManager = userManager;
        _mediator = mediator;
    }
    public async Task<IdentityResult> Handle(UpdateMobileCommend request, CancellationToken cancellationToken)
    {
        try
        {
            User? user = await _mediator.Send(new GetCurrentUserQuery());
            user.Mobile = request.mobile;
            var result = await _userManager.UpdateAsync(user);
            return result;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
