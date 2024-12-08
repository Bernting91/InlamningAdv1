using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Queries.GetAllUsers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, OperationResult<List<User>>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<GetAllUsersQueryHandler> _logger;

        public GetAllUsersQueryHandler(IUserRepository userRepository, ILogger<GetAllUsersQueryHandler> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<OperationResult<List<User>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetAllUsersQuery");

            try
            {
                var users = await _userRepository.GetAllUsers();
                if (users == null || users.Count == 0)
                {
                    _logger.LogWarning("No users found.");
                    return OperationResult<List<User>>.FailureResult("No users found.");
                }

                _logger.LogInformation("Found {UserCount} users.", users.Count);
                return OperationResult<List<User>>.SuccessResult(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving users.");
                return OperationResult<List<User>>.FailureResult($"An error occurred while retrieving users: {ex.Message}");
            }
        }
    }
}