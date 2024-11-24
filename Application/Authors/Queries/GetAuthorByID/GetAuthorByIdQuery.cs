using Domain;
using MediatR;

namespace Application.Authors.Queries.GetAuthorByID
{
    public record GetAuthorByIdQuery(int Id) : IRequest<Author?>;
}
