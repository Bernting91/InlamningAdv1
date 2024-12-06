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
            OperationResult<List<Book>> result = await _mediator.Send(new GetAllBooksQuery());

            // Assert
            Assert.That(result.IsSuccessfull, Is.True);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Data, Is.EquivalentTo(expectedBooks));
        }

        [Test]
        public void When_Method_GetAllBooks_isCalled_Then_NoBooksReturned()
        {
            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                OperationResult<List<Book>> result = await _mediator.Send(new GetAllBooksQuery());

                if (result.Data.Any())
                {
                    throw new InvalidOperationException("Books were returned when none were expected.");
                }
            });
        }
    }
}