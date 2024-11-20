using Domain;
using MediatR;

//TESTARR

namespace Application.Books.Commands
{
    public record AddBookCommand(Book Book) : IRequest<Book>;

}
