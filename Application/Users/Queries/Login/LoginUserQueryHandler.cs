using Application.Interfaces.RepositoryInterfaces;
using Application.Users.Queries.Login.Helpers;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Queries.Login
{
    public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, OperationResult<string>>
    {
        private readonly IUserRepository _userRepository;
        private readonly TokenHelper _tokenHelper;
        private readonly ILogger<LoginUserQueryHandler> _logger;

        public LoginUserQueryHandler(IUserRepository userRepository, TokenHelper tokenHelper, ILogger<LoginUserQueryHandler> logger)
        {
            _userRepository = userRepository;
            _tokenHelper = tokenHelper;
            _logger = logger;
        }

        public async Task<OperationResult<string>> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling LoginUserQuery for User: {UserName}", request.LoginUser.UserName);

            try
            {
                var user = await _userRepository.LoginUser(new User
                {
                    UserName = request.LoginUser.UserName,
                    Password = request.LoginUser.Password
                });

                if (user == null)
                {
                    _logger.LogWarning("Invalid username or password for User: {UserName}", request.LoginUser.UserName);
                    return OperationResult<string>.FailureResult("Invalid username or password");
                }

                string token = _tokenHelper.GenerateJwtToken(user);
                _logger.LogInformation("User {UserName} logged in successfully.", request.LoginUser.UserName);
                return OperationResult<string>.SuccessResult(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while logging in User: {UserName}", request.LoginUser.UserName);
                return OperationResult<string>.FailureResult($"An error occurred while logging in: {ex.Message}");
            }
        }
    }
}