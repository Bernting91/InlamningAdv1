using Application.Interfaces.RepositoryInterfaces;
using Application.Users.Queries.Login.Helpers;
using Domain;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Queries.Login
{
    public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, OperationResult<string>>
    {
        private readonly IUserRepository _userRepository;
        private readonly TokenHelper _tokenHelper;

        public LoginUserQueryHandler(IUserRepository userRepository, TokenHelper tokenHelper)
        {
            _userRepository = userRepository;
            _tokenHelper = tokenHelper;
        }

        public async Task<OperationResult<string>> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepository.LoginUser(new User
                {
                    UserName = request.LoginUser.UserName,
                    Password = request.LoginUser.Password
                });

                if (user == null)
                {
                    return OperationResult<string>.FailureResult("Invalid username or password");
                }

                string token = _tokenHelper.GenerateJwtToken(user);
                return OperationResult<string>.SuccessResult(token);
            }
            catch (Exception ex)
            {
                return OperationResult<string>.FailureResult($"An error occurred while logging in: {ex.Message}");
            }
        }
    }
}