using Domain;
using System.Collections.Generic;

namespace Infrastructure.Database
{
    public class FakeDatabase
    {
        public List<Book> Books { get { return allBooksFromDB; } set { allBooksFromDB = value; } }

        private static List<Book> allBooksFromDB = new List<Book>()
        {
            new Book(1, "RobertCook1", "Book of life1"),
            new Book(2, "RobertCook2", "Book of life2"),
            new Book(3, "RobertCook3", "Book of life3"),
            new Book(4, "RobertCook4", "Book of life4"),
            new Book(5, "RobertCook5", "Book of life5"),
        };

        //MY CRUD METHODS
        public Book AddNewBook(Book book)
        {
            allBooksFromDB.Add(book);
            return book;
        }

        public Book? RemoveBook(Book book)
        {
            if (allBooksFromDB.Remove(book))
            {
                return book;
            }
            else
            {
                return null;
            }
        }

        public Book? UpdateBook(Book book)
        {
            Book? bookToUpdate = allBooksFromDB.Find(bookToUpdate => bookToUpdate.Id == book.Id);
            if (bookToUpdate != null)
            {
                bookToUpdate.Title = book.Title;
                bookToUpdate.Description = book.Description;
            }
            return bookToUpdate;
        }

        public Book? GetBookById(int id)
        {
            return allBooksFromDB.Find(book => book.Id == id);
        }
    }
}