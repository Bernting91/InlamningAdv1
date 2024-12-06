using Application.Books.Commands.AddBook;
using Application.Books.Commands.UpdateBook;
using Domain;
using Infrastructure.Database;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace TestProject.Books.Commands
{
    public class UpdateBookTests
    {
        private FakeDatabase _fakeDatabase;
        private IMediator _mediator;

        [SetUp]
        public void Setup()
        {
            _fakeDatabase = new FakeDatabase();
            var services = new ServiceCollection();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UpdateBookCommandHandler).Assembly));
            services.AddSingleton(_fakeDatabase);
            var provider = services.BuildServiceProvider();
            _mediator = provider.GetRequiredService<IMediator>();
        }

        [Test]
        public async Task When_Method_UpdateBook_isCalled_Then_BookUpdated()
        {
            // Arrange
            Author author = new Author(Guid.NewGuid(), "Dr.Book McBookie");
            Book bookToTest = new Book(Guid.NewGuid(), "RobertCook", "Book of life", author);
            await _mediator.Send(new AddBookCommand(bookToTest));

            // Act
            bookToTest.Title = "Updated Title";
            Book? bookUpdated = await _mediator.Send(new UpdateBookCommand(Guid.NewGuid(),bookToTest));

            // Assert
            Assert.That(bookUpdated, Is.Not.Null);
            Assert.That(bookUpdated.Title, Is.EqualTo("Updated Title"));
        }

        [Test]
        public void When_Method_UpdateBook_isCalled_With_InvalidBook_Then_BookNotUpdated()
        {
            // Arrange
            Book? bookToTest = null;

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => _mediator.Send(new UpdateBookCommand(Guid.NewGuid(),bookToTest)));
        }
    }
}