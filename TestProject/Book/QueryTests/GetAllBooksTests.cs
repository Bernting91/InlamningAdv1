using Application.Books.Queries.GetAllBooks;
using Domain;
using Infrastructure.Database;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestProject.Books.Queries
{
    public class GetAllBooksTests
    {
        private FakeDatabase _fakeDatabase;
        private IMediator _mediator;

        [SetUp]
        public void Setup()
        {
            _fakeDatabase = new FakeDatabase();
            var services = new ServiceCollection();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllBooksQueryHandler).Assembly));
            services.AddSingleton(_fakeDatabase);
            var provider = services.BuildServiceProvider();
            _mediator = provider.GetRequiredService<IMediator>();
        }

        [Test]
        public async Task When_Method_GetAllBooks_isCalled_Then_AllBooksReturned()
        {
            // Arrange
            var expectedBooks = _fakeDatabase.Books;

            // Act
            IEnumerable<Book> booksReturned = await _mediator.Send(new GetAllBooksQuery());

            // Assert
            Assert.That(booksReturned, Is.Not.Null);
            Assert.That(booksReturned, Is.EquivalentTo(expectedBooks));
        }

        [Test]
        public void When_Method_GetAllBooks_isCalled_Then_NoBooksReturned()
        {
            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                IEnumerable<Book> booksReturned = await _mediator.Send(new GetAllBooksQuery());

                if (booksReturned.Any())
                {
                    throw new InvalidOperationException("Books were returned when none were expected.");
                }
            });
        }
    }
}