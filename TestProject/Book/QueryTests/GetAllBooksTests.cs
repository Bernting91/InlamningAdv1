using Application.Books.Queries.GetAllBooks;
using Application.Interfaces.RepositoryInterfaces;
using Domain;
using FakeItEasy;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestProject.Books.Queries
{
    public class GetAllBooksTests
    {
        private IMediator _mediator;
        private IBookRepository _bookRepository;

        [SetUp]
        public void Setup()
        {
            _bookRepository = A.Fake<IBookRepository>();
            var services = new ServiceCollection();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllBooksQueryHandler).Assembly));
            services.AddSingleton(_bookRepository);
            var provider = services.BuildServiceProvider();
            _mediator = provider.GetRequiredService<IMediator>();
        }

        [Test]
        public async Task When_Method_GetAllBooks_isCalled_Then_AllBooksReturned()
        {
            // Arrange
            var expectedBooks = new List<Book>
            {
                new Book(Guid.NewGuid(), "Book1", "Description1", new Author(Guid.NewGuid(), "Author1")),
                new Book(Guid.NewGuid(), "Book2", "Description2", new Author(Guid.NewGuid(), "Author2"))
            };
            A.CallTo(() => _bookRepository.GetAllBooks()).Returns(Task.FromResult(expectedBooks));

            // Act
            OperationResult<List<Book>> result = await _mediator.Send(new GetAllBooksQuery());

            // Assert
            Assert.That(result.IsSuccessfull, Is.True);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Data, Is.EquivalentTo(expectedBooks));
        }

        [Test]
        public async Task When_Method_GetAllBooks_isCalled_Then_NoBooksReturned()
        {
            // Arrange
            A.CallTo(() => _bookRepository.GetAllBooks()).Returns(Task.FromResult(new List<Book>()));

            // Act
            OperationResult<List<Book>> result = await _mediator.Send(new GetAllBooksQuery());

            // Assert
            Assert.That(result.IsSuccessfull, Is.False);
            Assert.That(result.Data, Is.Null);
            Assert.That(result.ErrorMessage, Is.EqualTo("No books found."));
        }
    }
}