using Domain;
using MediatR;

namespace Application.Books.Queries
{
    public record GetAllBooksQuery : IRequest<IEnumerable<Book>>;
}
