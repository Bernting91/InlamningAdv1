using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Queries.GetUserById
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, OperationResult<User>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<GetUserByIdQueryHandler> _logger;

        public GetUserByIdQueryHandler(IUserRepository userRepository, ILogger<GetUserByIdQueryHandler> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<OperationResult<User>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetUserByIdQuery for User Id: {UserId}", request.Id);

            try
            {
                if (request.Id == Guid.Empty)
                {
                    _logger.LogWarning("GetUserByIdQuery received with empty Id.");
                    return OperationResult<User>.FailureResult("User Id is required.");
                }

                var user = await _userRepository.GetUserById(request.Id);
                if (user == null)
                {
                    _logger.LogWarning("GetUserByIdQuery received for non-existent User with Id: {UserId}", request.Id);
                    return OperationResult<User>.FailureResult("User not found.");
                }

                _logger.LogInformation("User with Id: {UserId} found.", request.Id);
                return OperationResult<User>.SuccessResult(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving user with Id: {UserId}", request.Id);
                return OperationResult<User>.FailureResult($"An error occurred while retrieving user: {ex.Message}");
            }
        }
    }
}