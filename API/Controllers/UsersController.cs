using Application.Dtos;
using Application.Users.Commands.AddUser;
using Application.Users.Queries.GetAllUsers;
using Application.Users.Queries.Login;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        internal readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("getAllUsers")]
        [SwaggerOperation(Description = "Retrieves a list of all users.")]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await _mediator.Send(new GetAllUsersQuery()));
        }

        [HttpPost]
        [Route("Register")]
        [SwaggerOperation(Description = "Registers a new user.")]
        public async Task<IActionResult> Register([FromBody] UserDto newUser)
        {
            return Ok(await _mediator.Send(new AddNewUserCommand(newUser)));
        }

        [HttpPost]
        [Route("Login")]
        [SwaggerOperation(Description = "Logs in a user.")]
        public async Task<IActionResult> Login([FromBody] UserDto userWantToLogin)
        {
            return Ok(await _mediator.Send(new LoginUserQuery(userWantToLogin)));
        }
    }
}