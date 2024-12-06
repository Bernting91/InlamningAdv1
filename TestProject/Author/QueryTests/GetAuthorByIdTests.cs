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
            Author authorToFind = new Author(Guid.NewGuid(), "Author to Find");
            _fakeDatabase.Authors.Add(authorToFind);

            // Act
            OperationResult<Author> result = await _mediator.Send(new GetAuthorByIdQuery(authorToFind.Id));

            // Assert
            Assert.That(result.IsSuccessfull, Is.True);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Data.Id, Is.EqualTo(authorToFind.Id));
        }

        [Test]
        public async Task When_Method_GetAuthorById_isCalled_With_InvalidId_Then_AuthorNotFound()
        {
            // Act
            OperationResult<Author> result = await _mediator.Send(new GetAuthorByIdQuery(Guid.NewGuid()));

            // Assert
            Assert.That(result.IsSuccessfull, Is.False);
            Assert.That(result.Data, Is.Null);
            Assert.That(result.ErrorMessage, Is.EqualTo("Author not found"));
        }
    }
}