using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands.AddUser
{
    public class AddNewUserCommandHandler : IRequestHandler<AddNewUserCommand, OperationResult<User>>
    {
        private readonly IUserRepository _userRepository;

        public AddNewUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<OperationResult<User>> Handle(AddNewUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.NewUser.UserName))
                {
                    return OperationResult<User>.FailureResult("UserName cannot be empty.");
                }

                User userToCreate = new()
                {
                    Id = Guid.NewGuid(),
                    UserName = request.NewUser.UserName,
                    Password = request.NewUser.Password,
                };

                await _userRepository.AddUser(userToCreate);
                return OperationResult<User>.SuccessResult(userToCreate);
            }
            catch (Exception ex)
            {
                return OperationResult<User>.FailureResult($"An error occurred while adding the user: {ex.Message}");
            }
        }
    }
}