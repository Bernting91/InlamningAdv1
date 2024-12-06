using Domain;
using MediatR;


namespace Application.Books.Commands.UpdateBook
{
    public record UpdateBookCommand(Guid id, Book Book) : IRequest<OperationResult<Book?>>;
}
