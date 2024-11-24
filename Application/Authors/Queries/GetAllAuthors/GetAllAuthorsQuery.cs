using Domain;
using MediatR;

namespace Application.Authors.Queries.GetAllAuthors
{
    public record GetAllAuthorsQuery : IRequest<IEnumerable<Author>>;
}
