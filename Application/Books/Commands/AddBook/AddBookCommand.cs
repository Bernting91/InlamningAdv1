using Domain;
using MediatR;

//TESTARR

namespace Application.Books.Commands.AddBook
{
    public record AddBookCommand(Book Book) : IRequest<Book>;

}
