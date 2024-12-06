using Domain;
using MediatR;

namespace Application.Authors.Commands.RemoveAuthor
{
    public class RemoveAuthorCommand : IRequest<OperationResult<Author>>
    {
        public Guid Id { get; }

        public RemoveAuthorCommand(Guid id)
        {
            Id = id;
        }
    }
}