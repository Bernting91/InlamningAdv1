using Application.Authors.Commands.UpdateAuthor;
using Application.Interfaces.RepositoryInterfaces;
using Domain;
using FakeItEasy;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace TestProject.Authors.Commands
{
    public class UpdateAuthorCommandHandlerTests
    {
        private IMediator _mediator;
        private IAuthorRepository _authorRepository;

        [SetUp]
        public void Setup()
        {
            _authorRepository = A.Fake<IAuthorRepository>();
            var services = new ServiceCollection();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UpdateAuthorCommandHandler).Assembly));
            services.AddSingleton(_authorRepository);
            var provider = services.BuildServiceProvider();
            _mediator = provider.GetRequiredService<IMediator>();
        }

        [Test]
        public async Task When_Method_UpdateAuthor_isCalled_Then_AuthorUpdated()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            Author authorToUpdate = new Author(authorId, "Original Name");
            A.CallTo(() => _authorRepository.GetAuthorById(authorId)).Returns(Task.FromResult(authorToUpdate));
            A.CallTo(() => _authorRepository.UpdateAuthor(authorId, A<Author>.Ignored)).Returns(Task.FromResult(new Author(authorId, "Updated Name")));

            // Act
            var updatedName = "Updated Name";
            OperationResult<Author> result = await _mediator.Send(new UpdateAuthorCommand(authorId, updatedName));

            // Assert
            Assert.That(result.IsSuccessfull, Is.True);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Data.Name, Is.EqualTo(updatedName));
            A.CallTo(() => _authorRepository.UpdateAuthor(authorId, A<Author>.Ignored)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task When_Method_UpdateAuthor_isCalled_With_InvalidAuthor_Then_AuthorNotUpdated()
        {
            // Arrange
            var invalidAuthorId = Guid.NewGuid();
            var invalidAuthorName = "Invalid Author";
            A.CallTo(() => _authorRepository.GetAuthorById(invalidAuthorId)).Returns(Task.FromResult<Author>(null));

            // Act
            OperationResult<Author> result = await _mediator.Send(new UpdateAuthorCommand(invalidAuthorId, invalidAuthorName));

            // Assert
            Assert.That(result.IsSuccessfull, Is.False);
            Assert.That(result.Data, Is.Null);
            Assert.That(result.ErrorMessage, Is.EqualTo("Author not found."));
        }
    }
}