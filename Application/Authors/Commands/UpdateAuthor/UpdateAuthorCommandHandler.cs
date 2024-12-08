using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Authors.Commands.UpdateAuthor
{
    public class UpdateAuthorCommandHandler : IRequestHandler<UpdateAuthorCommand, OperationResult<Author?>>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly ILogger<UpdateAuthorCommandHandler> _logger;

        public UpdateAuthorCommandHandler(IAuthorRepository authorRepository, ILogger<UpdateAuthorCommandHandler> logger)
        {
            _authorRepository = authorRepository;
            _logger = logger;
        }

        public async Task<OperationResult<Author?>> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling UpdateAuthorCommand for Author Id: {AuthorId}", request.Id);

            var author = await _authorRepository.GetAuthorById(request.Id);
            if (author == null)
            {
                _logger.LogWarning("UpdateAuthorCommand received for non-existent Author with Id: {AuthorId}", request.Id);
                return OperationResult<Author?>.FailureResult("Author not found.");
            }

            author.Name = request.Name;
            var updatedAuthor = await _authorRepository.UpdateAuthor(request.Id, author);

            _logger.LogInformation("Author with Id: {AuthorId} updated successfully.", request.Id);
            return OperationResult<Author?>.SuccessResult(updatedAuthor);
        }
    }
}