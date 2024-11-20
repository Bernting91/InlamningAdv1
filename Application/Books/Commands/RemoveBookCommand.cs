using Domain;
using MediatR;

namespace Application.Books.Commands
{
    public record RemoveBookCommand(Book Book) : IRequest<Book?>;
}
