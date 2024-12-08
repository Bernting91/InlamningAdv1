using Application.Dtos;
using Application.Users.Commands.AddUser;
using Application.Users.Queries.GetAllUsers;
using Application.Users.Queries.Login;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        internal readonly IMediator _mediator;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IMediator mediator, ILogger<UsersController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("getAllUsers")]
        [SwaggerOperation(Summary = "Get all users", Description = "Retrieves a list of all users.")]
        public async Task<IActionResult> GetAllUsers()
        {
            _logger.LogInformation("Retrieving all users");
            return Ok(await _mediator.Send(new GetAllUsersQuery()));
        }

        [HttpPost]
        [Route("Register")]
        [SwaggerOperation(Summary = "Register a user", Description = "Registers a new user.")]
        public async Task<IActionResult> Register([FromBody] UserDto newUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Registering a new user: {UserName}", newUser.UserName);
            return Ok(await _mediator.Send(new AddNewUserCommand(newUser)));
        }

        [HttpPost]
        [Route("Login")]
        [SwaggerOperation(Summary = "Login",Description = "Logs in a user.")]
        public async Task<IActionResult> Login([FromBody] UserDto userWantToLogin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Logging in user: {UserName}", userWantToLogin.UserName);
            return Ok(await _mediator.Send(new LoginUserQuery(userWantToLogin)));
        }
    }
}