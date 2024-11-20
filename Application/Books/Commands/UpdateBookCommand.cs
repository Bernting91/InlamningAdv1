using Domain;
using MediatR;


namespace Application.Books.Commands
{
    public record UpdateBookCommand(Book Book) : IRequest<Book?>;
}
