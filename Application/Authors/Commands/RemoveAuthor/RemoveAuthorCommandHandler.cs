using Domain;
using Infrastructure.Database;
using MediatR;

namespace Application.Authors.Commands.RemoveAuthor
{
    public class RemoveAuthorCommandHandler : IRequestHandler<RemoveAuthorCommand, Author>
    {
        private readonly FakeDatabase _fakeDatabase;
        public RemoveAuthorCommandHandler(FakeDatabase fakeDatabase)
        {
            _fakeDatabase = fakeDatabase;
        }
        public Task<Author?> Handle(RemoveAuthorCommand request, CancellationToken cancellationToken)
        {
            if (request.Author == null)
            {
                throw new ArgumentNullException(nameof(request.Author), "Author cannot be null");
            }

            if (_fakeDatabase.Authors.Remove(request.Author))
            {
                return Task.FromResult<Author?>(request.Author);
            }
            else
            {
                return Task.FromResult<Author?>(null);
            }
        }
    }
}
