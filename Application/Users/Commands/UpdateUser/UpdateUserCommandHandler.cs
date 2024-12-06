using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;

namespace Application.Users.Commands.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, User>
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            if (request.Id == 0)
            {
                throw new ArgumentException("Id cannot be empty.", nameof(request.Id));
            }
            if (string.IsNullOrWhiteSpace(request.user.UserName))
            {
                throw new ArgumentException("Name cannot be empty.", nameof(request.user));
            }
            var updatedUser = await _userRepository.UpdateUser(request.Id, request.user);
            return updatedUser;
        }
    }
}
