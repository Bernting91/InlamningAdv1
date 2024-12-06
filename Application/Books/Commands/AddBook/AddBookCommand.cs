using Domain;
using MediatR;

namespace Application.Books.Commands.AddBook
{
    public record AddBookCommand(Book Book) : IRequest<OperationResult<Book>>;

}
