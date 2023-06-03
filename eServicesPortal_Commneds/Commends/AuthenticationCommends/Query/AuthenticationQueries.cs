using MediatR;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace eServicesPortal_Commends.Commends.AuthenticationCommends.Query;

public record GetTokenQuery(List<Claim> authClaims) : IRequest<JwtSecurityToken>;
