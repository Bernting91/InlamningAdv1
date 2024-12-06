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
            Author authorToAdd = new Author(Guid.NewGuid(), "New Author");

            // Act
            OperationResult<Author> result = await _mediator.Send(new AddAuthorCommand(authorToAdd));

            // Assert
            Assert.That(result.IsSuccessfull, Is.True);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(_fakeDatabase.Authors, Contains.Item(authorToAdd));
        }

        [Test]
        public void When_Method_AddAuthor_isCalled_With_InvalidAuthor_Then_AuthorNotAdded()
        {
            // Arrange
            Author? authorToAdd = null;

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentNullException>(() => _mediator.Send(new AddAuthorCommand(authorToAdd)));
            Assert.That(ex.ParamName, Is.EqualTo("author"));
        }
    }
}