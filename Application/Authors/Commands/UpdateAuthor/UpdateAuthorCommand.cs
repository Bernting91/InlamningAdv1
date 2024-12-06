using Domain;
using MediatR;

namespace Application.Authors.Commands.UpdateAuthor
{
    public record UpdateAuthorCommand(Guid Id, string Name) : IRequest<OperationResult<Author?>>;
}
