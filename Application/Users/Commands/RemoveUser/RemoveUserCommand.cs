using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.Commands.RemoveUser
{
    public record RemoveUserCommand(Guid Id) : IRequest<string>
    {
    }
}
