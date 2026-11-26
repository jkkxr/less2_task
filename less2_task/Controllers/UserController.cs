using System.ComponentModel;
using less2_task.Services;
using less2_task.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using less2_task.Models.DTO;

namespace less2_task.Controllers;

[ApiController]
[Route("api/users")]
[Tags("User Management")]
//[Produces("application/json")]
//[Consumes("application/json")]
public class UserController(UserService userService, IMapper mapper): ControllerBase
{
    /// <summary>
    /// Register a new user
    /// </summary>
    /// <param name="request">The registration request containing name, email, and password</param>
    /// <returns>The aunthentication with user name and generated token</returns>
    /// <response code ="200">Returns the registered user's token</response>
    /// <response code="400">If the user already exists or data is invalid</response>
    [HttpPost("register")]
    [EndpointName("RegisterUser")]
    [EndpointDescription("Register a new user")]
    [EndpointSummary("This endpoint allows a new user to register by providing their name, email, and password.")]

    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterUser([FromBody, Description("Register request")] RegisterRequest request)
    {
        var newUser = mapper.Map<User>(request);

        //try
        //{
            var registeredUser = await userService.RegisterUserAsync(newUser);
            var authResponse = mapper.Map<AuthResponse>(registeredUser);
            return Ok(authResponse);
        //}
        //catch (Exception e)
        //{
        //    return BadRequest(e.Message);
        //}
    }

    /// <summary>
    /// Authenticate a user with their email and password
    /// </summary>
    /// <param name="request">The login request containing email and password</param>
    /// <returns>The authentication response with user name and token</returns>
    /// <response code="200">Returns the authenticated user with a token</response>
    /// <response code="401">If the email or password is incorrect</response>
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    [HttpPost("login")]
    public async Task<IActionResult> LoginUser([FromBody] LoginRequest request)
    {
        var authenticatedUser = await userService.AuthenticateUserAsync(request.Email, request.Password);
        if(authenticatedUser == null)
        {
            return Unauthorized();
        }
        var authResponse = mapper.Map<AuthResponse>(authenticatedUser);
        return Ok(authResponse);
    }

    /// <summary>
    /// Get all users
    /// </summary>
    /// <returns>A collection of all users</returns>
    [HttpGet("all")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await userService.GetAllUsersAsync();

        var userViewModels = users.Select(u => mapper.Map<UserViewModel>(u));
        return Ok(userViewModels);
    }


    /// <summary>
    /// Get a user by their email
    /// </summary>
    /// <param name="email">The email of the user to retrieve</param>
    /// <returns>The user with the specified email, or a 404 Not Found if not found</returns>
    [HttpGet("{email}")]
    public async Task<IActionResult> GetUserByEmail(string email)
    {
        var user = await userService.GetUserByEmailIdAsync(email);
        if (user == null)
        {
            return NotFound(); 
        }
        var userViewModel = mapper.Map<UserViewModel>(user);
        return Ok(userViewModel);
    }
}
