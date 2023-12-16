using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Features.Users.Commands.CreateUser;
using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker.Enums;
using Dotnet.Homeworks.Mediator;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Dotnet.Homeworks.Features.Users.Queries.GetUser;
using Dotnet.Homeworks.Features.UserManagement.Queries.GetAllUsers;
using Dotnet.Homeworks.Features.Users.Commands.DeleteUser;
using Dotnet.Homeworks.Features.Users.Commands.UpdateUser;
using Dotnet.Homeworks.Features.UserManagement.Commands.DeleteUserByAdmin;

namespace Dotnet.Homeworks.MainProject.Controllers;

[ApiController]
public class UserManagementController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserManagementController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("user")]
    public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserCommand userDto, CancellationToken cancellationToken)
    {
        var creationResult = await _mediator.Send(userDto, cancellationToken);

        if (creationResult.IsFailure)
            return BadRequest(creationResult.Error);

        return Created("users", creationResult.Value.Guid);
    }

    [HttpGet("profile/{guid}")]
    public async Task<IActionResult> GetProfileAsync([FromRoute] Guid guid, CancellationToken cancellationToken) 
    {
        var getUserResult = await _mediator.Send(new GetUserQuery(guid), cancellationToken);

        return getUserResult.IsSuccess
            ? Ok(getUserResult.Value)
            : BadRequest(getUserResult.Error);
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsersAsync(CancellationToken cancellationToken)
    {
        var getAllUsersResult = await _mediator.Send(new GetAllUsersQuery(), cancellationToken);

        return getAllUsersResult.IsSuccess
            ? Ok(getAllUsersResult.Value)
            : BadRequest(getAllUsersResult.Error);
    }

    [HttpDelete("profile/{guid:guid}")]
    public async Task<IActionResult> DeleteProfileAsync([FromRoute] Guid guid, CancellationToken cancellationToken)
    {
        var deleteUserResult = await _mediator.Send(new DeleteUserCommand(guid), cancellationToken);

        return deleteUserResult.IsSuccess
            ? NoContent()
            : BadRequest(deleteUserResult.Error);
    }

    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfileAsync([FromBody] User user, CancellationToken cancellationToken)
    {
        var updateUserResult = await _mediator.Send(new UpdateUserCommand(user), cancellationToken);

        return updateUserResult.IsSuccess
            ? NoContent()
            : BadRequest(updateUserResult.Error);
    }

    [HttpDelete("user/{guid:guid}")]
    public async Task<IActionResult> DeleteUserAsync([FromRoute] Guid guid, CancellationToken cancellationToken)
    {
        var deleteUserByAdminResult = await _mediator.Send(new DeleteUserByAdminCommand(guid), cancellationToken);

        return deleteUserByAdminResult.IsSuccess
            ? NoContent()
            : BadRequest(deleteUserByAdminResult.Error);
    }

    [HttpPost("user/{guid:guid}/login")]
    public async Task<IActionResult> SignInAsync([FromRoute] Guid guid, [FromQuery] bool isAdmin = false)
    {
        var userSearchResult = await _mediator.Send(new GetUserQuery(guid));

        if (userSearchResult.IsFailure)
            return NotFound();

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, guid.ToString()),
            new Claim(ClaimTypes.Role, isAdmin ? Roles.Admin.ToString() : Roles.User.ToString())
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(principal);

        return Ok();
    }

    [HttpPost("user/{guid:guid}/login")]
    public async Task<IActionResult> SignInAsAdminAsync([FromRoute] Guid guid)
    {
        var userSearchResult = await _mediator.Send(new GetUserQuery(guid));

        if (userSearchResult.IsFailure)
            return NotFound();

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, guid.ToString()),
            new Claim(ClaimTypes.Role, Roles.User.ToString())
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(principal);

        return Ok();
    }


    [HttpPost("user/logout")]
    public async Task<IActionResult> SignOutAsync()
    {
        await HttpContext.SignOutAsync();

        return Ok();
    }
}