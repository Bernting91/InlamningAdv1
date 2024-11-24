using Application.Authors.Queries.GetAllAuthors;
using Domain;
using Infrastructure.Database;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestProject.Authors.Queries
{
    public class GetAllAuthorsQueryHandlerTests
    {
        private FakeDatabase _fakeDatabase;
        private IMediator _mediator;

        [SetUp]
        public void Setup()
        {
            _fakeDatabase = new FakeDatabase();
            var services = new ServiceCollection();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllAuthorsQueryHandler).Assembly));
            services.AddSingleton(_fakeDatabase);
            var provider = services.BuildServiceProvider();
            _mediator = provider.GetRequiredService<IMediator>();
        }

        [Test]
        public async Task When_Method_GetAllAuthors_isCalled_Then_AllAuthorsReturned()
        {
            // Arrange
            var expectedAuthors = _fakeDatabase.Authors;

            // Act
            IEnumerable<Author> authorsReturned = await _mediator.Send(new GetAllAuthorsQuery());

            // Assert
            Assert.That(authorsReturned, Is.Not.Null);
            Assert.That(authorsReturned, Is.EquivalentTo(expectedAuthors));
        }

        [Test]
        public void When_Method_GetAllAuthors_isCalled_Then_NoAuthorsReturned()
        {
            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                IEnumerable<Author> authorsReturned = await _mediator.Send(new GetAllAuthorsQuery());

                if (authorsReturned.Any())
                {
                    throw new InvalidOperationException("Authors were returned when none were expected.");
                }
            });
        }
    }
}