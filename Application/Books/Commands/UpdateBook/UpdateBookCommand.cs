using Domain;
using MediatR;


namespace Application.Books.Commands.UpdateBook
{
    public record UpdateBookCommand(Book Book) : IRequest<Book?>;
}
