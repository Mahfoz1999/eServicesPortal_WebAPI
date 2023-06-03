using eServicesPortal_Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eServicesPortal_WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService service;
    public UserController(IUserService service)
    {
        this.service = service;
    }
    [HttpGet]
    [Authorize]
    [Route("[action]")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var result = await service.GetCurrentUser();
        return Ok(result);
    }
}
