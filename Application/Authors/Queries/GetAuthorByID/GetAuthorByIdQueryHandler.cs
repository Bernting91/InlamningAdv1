using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Authors.Queries.GetAuthorByID
{
    public class GetAuthorByIdQueryHandler : IRequestHandler<GetAuthorByIdQuery, OperationResult<Author?>>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly ILogger<GetAuthorByIdQueryHandler> _logger;

        public GetAuthorByIdQueryHandler(IAuthorRepository authorRepository, ILogger<GetAuthorByIdQueryHandler> logger)
        {
            _authorRepository = authorRepository;
            _logger = logger;
        }

        public async Task<OperationResult<Author?>> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetAuthorByIdQuery for Author Id: {AuthorId}", request.Id);

            if (request.Id == Guid.Empty)
            {
                _logger.LogWarning("GetAuthorByIdQuery received with empty Id.");
                return OperationResult<Author?>.FailureResult("Id cannot be empty.");
            }

            var author = await _authorRepository.GetAuthorById(request.Id);
            if (author == null)
            {
                _logger.LogWarning("GetAuthorByIdQuery received for non-existent Author with Id: {AuthorId}", request.Id);
                return OperationResult<Author?>.FailureResult("Author not found.");
            }

            _logger.LogInformation("Author with Id: {AuthorId} found.", request.Id);
            return OperationResult<Author?>.SuccessResult(author);
        }
    }
}