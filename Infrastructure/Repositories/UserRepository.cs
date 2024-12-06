using Application.Interfaces.RepositoryInterfaces;
using Domain;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private readonly RealDatabase _realDatabase;

        public UserRepository(RealDatabase realDatabase)
        {
            _realDatabase = realDatabase;
        }

        public async Task<User> AddUser(User user)
        {
            _realDatabase.Users.Add(user);
            await _realDatabase.SaveChangesAsync();
            return user;
        }

        public async Task<string> DeleteUserById(Guid id)
        {
            var user = await _realDatabase.Users.FindAsync(id);
            if (user != null)
            {
                _realDatabase.Users.Remove(user);
                await _realDatabase.SaveChangesAsync();
                return "User Deleted Successfully";
            }
            return "User Not Found";
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _realDatabase.Users.ToListAsync();
        }

        public async Task<User> GetUserById(Guid id)
        {
            return await _realDatabase.Users.FindAsync(id);
        }

        public async Task<User> LoginUser(User user)
        {
            return await _realDatabase.Users.FirstOrDefaultAsync(u => u.UserName == user.UserName && u.Password == user.Password);
        }

        public async Task<User> UpdateUser(Guid id, User user)
        {
            _realDatabase.Users.Update(user);
            await _realDatabase.SaveChangesAsync();
            return user;
        }
    }
}