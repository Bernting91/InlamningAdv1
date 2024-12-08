using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands.RemoveUser
{
    public class RemoveUserCommandHandler : IRequestHandler<RemoveUserCommand, OperationResult<string>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<RemoveUserCommandHandler> _logger;

        public RemoveUserCommandHandler(IUserRepository userRepository, ILogger<RemoveUserCommandHandler> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<OperationResult<string>> Handle(RemoveUserCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling RemoveUserCommand for User Id: {UserId}", request.Id);

            try
            {
                if (request.Id == Guid.Empty)
                {
                    _logger.LogWarning("RemoveUserCommand received with empty Id.");
                    return OperationResult<string>.FailureResult("Id cannot be empty.");
                }

                var result = await _userRepository.DeleteUserById(request.Id);
                if (result == "User Not Found")
                {
                    _logger.LogWarning("RemoveUserCommand received for non-existent User with Id: {UserId}", request.Id);
                    return OperationResult<string>.FailureResult("User not found.");
                }

                _logger.LogInformation("User with Id: {UserId} removed successfully.", request.Id);
                return OperationResult<string>.SuccessResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while removing the user with Id: {UserId}", request.Id);
                return OperationResult<string>.FailureResult($"An error occurred while removing the user: {ex.Message}");
            }
        }
    }
}