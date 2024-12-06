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
            OperationResult<IEnumerable<Author>> result = await _mediator.Send(new GetAllAuthorsQuery());

            // Assert
            Assert.That(result.IsSuccessfull, Is.True);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Data, Is.EquivalentTo(expectedAuthors));
        }

        [Test]
        public async Task When_Method_GetAllAuthors_isCalled_Then_NoAuthorsReturned()
        {
            // Arrange
            _fakeDatabase.Authors.Clear();

            // Act
            OperationResult<IEnumerable<Author>> result = await _mediator.Send(new GetAllAuthorsQuery());

            // Assert
            Assert.That(result.IsSuccessfull, Is.False);
            Assert.That(result.Data, Is.Null);
            Assert.That(result.ErrorMessage, Is.EqualTo("No authors found"));
        }
    }
}