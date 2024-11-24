using Domain;
using Infrastructure.Database;
using MediatR;

namespace Application.Authors.Commands.UpdateAuthor
{
    public class UpdateAuthorCommandHandler : IRequestHandler<UpdateAuthorCommand, Author?>
    {
        private readonly FakeDatabase _fakeDatabase;

        public UpdateAuthorCommandHandler(FakeDatabase fakeDatabase)
        {
            _fakeDatabase = fakeDatabase;
        }

        public Task<Author?> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
        {
            if (request.Author == null)
            {
                throw new ArgumentNullException(nameof(request.Author), "Author cannot be null");
            }

            Author? authorToUpdate = _fakeDatabase.Authors.Find(author => author.Id == request.Author.Id);
            if (authorToUpdate != null)
            {
                authorToUpdate.Name = request.Author.Name;
            }
            return Task.FromResult(authorToUpdate);
        }
    }
}
