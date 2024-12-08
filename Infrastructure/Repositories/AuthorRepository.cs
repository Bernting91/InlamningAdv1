using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces.RepositoryInterfaces;
using Domain;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories
{
    internal class AuthorRepository : IAuthorRepository
    {
        private readonly RealDatabase _realDatabase;
        private readonly ILogger<AuthorRepository> _logger;

        public AuthorRepository(RealDatabase realDatabase, ILogger<AuthorRepository> logger)
        {
            _realDatabase = realDatabase;
            _logger = logger;
        }

        public async Task<Author> AddAuthor(Author author)
        {
            _logger.LogInformation("Adding a new author: {Name}", author.Name);
            _realDatabase.Authors.Add(author);
            await _realDatabase.SaveChangesAsync();
            _logger.LogInformation("Author added successfully: {Name}", author.Name);
            return author;
        }

        public async Task<string> DeleteAuthorById(Guid id)
        {
            _logger.LogInformation("Deleting author with Id: {AuthorId}", id);
            var author = await _realDatabase.Authors.FindAsync(id);
            if (author != null)
            {
                _realDatabase.Authors.Remove(author);
                await _realDatabase.SaveChangesAsync();
                _logger.LogInformation("Author deleted successfully: {AuthorId}", id);
                return "Author Deleted Successfully";
            }
            _logger.LogWarning("Author not found: {AuthorId}", id);
            return "Author Not Found";
        }

        public async Task<List<Author>> GetAllAuthors()
        {
            _logger.LogInformation("Retrieving all authors");
            return await _realDatabase.Authors.ToListAsync();
        }

        public async Task<Author> GetAuthorById(Guid id)
        {
            _logger.LogInformation("Retrieving author with Id: {AuthorId}", id);
            return await _realDatabase.Authors.FindAsync(id);
        }

        public async Task<Author> UpdateAuthor(Guid id, Author author)
        {
            _logger.LogInformation("Updating author with Id: {AuthorId}", id);
            _realDatabase.Authors.Update(author);
            await _realDatabase.SaveChangesAsync();
            _logger.LogInformation("Author updated successfully: {AuthorId}", id);
            return author;
        }
    }
}