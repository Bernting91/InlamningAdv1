using Domain;
using MediatR;

namespace Application.Books.Commands.RemoveBook
{
    public record RemoveBookCommand(Book Book) : IRequest<Book?>;
}
