using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;


namespace Application.Users.Commands.AddUser
{
    internal sealed class AddNewUserCommandHandler : IRequestHandler<AddNewUserCommand, User>
    {
        private readonly IUserRepository _userRepository;

        public AddNewUserCommandHandler (IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<User> Handle(AddNewUserCommand request, CancellationToken cancellationToken)
        {
            User userToCreate = new()
            {
                Id = Guid.NewGuid(),
                UserName = request.NewUser.UserName,
                Password = request.NewUser.Password,
            };

            _userRepository.AddUser(userToCreate);
            return Task.FromResult(userToCreate);
            
        }

    }
}
