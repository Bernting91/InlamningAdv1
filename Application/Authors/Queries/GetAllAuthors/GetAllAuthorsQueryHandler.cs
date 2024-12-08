using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Authors.Queries.GetAllAuthors
{
    public class GetAllAuthorsQueryHandler : IRequestHandler<GetAllAuthorsQuery, OperationResult<IEnumerable<Author>>>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly ILogger<GetAllAuthorsQueryHandler> _logger;

        public GetAllAuthorsQueryHandler(IAuthorRepository authorRepository, ILogger<GetAllAuthorsQueryHandler> logger)
        {
            _authorRepository = authorRepository;
            _logger = logger;
        }

        public async Task<OperationResult<IEnumerable<Author>>> Handle(GetAllAuthorsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetAllAuthorsQuery");

            var authors = await _authorRepository.GetAllAuthors();
            if (authors == null || !authors.Any())
            {
                _logger.LogWarning("No authors found.");
                return OperationResult<IEnumerable<Author>>.FailureResult("No authors found.");
            }

            _logger.LogInformation("Found {AuthorCount} authors.", authors.Count);
            return OperationResult<IEnumerable<Author>>.SuccessResult(authors);
        }
    }
}