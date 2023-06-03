using eServicesPortal_DTO.User;
using eServicesPortal_Models.Models;
using eServicesPortal_Services.AuthenticationService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace eServicesPortal_WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private IAuthenticationService Service { get; set; }
    public AuthenticationController(IAuthenticationService Service, UserManager<User> userManager)
    {
        this.Service = Service;
        _userManager = userManager;
    }
    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> SignUp(UserSignUpDto userDto)
    {
        var result = await Service.SignUp(userDto);
        return result.Succeeded ? StatusCode(201) : new BadRequestObjectResult(result);
    }
    [HttpGet]
    [Route("[action]")]
    public async Task<IActionResult> ConfirmEmail(string token, string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        var result = await _userManager.ConfirmEmailAsync(user!, token);
        return Ok(result.Succeeded ? nameof(ConfirmEmail) : "Error");
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> LogIn([FromBody] UserLoginDto user)
    {
        return Ok(await Service.LogIn(user));

    }
}
