using eServicesPortal_Commends.Commends.AuthenticationCommends.Query;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace eServicesPortal_Commends.Commends.AuthenticationCommends.QueryHandler;

public class GetTokenQueryHandler : IRequestHandler<GetTokenQuery, JwtSecurityToken>
{
    private readonly IConfiguration configuration;
    public GetTokenQueryHandler(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public async Task<JwtSecurityToken> Handle(GetTokenQuery request, CancellationToken cancellationToken)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtConfig:secret"]!));

        var token = new JwtSecurityToken(
            issuer: configuration["JwtConfig:validIssuer"],
            audience: configuration["JwtConfig:validAudience"],
            expires: DateTime.Now.AddHours(3),
            claims: request.authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

        return token;
    }
}
