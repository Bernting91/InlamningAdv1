using Application.Books.Commands.AddBook;
using Application.Books.Commands.RemoveBook;
using Domain;
using Infrastructure.Database;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace TestProject.Books.Commands
{
    public class RemoveBookTests
    {
        private FakeDatabase _fakeDatabase;
        private IMediator _mediator;

        [SetUp]
        public void Setup()
        {
            _fakeDatabase = new FakeDatabase();
            var services = new ServiceCollection();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(RemoveBookCommandHandler).Assembly));
            services.AddSingleton(_fakeDatabase);
            var provider = services.BuildServiceProvider();
            _mediator = provider.GetRequiredService<IMediator>();
        }

        [Test]
        public async Task When_Method_RemoveBook_isCalled_Then_BookRemovedFromList()
        {
            // Arrange
            Author author = new Author(1, "Dr.Book McBookie");
            Book bookToTest = new Book(1, "RobertCook", "Book of life", author);
            await _mediator.Send(new AddBookCommand(bookToTest));

            // Act
            Book? bookRemoved = await _mediator.Send(new RemoveBookCommand(bookToTest));

            // Assert
            Assert.That(bookRemoved, Is.Not.Null);
        }

        [Test]
        public void When_Method_RemoveBook_isCalled_With_InvalidBook_Then_BookNotRemoved()
        {
            // Arrange
            Book? bookToTest = null;

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => _mediator.Send(new RemoveBookCommand(bookToTest)));
        }
    }
}