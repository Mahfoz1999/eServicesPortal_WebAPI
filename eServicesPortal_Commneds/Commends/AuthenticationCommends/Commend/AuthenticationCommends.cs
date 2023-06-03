using eServicesPortal_DTO.Auth;
using eServicesPortal_DTO.User;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace eServicesPortal_Commends.Commends.AuthenticationCommends.Commend;

public record SignUpCommend(UserSignUpDto formDto) : IRequest<IdentityResult>;
public record LogInCommend(UserLoginDto model) : IRequest<TokenModel>;
public record UpdateFullNameCommend(string fullName) : IRequest<IdentityResult>;
public record UpdateEmailCommend(string email) : IRequest<IdentityResult>;
public record UpdateMobileCommend(string mobile) : IRequest<IdentityResult>;
