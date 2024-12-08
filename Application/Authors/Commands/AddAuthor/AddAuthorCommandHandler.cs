using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Authors.Commands.AddAuthor
{
    public class AddAuthorCommandHandler : IRequestHandler<AddAuthorCommand, OperationResult<Author>>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly ILogger<AddAuthorCommandHandler> _logger;

        public AddAuthorCommandHandler(IAuthorRepository authorRepository, ILogger<AddAuthorCommandHandler> logger)
        {
            _authorRepository = authorRepository;
            _logger = logger;
        }

        public async Task<OperationResult<Author>> Handle(AddAuthorCommand request, CancellationToken cancellationToken)
        {
            if (request.Author == null)
            {
                _logger.LogWarning("AddAuthorCommand received with null Author.");
                return OperationResult<Author>.FailureResult("Author cannot be null.");
            }
            if (string.IsNullOrWhiteSpace(request.Author.Name))
            {
                _logger.LogWarning("AddAuthorCommand received with empty Author name.");
                return OperationResult<Author>.FailureResult("Author name cannot be empty.");
            }

            var existingAuthor = await _authorRepository.GetAuthorById(request.Author.Id);
            if (existingAuthor != null)
            {
                _logger.LogWarning("AddAuthorCommand received for an existing Author with Id: {AuthorId}", request.Author.Id);
                return OperationResult<Author>.FailureResult("Author already exists.");
            }

            var addedAuthor = await _authorRepository.AddAuthor(request.Author);
            _logger.LogInformation("Author with Id: {AuthorId} added successfully.", addedAuthor.Id);
            return OperationResult<Author>.SuccessResult(addedAuthor);
        }
    }
}