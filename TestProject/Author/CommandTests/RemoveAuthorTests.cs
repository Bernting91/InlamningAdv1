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
            Author authorToRemove = new Author(1, "Author to Remove");
            _fakeDatabase.Authors.Add(authorToRemove);

            // Act
            Author? authorRemoved = await _mediator.Send(new RemoveAuthorCommand(authorToRemove));

            // Assert
            Assert.That(authorRemoved, Is.Not.Null);
            Assert.That(_fakeDatabase.Authors, Does.Not.Contain(authorToRemove));
        }

        [Test]
        public void When_Method_RemoveAuthor_isCalled_With_InvalidAuthor_Then_AuthorNotRemoved()
        {
            // Arrange
            Author? authorToRemove = null;

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => _mediator.Send(new RemoveAuthorCommand(authorToRemove)));
        }
    }
}