using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces.RepositoryInterfaces;
using Domain;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    internal class BookRepository : IBookRepository
    {
        private readonly RealDatabase _realDatabase;

        public BookRepository(RealDatabase realDatabase)
        {
            _realDatabase = realDatabase;
        }

        public async Task<Book> AddBook(Book book)
        {
            _realDatabase.Books.Add(book);
            await _realDatabase.SaveChangesAsync();
            return book;
        }

        public async Task<string> DeleteBookById(Guid id)
        {
            var book = await _realDatabase.Books.FindAsync(id);
            if (book != null)
            {
                _realDatabase.Books.Remove(book);
                await _realDatabase.SaveChangesAsync();
                return "Book Deleted Successfully";
            }
            return "Book Not Found";
        }

        public async Task<List<Book>> GetAllBooks()
        {
            return await _realDatabase.Books.ToListAsync();
        }

        public async Task<Book> GetBookById(Guid id)
        {
            return await _realDatabase.Books.FindAsync(id);
        }

        public async Task<Book> UpdateBook(Guid id, Book book)
        {
            _realDatabase.Books.Update(book);
            await _realDatabase.SaveChangesAsync();
            return book;
        }
    }
}