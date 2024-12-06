using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using MediatR;

namespace Application.Users.Queries.GetAllUsers
{
    public record GetAllUsersQuery : IRequest<OperationResult<List<User>>>
    {
    }
}
