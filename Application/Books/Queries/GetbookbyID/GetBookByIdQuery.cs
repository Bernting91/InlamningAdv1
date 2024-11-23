using Domain;
using MediatR;

namespace Application.Books.Queries.GetbookbyID
{
    public record GetBookByIdQuery(int Id) : IRequest<Book?>;
}
