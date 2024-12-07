using Application.Authors.Commands.AddAuthor;
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
    public class AddAuthorCommandHandlerTests
    {
        private IMediator _mediator;
        private IAuthorRepository _authorRepository;

        [SetUp]
        public void Setup()
        {
            _authorRepository = A.Fake<IAuthorRepository>();
            var services = new ServiceCollection();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AddAuthorCommandHandler).Assembly));
            services.AddSingleton(_authorRepository);
            var provider = services.BuildServiceProvider();
            _mediator = provider.GetRequiredService<IMediator>();
        }

        [Test]
        public async Task When_Method_AddAuthor_isCalled_Then_AuthorAdded()
        {
            // Arrange
            Author authorToAdd = new Author(Guid.NewGuid(), "New Author");
            A.CallTo(() => _authorRepository.AddAuthor(A<Author>.Ignored)).Returns(Task.FromResult(authorToAdd));

            // Act
            OperationResult<Author> result = await _mediator.Send(new AddAuthorCommand(authorToAdd));

            // Assert
            Assert.That(result.IsSuccessfull, Is.True);
            Assert.That(result.Data, Is.Not.Null);
            A.CallTo(() => _authorRepository.AddAuthor(authorToAdd)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task When_AddAuthorCommandHandler_isCalled_Then_AuthorIsAddedToRepository()
        {
            // Arrange
            Author authorToAdd = new Author(Guid.NewGuid(), "New Author");
            A.CallTo(() => _authorRepository.AddAuthor(A<Author>.Ignored)).Returns(Task.FromResult(authorToAdd));

            // Act
            OperationResult<Author> result = await _mediator.Send(new AddAuthorCommand(authorToAdd));

            // Assert
            Assert.That(result.IsSuccessfull, Is.True);
            Assert.That(result.Data, Is.Not.Null);
            A.CallTo(() => _authorRepository.AddAuthor(authorToAdd)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task When_AddAuthorCommandHandler_isCalled_With_ExistingAuthor_Then_FailureResultIsReturned()
        {
            // Arrange
            Author existingAuthor = new Author(Guid.NewGuid(), "Existing Author");
            A.CallTo(() => _authorRepository.GetAuthorById(existingAuthor.Id)).Returns(Task.FromResult(existingAuthor));

            // Act
            OperationResult<Author> result = await _mediator.Send(new AddAuthorCommand(existingAuthor));

            // Assert
            Assert.That(result.IsSuccessfull, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("Author already exists."));
        }
    }
}