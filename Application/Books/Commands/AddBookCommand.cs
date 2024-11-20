using Domain;
using MediatR;

//TEST

namespace Application.Books.Commands
{
    public record AddBookCommand(Book Book) : IRequest<Book>;

}
