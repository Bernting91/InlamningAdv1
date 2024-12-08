using Application.Interfaces.RepositoryInterfaces;
using Domain;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private readonly RealDatabase _realDatabase;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(RealDatabase realDatabase, ILogger<UserRepository> logger)
        {
            _realDatabase = realDatabase;
            _logger = logger;
        }

        public async Task<User> AddUser(User user)
        {
            _logger.LogInformation("Adding a new user: {UserName}", user.UserName);
            _realDatabase.Users.Add(user);
            await _realDatabase.SaveChangesAsync();
            _logger.LogInformation("User added successfully: {UserName}", user.UserName);
            return user;
        }

        public async Task<string> DeleteUserById(Guid id)
        {
            _logger.LogInformation("Deleting user with Id: {UserId}", id);
            var user = await _realDatabase.Users.FindAsync(id);
            if (user != null)
            {
                _realDatabase.Users.Remove(user);
                await _realDatabase.SaveChangesAsync();
                _logger.LogInformation("User deleted successfully: {UserId}", id);
                return "User Deleted Successfully";
            }
            _logger.LogWarning("User not found: {UserId}", id);
            return "User Not Found";
        }

        public async Task<List<User>> GetAllUsers()
        {
            _logger.LogInformation("Retrieving all users");
            return await _realDatabase.Users.ToListAsync();
        }

        public async Task<User> GetUserById(Guid id)
        {
            _logger.LogInformation("Retrieving user with Id: {UserId}", id);
            return await _realDatabase.Users.FindAsync(id);
        }

        public async Task<User> LoginUser(User user)
        {
            _logger.LogInformation("Logging in user: {UserName}", user.UserName);
            return await _realDatabase.Users.FirstOrDefaultAsync(u => u.UserName == user.UserName && u.Password == user.Password);
        }

        public async Task<User> UpdateUser(Guid id, User user)
        {
            _logger.LogInformation("Updating user with Id: {UserId}", id);
            _realDatabase.Users.Update(user);
            await _realDatabase.SaveChangesAsync();
            _logger.LogInformation("User updated successfully: {UserId}", id);
            return user;
        }
    }
}