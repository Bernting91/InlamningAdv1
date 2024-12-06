
using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;


namespace Application.Users.Queries.GetAllUsers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, OperationResult<List<User>>>
    {
        private readonly IUserRepository _userRepository;

        public GetAllUsersQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<OperationResult<List<User>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var users = await _userRepository.GetAllUsers();
                return OperationResult<List<User>>.SuccessResult(users);
            }
            catch (Exception ex)
            {
                return OperationResult<List<User>>.FailureResult($"An error occurred while retrieving users: {ex.Message}");
            }
        }
    }
}
