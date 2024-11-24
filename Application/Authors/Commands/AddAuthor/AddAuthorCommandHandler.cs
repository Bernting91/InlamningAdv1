using Domain;
using Infrastructure.Database;
using MediatR;

namespace Application.Authors.Commands.AddAuthor
{

public class AddAuthorCommandHandler : IRequestHandler<AddAuthorCommand, Author>
    {
        private readonly FakeDatabase _fakeDatabase;
        public AddAuthorCommandHandler(FakeDatabase fakeDatabase)
        {
            _fakeDatabase = fakeDatabase;
        }
        public Task<Author> Handle(AddAuthorCommand request, CancellationToken cancellationToken)
        {
            if (request.Author == null)
            {
                throw new ArgumentNullException(nameof(request.Author), "Author cannot be null.");
            }
            if (string.IsNullOrWhiteSpace(request.Author.Name))
            {
                throw new ArgumentException("Name cannot be empty.", nameof(request.Author));
            }
            _fakeDatabase.Authors.Add(request.Author);
            return Task.FromResult(request.Author);
        }
    }

}
