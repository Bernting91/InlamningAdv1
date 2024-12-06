using Application.Books.Commands.AddBook;
using Application.Books.Queries.GetbookbyID;
using Domain;
using Infrastructure.Database;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace TestProject.Books.Queries
{
    public class GetBookByIdTests
    {
        private FakeDatabase _fakeDatabase;
        private IMediator _mediator;

        [SetUp]
        public void Setup()
        {
            _fakeDatabase = new FakeDatabase();
            var services = new ServiceCollection();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetBookByIdQueryHandler).Assembly));
            services.AddSingleton(_fakeDatabase);
            var provider = services.BuildServiceProvider();
            _mediator = provider.GetRequiredService<IMediator>();
        }

        [Test]
        public async Task When_Method_GetBookById_isCalled_Then_BookReturned()
        {
            // Arrange
            Author author = new Author(Guid.NewGuid(), "Dr.Book McBookie");
            Book bookToTest = new Book(Guid.NewGuid(), "RobertCook", "Book of life", author);
            await _mediator.Send(new AddBookCommand(bookToTest));

            // Act
            OperationResult<Book> result = await _mediator.Send(new GetBookByIdQuery(bookToTest.Id));

            // Assert
            Assert.That(result.IsSuccessfull, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }

        [Test]
        public void When_Method_GetBookById_isCalled_With_InvalidBook_Then_BookNotReturned()
        {
            // Arrange
            Guid invalidId = Guid.NewGuid();

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(() => _mediator.Send(new GetBookByIdQuery(invalidId)));
        }
    }
}