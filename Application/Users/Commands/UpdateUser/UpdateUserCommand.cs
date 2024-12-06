using Domain;
using MediatR;

namespace Application.Users.Commands.UpdateUser
{
    public record UpdateUserCommand(int Id, User user) : IRequest<User?>
    {
    }
}
