using Application.Users.Queries.Login.Helpers;
using Domain;
using Infrastructure.Database;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.Queries.Login
{
    public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, string>
    {
        private readonly FakeDatabase _fakeDatabase;
        private readonly TokenHelper _tokenHelper;

        public LoginUserQueryHandler(FakeDatabase fakeDatabase, TokenHelper tokenhelper)
        {
            _fakeDatabase = fakeDatabase;
            _tokenHelper = tokenhelper;
        }
        public Task<String> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            var user = _fakeDatabase.Users.FirstOrDefault(user => user.UserName == request.LoginUser.UserName && user.Password == request.LoginUser.Password);
            if (user == null)
            {
                return Task.FromResult("Invalid username or password");
            }
            string token = _tokenHelper.GenerateJwtToken(user);
            return Task.FromResult(token);
        }
    }
}
