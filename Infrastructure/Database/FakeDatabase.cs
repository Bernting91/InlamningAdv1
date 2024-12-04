using Domain;
using System.Collections.Generic;

namespace Infrastructure.Database
{
    public class FakeDatabase
    {
        public List<Book> Books { get; set; } = new()
            {
                new Book(1, "RobertCook1", "Book of life1", new Author(1, "Dr.Book McBookie")),
                new Book(2, "RobertCook2", "Book of life2", new Author(2, "Dr.Booksie McBookie")),
                new Book(3, "RobertCook3", "Book of life3", new Author(3, "Dr.Booksson McBookie")),
                new Book(4, "RobertCook4", "Book of life4", new Author(4, "Dr.Booker McBookie")),
                new Book(5, "RobertCook5", "Book of life5", new Author(5, "Dr.BookDur McBookie")),
            };
        public List<Author> Authors { get; set; } = new()
            {
                new Author(1, "Dr.Book McBookie"),
                new Author(2, "Dr.Booksie McBookie"),
                new Author(3, "Dr.Booksson McBookie"),
                new Author(4, "Dr.Booker McBookie"),
                new Author(5, "Dr.BookDur McBookie"),
            };

        public List<User> Users
        {
            get { return allUsers; }
            set { allUsers = value; }
        }
        private static List<User> allUsers = new()
        {
            new User { Id = Guid.NewGuid(), UserName = "Robert"},
            new User { Id = Guid.NewGuid(), UserName = "Bobert"},
            new User { Id = Guid.NewGuid(), UserName = "Snobert"},
            new User { Id = new Guid ("12345678-1234-5678-1234-567812365478"), UserName = "TestUserForUnitTests" }
        };
    }
}