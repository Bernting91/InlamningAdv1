using Domain;
using MediatR;

namespace Application.Authors.Commands.RemoveAuthor
{
    public record RemoveAuthorCommand(Author Author) : IRequest<Author?>;
}
