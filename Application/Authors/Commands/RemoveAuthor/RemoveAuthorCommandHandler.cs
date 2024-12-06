using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;

namespace Application.Authors.Commands.RemoveAuthor
{
    public class RemoveAuthorCommandHandler : IRequestHandler<RemoveAuthorCommand, OperationResult<Author?>>
    {
        private readonly IAuthorRepository _authorRepository;

        public RemoveAuthorCommandHandler(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task<OperationResult<Author?>> Handle(RemoveAuthorCommand request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
            {
                return OperationResult<Author>.FailureResult("Id cannot be empty.");
            }

            var author = await _authorRepository.GetAuthorById(request.Id);
            if (author == null)
            {
                return OperationResult<Author>.FailureResult("Author not found.");
            }

            await _authorRepository.DeleteAuthorById(request.Id);
            return OperationResult<Author>.SuccessResult(author);
        }
    }
}