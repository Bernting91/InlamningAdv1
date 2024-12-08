using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands.AddUser
{
    public class AddNewUserCommandHandler : IRequestHandler<AddNewUserCommand, OperationResult<User>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<AddNewUserCommandHandler> _logger;

        public AddNewUserCommandHandler(IUserRepository userRepository, ILogger<AddNewUserCommandHandler> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<OperationResult<User>> Handle(AddNewUserCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling AddNewUserCommand for User: {UserName}", request.NewUser.UserName);

            try
            {
                if (string.IsNullOrWhiteSpace(request.NewUser.UserName))
                {
                    _logger.LogWarning("AddNewUserCommand received with empty UserName.");
                    return OperationResult<User>.FailureResult("UserName cannot be empty.");
                }

                User userToCreate = new()
                {
                    Id = Guid.NewGuid(),
                    UserName = request.NewUser.UserName,
                    Password = request.NewUser.Password,
                };

                await _userRepository.AddUser(userToCreate);
                _logger.LogInformation("User {UserName} added successfully.", request.NewUser.UserName);
                return OperationResult<User>.SuccessResult(userToCreate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding the user: {UserName}", request.NewUser.UserName);
                return OperationResult<User>.FailureResult($"An error occurred while adding the user: {ex.Message}");
            }
        }
    }
}