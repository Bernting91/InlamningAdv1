using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Authors.Commands.UpdateAuthor
{
    public class UpdateAuthorCommandHandler : IRequestHandler<UpdateAuthorCommand, OperationResult<Author?>>
    {
        private readonly IAuthorRepository _authorRepository;

        public UpdateAuthorCommandHandler(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task<OperationResult<Author?>> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
        {
            var author = await _authorRepository.GetAuthorById(request.Id);
            if (author == null)
            {
                return OperationResult<Author?>.FailureResult("Author not found.");
            }

            author.Name = request.Name;
            var updatedAuthor = await _authorRepository.UpdateAuthor(request.Id, author);

            return OperationResult<Author?>.SuccessResult(updatedAuthor);
        }
    }
}