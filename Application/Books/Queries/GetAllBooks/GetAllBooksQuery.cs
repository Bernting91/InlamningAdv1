using Domain;
using MediatR;

namespace Application.Books.Queries.GetAllBooks
{
    public record GetAllBooksQuery : IRequest<OperationResult<List<Book>>>;
}
