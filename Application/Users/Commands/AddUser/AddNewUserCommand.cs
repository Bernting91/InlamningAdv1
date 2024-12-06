using Application.Dtos;
using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.Commands.AddUser
{
    public class AddNewUserCommand : IRequest<OperationResult<User>>
    {
      public AddNewUserCommand (UserDto newUser)
        {
            NewUser = newUser;
        }
        public UserDto NewUser { get; }
    }
}
