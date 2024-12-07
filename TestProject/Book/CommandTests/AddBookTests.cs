using Application.Books.Commands.AddBook;
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
    public class AddBookTests
    {
        private IMediator _mediator;
        private IBookRepository _bookRepository;

        [SetUp]
        public void Setup()
        {
            _bookRepository = A.Fake<IBookRepository>();
            var services = new ServiceCollection();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AddBookCommandHandler).Assembly));
            services.AddSingleton(_bookRepository);
            var provider = services.BuildServiceProvider();
            _mediator = provider.GetRequiredService<IMediator>();
        }

        [Test]
        public async Task When_Method_AddNewBook_isCalled_Then_BookAddedToList()
        {
            // Arrange
            Author author = new Author(Guid.NewGuid(), "Dr.Book McBookie");
            Book bookToTest = new Book(Guid.NewGuid(), "RobertBook", "Book of life", author);
            A.CallTo(() => _bookRepository.AddBook(bookToTest)).Returns(Task.FromResult(bookToTest));

            // Act
            OperationResult<Book> result = await _mediator.Send(new AddBookCommand(bookToTest));

            // Assert
            Assert.That(result.IsSuccessfull, Is.True);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Data.Description, Is.EqualTo(bookToTest.Description));
        }

        [Test]
        public async Task When_Method_AddNewBook_isCalled_With_EmptyTitle_Then_OperationResultFailure()
        {
            // Arrange
            Author author = new Author(Guid.NewGuid(), "Dr.Book McBookie");
            Book bookToTest = new Book(Guid.NewGuid(), "", "Description", author);

            // Act
            OperationResult<Book> result = await _mediator.Send(new AddBookCommand(bookToTest));

            // Assert
            Assert.That(result.IsSuccessfull, Is.False);
            Assert.That(result.Data, Is.Null);
            Assert.That(result.ErrorMessage, Is.EqualTo("Book title cannot be empty."));
        }
    }
}