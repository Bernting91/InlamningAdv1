using Domain;
using MediatR;

namespace Application.Users.Commands.UpdateUser
{
    public record UpdateUserCommand(Guid Id, User user) : IRequest<OperationResult<User?>>
    {
    }
}
