using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, OperationResult<User>>
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<OperationResult<User>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id == Guid.Empty)
                {
                    return OperationResult<User>.FailureResult("Id cannot be empty.");
                }
                if (string.IsNullOrWhiteSpace(request.user.UserName))
                {
                    return OperationResult<User>.FailureResult("Username cannot be empty.");
                }

                var existingUser = await _userRepository.GetUserById(request.Id);
                if (existingUser == null)
                {
                    return OperationResult<User>.FailureResult("User not found.");
                }

                existingUser.UserName = request.user.UserName;
                existingUser.Password = request.user.Password;

                var updatedUser = await _userRepository.UpdateUser(request.Id, existingUser);
                return OperationResult<User>.SuccessResult(updatedUser);
            }
            catch (Exception ex)
            {
                return OperationResult<User>.FailureResult($"An error occurred while updating the user: {ex.Message}");
            }
        }
    }
}