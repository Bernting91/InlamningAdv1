using Domain;
using MediatR;

namespace Application.Books.Queries.GetbookbyID
{
    public record GetBookByIdQuery(Guid Id) : IRequest<Book?>;
}
