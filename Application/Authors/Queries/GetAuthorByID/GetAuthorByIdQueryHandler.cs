using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;

namespace Application.Authors.Queries.GetAuthorByID
{
    public class GetAuthorByIdQueryHandler : IRequestHandler<GetAuthorByIdQuery, OperationResult<Author?>>
    {
        private readonly IAuthorRepository _authorRepository;

        public GetAuthorByIdQueryHandler(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task<OperationResult<Author?>> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
            {
                return OperationResult<Author?>.FailureResult("Id cannot be empty.");
            }
            var author = await _authorRepository.GetAuthorById(request.Id);
            if (author == null)
            {
                return OperationResult<Author?>.FailureResult("Author not found.");
            }
            return OperationResult<Author?>.SuccessResult(author);
        }
    }
}