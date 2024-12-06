using Application.Interfaces.RepositoryInterfaces;
using Application.Users.Queries.Login.Helpers;
using Domain;
using MediatR;

namespace Application.Users.Queries.Login
{
    public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly TokenHelper _tokenHelper;

        public LoginUserQueryHandler(IUserRepository userRepository, TokenHelper tokenHelper)
        {
            _userRepository = userRepository;
            _tokenHelper = tokenHelper;
        }

        public async Task<string> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.LoginUser(new User
            {
                UserName = request.LoginUser.UserName,
                Password = request.LoginUser.Password
            });

            if (user == null)
            {
                return "Invalid username or password";
            }

            string token = _tokenHelper.GenerateJwtToken(user);
            return token;
        }
    }
}