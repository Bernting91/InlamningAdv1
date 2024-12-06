using Domain;
using MediatR;

namespace Application.Authors.Queries.GetAuthorByID
{
    public record GetAuthorByIdQuery(Guid Id) : IRequest<Author?>;
}
