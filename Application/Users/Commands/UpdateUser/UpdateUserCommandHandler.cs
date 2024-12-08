using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, OperationResult<User>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UpdateUserCommandHandler> _logger;

        public UpdateUserCommandHandler(IUserRepository userRepository, ILogger<UpdateUserCommandHandler> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<OperationResult<User>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling UpdateUserCommand for User Id: {UserId}", request.Id);

            try
            {
                if (request.Id == Guid.Empty)
                {
                    _logger.LogWarning("UpdateUserCommand received with empty Id.");
                    return OperationResult<User>.FailureResult("Id cannot be empty.");
                }
                if (string.IsNullOrWhiteSpace(request.user.UserName))
                {
                    _logger.LogWarning("UpdateUserCommand received with empty UserName.");
                    return OperationResult<User>.FailureResult("Username cannot be empty.");
                }

                var existingUser = await _userRepository.GetUserById(request.Id);
                if (existingUser == null)
                {
                    _logger.LogWarning("UpdateUserCommand received for non-existent User with Id: {UserId}", request.Id);
                    return OperationResult<User>.FailureResult("User not found.");
                }

                existingUser.UserName = request.user.UserName;
                existingUser.Password = request.user.Password;

                var updatedUser = await _userRepository.UpdateUser(request.Id, existingUser);
                _logger.LogInformation("User with Id: {UserId} updated successfully.", request.Id);
                return OperationResult<User>.SuccessResult(updatedUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the user with Id: {UserId}", request.Id);
                return OperationResult<User>.FailureResult($"An error occurred while updating the user: {ex.Message}");
            }
        }
    }
}