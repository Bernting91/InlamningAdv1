using Domain;
using MediatR;

namespace Application.Books.Commands.RemoveBook
{
    public record RemoveBookCommand(Guid Id) : IRequest<OperationResult<Book?>>;
}
