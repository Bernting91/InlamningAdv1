using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Authors.Commands.RemoveAuthor
{
    public class RemoveAuthorCommandHandler : IRequestHandler<RemoveAuthorCommand, OperationResult<Author?>>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly ILogger<RemoveAuthorCommandHandler> _logger;

        public RemoveAuthorCommandHandler(IAuthorRepository authorRepository, ILogger<RemoveAuthorCommandHandler> logger)
        {
            _authorRepository = authorRepository;
            _logger = logger;
        }

        public async Task<OperationResult<Author?>> Handle(RemoveAuthorCommand request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
            {
                _logger.LogWarning("RemoveAuthorCommand received with empty Id.");
                return OperationResult<Author>.FailureResult("Id cannot be empty.");
            }

            var author = await _authorRepository.GetAuthorById(request.Id);
            if (author == null)
            {
                _logger.LogWarning("RemoveAuthorCommand received for non-existent Author with Id: {AuthorId}", request.Id);
                return OperationResult<Author>.FailureResult("Author not found.");
            }

            await _authorRepository.DeleteAuthorById(request.Id);
            _logger.LogInformation("Author with Id: {AuthorId} removed successfully.", request.Id);
            return OperationResult<Author>.SuccessResult(author);
        }
    }
}