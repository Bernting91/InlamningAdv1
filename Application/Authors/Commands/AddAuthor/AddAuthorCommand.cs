using Domain;
using MediatR;

namespace Application.Authors.Commands.AddAuthor
{
    public record AddAuthorCommand(Author Author) : IRequest<OperationResult<Author?>>;
}
