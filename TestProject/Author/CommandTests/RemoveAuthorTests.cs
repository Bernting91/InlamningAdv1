using Application.Authors.Commands.RemoveAuthor;
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
    public class RemoveAuthorCommandHandlerTests
    {
        private IMediator _mediator;
        private IAuthorRepository _authorRepository;

        [SetUp]
        public void Setup()
        {
            _authorRepository = A.Fake<IAuthorRepository>();
            var services = new ServiceCollection();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(RemoveAuthorCommandHandler).Assembly));
            services.AddSingleton(_authorRepository);
            var provider = services.BuildServiceProvider();
            _mediator = provider.GetRequiredService<IMediator>();
        }

        [Test]
        public async Task When_Method_RemoveAuthor_isCalled_Then_AuthorRemoved()
        {
            // Arrange
            Author authorToRemove = new Author(Guid.NewGuid(), "Author to Remove");
            A.CallTo(() => _authorRepository.GetAuthorById(authorToRemove.Id)).Returns(Task.FromResult(authorToRemove));
            A.CallTo(() => _authorRepository.DeleteAuthorById(authorToRemove.Id)).Returns(Task.FromResult("Author removed"));

            // Act
            OperationResult<Author> result = await _mediator.Send(new RemoveAuthorCommand(authorToRemove.Id));

            // Assert
            Assert.That(result.IsSuccessfull, Is.True);
            Assert.That(result.Data, Is.Not.Null);
            A.CallTo(() => _authorRepository.DeleteAuthorById(authorToRemove.Id)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task When_Method_RemoveAuthor_isCalled_With_InvalidAuthor_Then_AuthorNotRemoved()
        {
            // Arrange
            Guid invalidAuthorId = Guid.NewGuid();
            A.CallTo(() => _authorRepository.GetAuthorById(invalidAuthorId)).Returns(Task.FromResult<Author>(null));

            // Act
            OperationResult<Author> result = await _mediator.Send(new RemoveAuthorCommand(invalidAuthorId));

            // Assert
            Assert.That(result.IsSuccessfull, Is.False);
            Assert.That(result.Data, Is.Null);
            Assert.That(result.ErrorMessage, Is.EqualTo("Author not found."));
        }
    }
}