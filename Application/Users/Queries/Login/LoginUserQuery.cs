using Application.Dtos;
using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.Queries.Login
{
    public class LoginUserQuery : IRequest<OperationResult<String>>
    {
        public LoginUserQuery(UserDto loginUser)
        {
            LoginUser = loginUser;
        }
        public UserDto LoginUser
        {
            get;
        }
    }
}
