using Domain;
using System.Collections.Generic;

namespace Infrastructure.Database
{
    public class FakeDatabase
    {
        public List<Book> Books { get; set; } = new()
            {
                new Book(Guid.NewGuid(), "RobertCook1", "Book of life1", new Author(Guid.NewGuid(), "Dr.Book McBookie")),
                new Book(Guid.NewGuid(), "RobertCook2", "Book of life2", new Author(Guid.NewGuid(), "Dr.Booksie McBookie")),
                new Book(Guid.NewGuid(), "RobertCook3", "Book of life3", new Author(Guid.NewGuid(), "Dr.Booksson McBookie")),
                new Book(Guid.NewGuid(), "RobertCook4", "Book of life4", new Author(Guid.NewGuid(), "Dr.Booker McBookie")),
                new Book(Guid.NewGuid(), "RobertCook5", "Book of life5", new Author(Guid.NewGuid(), "Dr.BookDur McBookie")),
            };
        public List<Author> Authors { get; set; } = new()
            {
                new Author(Guid.NewGuid(), "Dr.Book McBookie"),
                new Author(Guid.NewGuid(), "Dr.Booksie McBookie"),
                new Author(Guid.NewGuid(), "Dr.Booksson McBookie"),
                new Author(Guid.NewGuid(), "Dr.Booker McBookie"),
                new Author(Guid.NewGuid(), "Dr.BookDur McBookie"),
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