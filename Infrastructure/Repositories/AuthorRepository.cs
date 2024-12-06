using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces.RepositoryInterfaces;
using Domain;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    internal class AuthorRepository : IAuthorRepository
    {
        private readonly RealDatabase _realDatabase;

        public AuthorRepository(RealDatabase realDatabase)
        {
            _realDatabase = realDatabase;
        }

        public async Task<Author> AddAuthor(Author author)
        {
            _realDatabase.Authors.Add(author);
            await _realDatabase.SaveChangesAsync();
            return author;
        }

        public async Task<string> DeleteAuthorById(Guid id)
        {
            var author = await _realDatabase.Authors.FindAsync(id);
            if (author != null)
            {
                _realDatabase.Authors.Remove(author);
                await _realDatabase.SaveChangesAsync();
                return "Author Deleted Successfully";
            }
            return "Author Not Found";
        }

        public async Task<List<Author>> GetAllAuthors()
        {
            return await _realDatabase.Authors.ToListAsync();
        }


        public async Task<Author> GetAuthorById(Guid id)
        {
            return await _realDatabase.Authors.FindAsync(id);
        }

        public async Task<Author> UpdateAuthor(Guid id, Author author)
        {
            _realDatabase.Authors.Update(author);
            await _realDatabase.SaveChangesAsync();
            return author;
        }
    }
}