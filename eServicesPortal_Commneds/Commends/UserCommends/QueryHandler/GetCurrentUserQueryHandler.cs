using eServicesPortal_Commends.Commends.UserCommends.Query;
using eServicesPortal_Models.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace eServicesPortal_Commends.Commends.UserCommends.QueryHandler;

public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, User>
{
    private readonly UserManager<User> _userManager;
    private IHttpContextAccessor _httpContextAccessor { get; set; }
    public GetCurrentUserQueryHandler(UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<User> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var _httpcontext = _httpContextAccessor.HttpContext;
        if (_httpcontext != null
                && _httpcontext.User != null
                && _httpcontext.User.Identity != null
                && _httpcontext.User.Identity.IsAuthenticated)
        {
            User? user = await _userManager.GetUserAsync(_httpcontext.User)!;
            return user!;
        }
        else
            throw new UnauthorizedAccessException("User is not Authenticated");
    }
}
