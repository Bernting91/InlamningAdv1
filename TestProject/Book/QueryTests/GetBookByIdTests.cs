using Application.Books.Commands.AddBook;
using Application.Books.Queries.GetbookbyID;
using Application.Interfaces.RepositoryInterfaces;
using Domain;
using FakeItEasy;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace TestProject.Books.Queries
{
    public class GetBookByIdTests
    {
        private IMediator _mediator;
        private IBookRepository _bookRepository;

        [SetUp]
        public void Setup()
        {
            _bookRepository = A.Fake<IBookRepository>();
            var services = new ServiceCollection();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetBookByIdQueryHandler).Assembly));
            services.AddSingleton(_bookRepository);
            var provider = services.BuildServiceProvider();
            _mediator = provider.GetRequiredService<IMediator>();
        }

        [Test]
        public async Task When_Method_GetBookById_isCalled_Then_BookReturned()
        {
            // Arrange
            Author author = new Author(Guid.NewGuid(), "Dr.Book McBookie");
            Book bookToTest = new Book(Guid.NewGuid(), "RobertCook", "Book of life", author);
            A.CallTo(() => _bookRepository.GetBookById(bookToTest.Id)).Returns(Task.FromResult(bookToTest));

            // Act
            OperationResult<Book?> result = await _mediator.Send(new GetBookByIdQuery(bookToTest.Id));

            // Assert
            Assert.That(result.IsSuccessfull, Is.True);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Data, Is.EqualTo(bookToTest));
        }

        [Test]
        public async Task When_Method_GetBookById_isCalled_With_InvalidBook_Then_BookNotReturned()
        {
            // Arrange
            Guid invalidId = Guid.NewGuid();
            A.CallTo(() => _bookRepository.GetBookById(invalidId)).Returns(Task.FromResult<Book?>(null));

            // Act
            OperationResult<Book?> result = await _mediator.Send(new GetBookByIdQuery(invalidId));

            // Assert
            Assert.That(result.IsSuccessfull, Is.False);
            Assert.That(result.Data, Is.Null);
            Assert.That(result.ErrorMessage, Is.EqualTo("Book not found."));
        }

        [Test]
        public async Task When_Method_GetBookById_isCalled_With_EmptyId_Then_FailureResult()
        {
            // Arrange
            Guid emptyId = Guid.Empty;

            // Act
            OperationResult<Book?> result = await _mediator.Send(new GetBookByIdQuery(emptyId));

            // Assert
            Assert.That(result.IsSuccessfull, Is.False);
            Assert.That(result.Data, Is.Null);
            Assert.That(result.ErrorMessage, Is.EqualTo("Id cannot be empty."));
        }
    }
}