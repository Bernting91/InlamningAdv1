using Application.Authors.Commands.UpdateAuthor;
using Domain;
using Infrastructure.Database;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace TestProject.Authors.Commands
{
    public class UpdateAuthorCommandHandlerTests
    {
        private FakeDatabase _fakeDatabase;
        private IMediator _mediator;

        [SetUp]
        public void Setup()
        {
            _fakeDatabase = new FakeDatabase();
            var services = new ServiceCollection();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UpdateAuthorCommandHandler).Assembly));
            services.AddSingleton(_fakeDatabase);
            var provider = services.BuildServiceProvider();
            _mediator = provider.GetRequiredService<IMediator>();
        }

        [Test]
        public async Task When_Method_UpdateAuthor_isCalled_Then_AuthorUpdated()
        {
            // Arrange
            Author authorToUpdate = new Author(1, "Original Name");
            _fakeDatabase.Authors.Add(authorToUpdate);

            // Act
            authorToUpdate.Name = "Updated Name";
            Author? authorUpdated = await _mediator.Send(new UpdateAuthorCommand(authorToUpdate));

            // Assert
            Assert.That(authorUpdated, Is.Not.Null);
            Assert.That(authorUpdated.Name, Is.EqualTo("Updated Name"));
        }

        [Test]
        public void When_Method_UpdateAuthor_isCalled_With_InvalidAuthor_Then_AuthorNotUpdated()
        {
            // Arrange
            Author? authorToUpdate = null;

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => _mediator.Send(new UpdateAuthorCommand(authorToUpdate)));
        }
    }
}