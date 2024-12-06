using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Queries.GetUserById
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, OperationResult<User>>
    {
        private readonly IUserRepository _userRepository;

        public GetUserByIdQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<OperationResult<User>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
            if (request.Id == Guid.Empty)
            {
                return OperationResult<User>.FailureResult("User Id is required.");
            }

            var user = await _userRepository.GetUserById(request.Id);
            if (user == null)
            {
                return OperationResult<User>.FailureResult("User not found.");
            }

            return OperationResult<User>.SuccessResult(user);
        }
    
            catch (Exception ex)
            {
                return OperationResult<User>.FailureResult($"An error occurred while retrieving user: {ex.Message}");
            }
        }
    }
}