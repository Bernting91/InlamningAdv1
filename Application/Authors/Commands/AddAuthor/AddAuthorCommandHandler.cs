using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;

namespace Application.Authors.Commands.AddAuthor
{
    public class AddAuthorCommandHandler : IRequestHandler<AddAuthorCommand, OperationResult<Author>>
    {
        private readonly IAuthorRepository _authorRepository;

        public AddAuthorCommandHandler(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task<OperationResult<Author>> Handle(AddAuthorCommand request, CancellationToken cancellationToken)
        {
            if (request.Author == null)
            {
                return OperationResult<Author>.FailureResult("Author cannot be null.");
            }
            if (string.IsNullOrWhiteSpace(request.Author.Name))
            {
                return OperationResult<Author>.FailureResult("Author name cannot be empty.");
            }

            _authorRepository.AddAuthor(request.Author);
            return OperationResult<Author>.SuccessResult(request.Author);
        }
    }
}