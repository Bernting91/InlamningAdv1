using Application.Books.Commands.AddBook;
using Application.Books.Commands.UpdateBook;
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
    public class UpdateBookTests
    {
        private IMediator _mediator;
        private IBookRepository _bookRepository;

        [SetUp]
        public void Setup()
        {
            _bookRepository = A.Fake<IBookRepository>();
            var services = new ServiceCollection();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UpdateBookCommandHandler).Assembly));
            services.AddSingleton(_bookRepository);
            var provider = services.BuildServiceProvider();
            _mediator = provider.GetRequiredService<IMediator>();
        }

        [Test]
        public async Task When_Method_UpdateBook_isCalled_Then_BookUpdated()
        {
            // Arrange
            Author author = new Author(Guid.NewGuid(), "Dr.Book McBookie");
            Book bookToTest = new Book(Guid.NewGuid(), "RobertCook", "Book of life", author);
            A.CallTo(() => _bookRepository.GetBookById(bookToTest.Id)).Returns(Task.FromResult(bookToTest));
            A.CallTo(() => _bookRepository.UpdateBook(bookToTest.Id, bookToTest)).ReturnsLazily((Guid id, Book book) =>
            {
                book.Title = "Updated Title";
                return Task.FromResult(book);
            });

            // Act
            OperationResult<Book?> result = await _mediator.Send(new UpdateBookCommand(bookToTest.Id, bookToTest));

            // Assert
            Assert.That(result.IsSuccessfull, Is.True);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Data.Title, Is.EqualTo("Updated Title"));
        }

        [Test]
        public async Task When_Method_UpdateBook_isCalled_With_InvalidBook_Then_OperationResultFailure()
        {
            // Act
            OperationResult<Book?> result = await _mediator.Send(new UpdateBookCommand(Guid.NewGuid(), null));

            // Assert
            Assert.That(result.IsSuccessfull, Is.False);
            Assert.That(result.Data, Is.Null);
            Assert.That(result.ErrorMessage, Is.EqualTo("Book cannot be null."));
        }
    }
}