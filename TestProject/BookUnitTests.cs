using Application.Books.Commands;
using Application.Books.Commands.AddBook;
using Application.Books.Commands.RemoveBook;
using Application.Books.Commands.UpdateBook;
using Application.Books.Queries;
using Application.Books.Queries.GetbookbyID;
using Domain;
using Infrastructure.Database;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Threading.Tasks;

namespace TestProject
{
    public class Tests
    {
        private FakeDatabase _fakeDatabase;
        private IMediator _mediator;

        [SetUp]
        public void Setup()
        {
            _fakeDatabase = new FakeDatabase();
            var services = new ServiceCollection();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AddBookCommandHandler).Assembly));
            services.AddSingleton(_fakeDatabase);
            var provider = services.BuildServiceProvider();
            _mediator = provider.GetRequiredService<IMediator>();
        }

        [Test]
        public async Task When_Method_AddNewBook_isCalled_Then_BookAddedToList()
        {
            // Arrange
            Book bookToTest = new Book(1, "RobertBook", "Book of life","Dr.Booker McBook");

            // Act
            Book bookCreated = await _mediator.Send(new AddBookCommand(bookToTest));

            // Assert
            Assert.That(bookToTest, Is.Not.Null);
            Assert.That(bookCreated.Description, Is.EqualTo(bookToTest.Description));
        }

        [Test]
        public async Task When_Method_RemoveBook_isCalled_Then_BookRemovedFromList()
        {
            // Arrange
            Book bookToTest = new Book(1, "RobertCook", "Book of life");
            await _mediator.Send(new AddBookCommand(bookToTest)); // Add the book first

            // Act
            Book? bookRemoved = await _mediator.Send(new RemoveBookCommand(bookToTest));

            // Assert
            Assert.That(bookRemoved, Is.Not.Null);
        }

        [Test]
        public async Task When_Method_UpdateBook_isCalled_Then_BookUpdated()
        {
            // Arrange
            Book bookToTest = new Book(1, "RobertCook", "Book of life");
            await _mediator.Send(new AddBookCommand(bookToTest)); // Add the book first

            // Act
            bookToTest.Title = "Updated Title";
            Book? bookUpdated = await _mediator.Send(new UpdateBookCommand(bookToTest));

            // Assert
            Assert.That(bookUpdated, Is.Not.Null);
            Assert.That(bookUpdated.Title, Is.EqualTo("Updated Title"));
        }

        [Test]
        public async Task When_Method_GetBookById_isCalled_Then_BookReturned()
        {
            // Arrange
            Book bookToTest = new Book(1, "RobertCook", "Book of life");
            await _mediator.Send(new AddBookCommand(bookToTest)); // Add the book first

            // Act
            Book? bookReturned = await _mediator.Send(new GetBookByIdQuery(1));

            // Assert
            Assert.That(bookReturned, Is.Not.Null);
        }

        [Test]
        public void When_Method_AddNewBook_isCalled_With_EmptyTitle_Then_ArgumentExceptionIsThrown()
        {
            // Arrange
            Book bookToTest = new Book(1, "", "Description");  // Empty title

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(() => _mediator.Send(new AddBookCommand(bookToTest)));
        }

        [Test]
        public void When_Method_RemoveBook_isCalled_With_InvalidBook_Then_BookNotRemoved()
        {
            // Arrange
            Book? bookToTest = null;

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => _mediator.Send(new RemoveBookCommand(bookToTest)));
        }

        [Test]
        public void When_Method_UpdateBook_isCalled_With_InvalidBook_Then_BookNotUpdated()
        {
            // Arrange
            Book? bookToTest = null;

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => _mediator.Send(new UpdateBookCommand(bookToTest)));
        }

        [Test]
        public void When_Method_GetBookById_isCalled_With_InvalidBook_Then_BookNotReturned()
        {
            // Arrange
            int invalidId = -1;

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(() => _mediator.Send(new GetBookByIdQuery(invalidId)));
        }
    }
}