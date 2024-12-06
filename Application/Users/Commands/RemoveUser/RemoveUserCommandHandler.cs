using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands.RemoveUser
{
    public class RemoveUserCommandHandler : IRequestHandler<RemoveUserCommand, string>
    {
        private readonly IUserRepository _userRepository;

        public RemoveUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<string> Handle(RemoveUserCommand request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
            {
                throw new ArgumentException("Id cannot be empty", nameof(request.Id));
            }

            var result = await _userRepository.DeleteUserById(request.Id);
            if (result == "User Not Found")
            {
                throw new KeyNotFoundException($"User with Id {request.Id} not found.");
            }

            return result;
        }
    }
}