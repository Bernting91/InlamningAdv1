using Application.Authors.Commands.RemoveAuthor;
using Domain;
using Infrastructure.Database;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace TestProject.Authors.Commands
{
    public class RemoveAuthorCommandHandlerTests
    {
        private FakeDatabase _fakeDatabase;
        private IMediator _mediator;

        [SetUp]
        public void Setup()
        {
            _fakeDatabase = new FakeDatabase();
            var services = new ServiceCollection();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(RemoveAuthorCommandHandler).Assembly));
            services.AddSingleton(_fakeDatabase);
            var provider = services.BuildServiceProvider();
            _mediator = provider.GetRequiredService<IMediator>();
        }

        [Test]
        public async Task When_Method_RemoveAuthor_isCalled_Then_AuthorRemoved()
        {
            // Arrange
            Author authorToRemove = new Author(Guid.NewGuid(), "Author to Remove");
            _fakeDatabase.Authors.Add(authorToRemove);

            // Act
            OperationResult<Author> result = await _mediator.Send(new RemoveAuthorCommand(authorToRemove.Id));

            // Assert
            Assert.That(result.IsSuccessfull, Is.True);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(_fakeDatabase.Authors, Does.Not.Contain(authorToRemove));
        }

        [Test]
        public async Task When_Method_RemoveAuthor_isCalled_With_InvalidAuthor_Then_AuthorNotRemoved()
        {
            // Arrange
            Guid invalidAuthorId = Guid.NewGuid();

            // Act
            OperationResult<Author> result = await _mediator.Send(new RemoveAuthorCommand(invalidAuthorId));

            // Assert
            Assert.That(result.IsSuccessfull, Is.False);
            Assert.That(result.Data, Is.Null);
            Assert.That(result.ErrorMessage, Is.EqualTo("Author not found"));
        }
    }
}