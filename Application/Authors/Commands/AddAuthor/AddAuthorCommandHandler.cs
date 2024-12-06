using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;

namespace Application.Authors.Commands.AddAuthor
{
    public class AddAuthorCommandHandler : IRequestHandler<AddAuthorCommand, Author>
    {
        private readonly IAuthorRepository _authorRepository;

        public AddAuthorCommandHandler(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task<Author> Handle(AddAuthorCommand request, CancellationToken cancellationToken)
        {
            if (request.Author == null)
            {
                throw new ArgumentNullException(nameof(request.Author), "Author cannot be null.");
            }
            if (string.IsNullOrWhiteSpace(request.Author.Name))
            {
                throw new ArgumentException("Name cannot be empty.", nameof(request.Author));
            }

            _authorRepository.AddAuthor(request.Author);
            return await Task.FromResult(request.Author);
        }
    }
}