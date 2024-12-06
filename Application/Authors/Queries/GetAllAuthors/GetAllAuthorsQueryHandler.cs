using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;

namespace Application.Authors.Queries.GetAllAuthors
{
    public class GetAllAuthorsQueryHandler : IRequestHandler<GetAllAuthorsQuery, OperationResult<IEnumerable<Author>>>
    {
        private readonly IAuthorRepository _authorRepository;

        public GetAllAuthorsQueryHandler(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task<OperationResult<IEnumerable<Author>>> Handle(GetAllAuthorsQuery request, CancellationToken cancellationToken)
        {
            var authors = await _authorRepository.GetAllAuthors();
            if (authors == null || !authors.Any())
            {
                return OperationResult<IEnumerable<Author>>.FailureResult("No authors found.");
            }
            return OperationResult<IEnumerable<Author>>.SuccessResult(authors);
        }
    }
}
