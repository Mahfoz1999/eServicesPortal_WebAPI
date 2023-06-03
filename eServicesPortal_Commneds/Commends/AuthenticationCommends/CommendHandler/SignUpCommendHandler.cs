using eServicesPortal_Commends.Commends.AuthenticationCommends.Commend;
using eServicesPortal_Commends.Commends.MailCommends.Commend;
using eServicesPortal_Commneds.Exceptions;
using eServicesPortal_Database.DatabaseConnection;
using eServicesPortal_DTO.Mail;
using eServicesPortal_Models.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Web;

namespace eServicesPortal_Commends.Commends.AuthenticationCommends.CommendHandler;

public class SignUpCommendHandler : IRequestHandler<SignUpCommend, IdentityResult>
{
    private IHttpContextAccessor _httpContextAccessor { get; set; }
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IMediator _mediator;
    private readonly eServicesPortalDbContext Context;
    public SignUpCommendHandler(IHttpContextAccessor httpContextAccessor, IMediator mediator,
        UserManager<User> userManager, eServicesPortalDbContext Context, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _httpContextAccessor = httpContextAccessor;
        _mediator = mediator;
        this.Context = Context;
    }
    public async Task<IdentityResult> Handle(SignUpCommend request, CancellationToken cancellationToken)
    {
        try
        {
            User? user = await _userManager.FindByEmailAsync(request.formDto.Email)!;
            if (user is null)
            {
                user = new()
                {
                    UserName = request.formDto.Email,
                    FullName = request.formDto.FullName,
                    Email = request.formDto.Email,
                    Mobile = request.formDto.Mobile
                };
                var result = await _userManager.CreateAsync(user, request.formDto.Password);
                if (!await _roleManager.RoleExistsAsync(nameof(UserRoles.Admin)))
                    await _roleManager.CreateAsync(new IdentityRole(nameof(UserRoles.Admin)));
                if (!await _roleManager.RoleExistsAsync(nameof(UserRoles.Customer)))
                    await _roleManager.CreateAsync(new IdentityRole(nameof(UserRoles.Customer)));
                if (request.formDto.UserRole == UserRoles.Admin)
                    if (await _roleManager.RoleExistsAsync(nameof(UserRoles.Admin)))
                    {
                        await _userManager.AddToRoleAsync(user, nameof(UserRoles.Admin));
                        user.UserRole = nameof(UserRoles.Admin);
                    }
                if (request.formDto.UserRole == UserRoles.Customer)
                    if (await _roleManager.RoleExistsAsync(nameof(UserRoles.Customer)))
                    {
                        await _userManager.AddToRoleAsync(user, nameof(UserRoles.Customer));
                        user.UserRole = nameof(UserRoles.Customer);
                    }
                await Context.SaveChangesAsync();
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                string codeHtmlVersion = HttpUtility.UrlEncode(token);
                var confirmationLink = "https://" + _httpContextAccessor?.HttpContext?.Request.Host + $"/api/Authentication/ConfirmEmail?token={codeHtmlVersion}&email={user.Email}";
                if (confirmationLink is not null)
                {
                    MailRequest mailRequest = new()
                    {
                        ToEmail = user.Email!,
                        Subject = "Email Confirmation",
                        Body = confirmationLink,
                    };
                    await _mediator.Send(new SendEmailCommend(mailRequest));
                }
                return result;
            }
            else throw new ValidException("User already exists");
        }
        catch (Exception)
        {
            throw;
        }

    }
}
