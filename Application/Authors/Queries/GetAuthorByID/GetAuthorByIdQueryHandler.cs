using Domain;
using Infrastructure.Database;
using MediatR;

namespace Application.Authors.Queries.GetAuthorByID
{
    public class GetAuthorByIdQueryHandler : IRequestHandler<GetAuthorByIdQuery, Author?>
    {
        private readonly FakeDatabase _fakeDatabase;

        public GetAuthorByIdQueryHandler(FakeDatabase fakeDatabase)
        {
            _fakeDatabase = fakeDatabase;
        }

        public Task<Author?> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
            {
                throw new ArgumentException("Invalid author ID");
            }

            Author? author = _fakeDatabase.Authors.Find(author => author.Id == request.Id);
            return Task.FromResult(author);
        }
    }
}
