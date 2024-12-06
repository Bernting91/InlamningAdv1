using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.RepositoryInterfaces
{
    public interface IUserRepository
    {

        Task<User> AddUser(User user);

        Task<List<User>> GetAllUsers();

        Task<User> GetUserById(Guid id);

        Task<string> DeleteUserById(Guid id);

        Task<User> UpdateUser(int id, User user);

        Task<User> LoginUser(User user);
    }
}
