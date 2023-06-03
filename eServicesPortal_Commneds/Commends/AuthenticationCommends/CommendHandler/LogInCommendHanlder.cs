using eServicesPortal_Commends.Commends.AuthenticationCommends.Commend;
using eServicesPortal_Commends.Commends.AuthenticationCommends.Query;
using eServicesPortal_Commneds.Exceptions;
using eServicesPortal_DTO.Auth;
using eServicesPortal_Models.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace eServicesPortal_Commends.Commends.AuthenticationCommends.CommendHandler;

public class LogInCommendHanlder : IRequestHandler<LogInCommend, TokenModel>
{
    private readonly UserManager<User> _userManager;
    private readonly IMediator _mediator;
    public LogInCommendHanlder(UserManager<User> userManager, IMediator mediator)
    {
        _userManager = userManager;
        _mediator = mediator;
    }
    public async Task<TokenModel> Handle(LogInCommend request, CancellationToken cancellationToken)
    {
        try
        {
            User? user = await _userManager.FindByEmailAsync(request.model.Email);
            bool emailStatus = await _userManager.IsEmailConfirmedAsync(user!);
            if (emailStatus == false)
            {
                throw new ValidException("Email is unconfirmed, please confirm it first");
            }
            if (user != null && await _userManager.CheckPasswordAsync(user, request.model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, user.UserName !),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                var token = await _mediator.Send(new GetTokenQuery(authClaims));
                return new TokenModel()
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                };

            }
            throw new UnauthorizedException();
        }
        catch (Exception)
        {
            throw;
        }
    }
}
