using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands.RemoveUser
{
    internal sealed class RemoveUserCommandHandler : IRequestHandler<RemoveUserCommand, OperationResult<string>>
    {
        private readonly IUserRepository _userRepository;

        public RemoveUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<OperationResult<string>> Handle(RemoveUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id == Guid.Empty)
                {
                    return OperationResult<string>.FailureResult("Id cannot be empty.");
                }

                var result = await _userRepository.DeleteUserById(request.Id);
                if (result == "User Not Found")
                {
                    return OperationResult<string>.FailureResult("User not found.");
                }

                return OperationResult<string>.SuccessResult(result);
            }
            catch (Exception ex)
            {
                return OperationResult<string>.FailureResult($"An error occurred while removing the user: {ex.Message}");
            }
        }
    }
}