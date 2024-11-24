using Application.Authors.Queries.GetAuthorByID;
using Domain;
using Infrastructure.Database;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace TestProject.Authors.Queries
{
    public class GetAuthorByIdQueryHandlerTests
    {
        private FakeDatabase _fakeDatabase;
        private IMediator _mediator;

        [SetUp]
        public void Setup()
        {
            _fakeDatabase = new FakeDatabase();
            var services = new ServiceCollection();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAuthorByIdQueryHandler).Assembly));
            services.AddSingleton(_fakeDatabase);
            var provider = services.BuildServiceProvider();
            _mediator = provider.GetRequiredService<IMediator>();
        }

        [Test]
        public async Task When_Method_GetAuthorById_isCalled_Then_AuthorReturned()
        {
            // Arrange
            Author authorToFind = new Author(1, "Author to Find");
            _fakeDatabase.Authors.Add(authorToFind);

            // Act
            Author? authorFound = await _mediator.Send(new GetAuthorByIdQuery(authorToFind.Id));

            // Assert
            Assert.That(authorFound, Is.Not.Null);
            Assert.That(authorFound.Id, Is.EqualTo(authorToFind.Id));
        }

        [Test]
        public void When_Method_GetAuthorById_isCalled_With_InvalidId_Then_ExceptionThrown()
        {
            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(() => _mediator.Send(new GetAuthorByIdQuery(-1)));
        }
    }
}