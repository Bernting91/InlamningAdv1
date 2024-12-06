using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;

namespace Application.Authors.Commands.RemoveAuthor
{
    public class RemoveAuthorCommandHandler : IRequestHandler<RemoveAuthorCommand, Author?>
    {
        private readonly IAuthorRepository _authorRepository;

        public RemoveAuthorCommandHandler(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task<Author?> Handle(RemoveAuthorCommand request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
            {
                throw new ArgumentException("Id cannot be empty.", nameof(request.Id));
            }

            var author = await _authorRepository.GetAuthorById(request.Id);
            if (author == null)
            {
                throw new KeyNotFoundException("Author not found.");
            }

            await _authorRepository.DeleteAuthorById(request.Id);
            return author;
        }
    }
}