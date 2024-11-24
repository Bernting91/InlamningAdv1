using Application.Authors.Commands.AddAuthor;
using Domain;
using Infrastructure.Database;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace TestProject.Authors.Commands
{
    public class AddAuthorCommandHandlerTests
    {
        private FakeDatabase _fakeDatabase;
        private IMediator _mediator;

        [SetUp]
        public void Setup()
        {
            _fakeDatabase = new FakeDatabase();
            var services = new ServiceCollection();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AddAuthorCommandHandler).Assembly));
            services.AddSingleton(_fakeDatabase);
            var provider = services.BuildServiceProvider();
            _mediator = provider.GetRequiredService<IMediator>();
        }

        [Test]
        public async Task When_Method_AddAuthor_isCalled_Then_AuthorAdded()
        {
            // Arrange
            Author authorToAdd = new Author(1, "New Author");

            // Act
            Author? authorAdded = await _mediator.Send(new AddAuthorCommand(authorToAdd));

            // Assert
            Assert.That(authorAdded, Is.Not.Null);
            Assert.That(_fakeDatabase.Authors, Contains.Item(authorToAdd));
        }

        [Test]
        public void When_Method_AddAuthor_isCalled_With_InvalidAuthor_Then_AuthorNotAdded()
        {
            // Arrange
            Author? authorToAdd = null;

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => _mediator.Send(new AddAuthorCommand(authorToAdd)));
        }
    }
}