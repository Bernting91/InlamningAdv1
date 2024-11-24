using Domain;
using MediatR;

namespace Application.Authors.Commands.UpdateAuthor
{
    public record UpdateAuthorCommand(Author Author) : IRequest<Author?>;
}
