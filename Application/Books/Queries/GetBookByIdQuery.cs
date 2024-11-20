using Domain;
using MediatR;

namespace Application.Books.Queries
{
    public record GetBookByIdQuery(int Id) : IRequest<Book?>;
}
