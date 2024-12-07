using Application.Books.Commands.RemoveBook;
using Application.Interfaces.RepositoryInterfaces;
using Domain;
using FakeItEasy;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace TestProject.Books.Commands
{
    public class RemoveBookTests
    {
        private IMediator _mediator;
        private IBookRepository _bookRepository;

        [SetUp]
        public void Setup()
        {
            _bookRepository = A.Fake<IBookRepository>();
            var services = new ServiceCollection();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(RemoveBookCommandHandler).Assembly));
            services.AddSingleton(_bookRepository);
            var provider = services.BuildServiceProvider();
            _mediator = provider.GetRequiredService<IMediator>();
        }

        [Test]
        public async Task When_Method_RemoveBook_isCalled_Then_BookRemovedFromList()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var book = new Book(bookId, "Test Title", "Test Description", new Author(Guid.NewGuid(), "Test Author"));

            A.CallTo(() => _bookRepository.GetBookById(bookId)).Returns(Task.FromResult(book));
            A.CallTo(() => _bookRepository.DeleteBookById(bookId)).Returns(Task.FromResult("Book Deleted Successfully"));

            // Act
            var result = await _mediator.Send(new RemoveBookCommand(bookId));

            // Assert
            Assert.That(result.IsSuccessfull, Is.True);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Data.Id, Is.EqualTo(bookId));
        }

        [Test]
        public async Task When_Method_RemoveBook_isCalled_With_InvalidBook_Then_BookNotRemoved()
        {
            // Arrange
            var bookId = Guid.Empty;

            // Act
            var result = await _mediator.Send(new RemoveBookCommand(bookId));

            // Assert
            Assert.That(result.IsSuccessfull, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("Id cannot be empty."));
        }

        [Test]
        public async Task When_Method_RemoveBook_isCalled_With_NonExistentBook_Then_FailureResultIsReturned()
        {
            // Arrange
            var bookId = Guid.NewGuid();

            A.CallTo(() => _bookRepository.GetBookById(bookId)).Returns(Task.FromResult<Book>(null));

            // Act
            var result = await _mediator.Send(new RemoveBookCommand(bookId));

            // Assert
            Assert.That(result.IsSuccessfull, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("Book not found."));
        }

        [Test]
        public async Task When_Method_RemoveBook_isCalled_And_ExceptionOccurs_Then_FailureResultIsReturned()
        {
            // Arrange
            var bookId = Guid.NewGuid();

            A.CallTo(() => _bookRepository.GetBookById(bookId)).Throws<Exception>();

            // Act
            var result = await _mediator.Send(new RemoveBookCommand(bookId));

            // Assert
            Assert.That(result.IsSuccessfull, Is.False);
            Assert.That(result.ErrorMessage, Is.Not.Null);
        }
    }
}