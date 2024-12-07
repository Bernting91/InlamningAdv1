using Application.Authors.Queries.GetAuthorByID;
using Application.Interfaces.RepositoryInterfaces;
using Domain;
using FakeItEasy;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace TestProject.Authors.Queries
{
    public class GetAuthorByIdQueryHandlerTests
    {
        private IMediator _mediator;
        private IAuthorRepository _authorRepository;

        [SetUp]
        public void Setup()
        {
            _authorRepository = A.Fake<IAuthorRepository>();
            var services = new ServiceCollection();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAuthorByIdQueryHandler).Assembly));
            services.AddSingleton(_authorRepository);
            var provider = services.BuildServiceProvider();
            _mediator = provider.GetRequiredService<IMediator>();
        }

        [Test]
        public async Task When_Method_GetAuthorById_isCalled_Then_AuthorReturned()
        {
            // Arrange
            Author authorToFind = new Author(Guid.NewGuid(), "Author to Find");
            A.CallTo(() => _authorRepository.GetAuthorById(authorToFind.Id)).Returns(Task.FromResult<Author?>(authorToFind));

            // Act
            OperationResult<Author?> result = await _mediator.Send(new GetAuthorByIdQuery(authorToFind.Id));

            // Assert
            Assert.That(result.IsSuccessfull, Is.True);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Data!.Id, Is.EqualTo(authorToFind.Id));
        }

        [Test]
        public async Task When_Method_GetAuthorById_isCalled_With_InvalidId_Then_AuthorNotFound()
        {
            // Arrange
            A.CallTo(() => _authorRepository.GetAuthorById(A<Guid>.Ignored)).Returns(Task.FromResult<Author?>(null));

            // Act
            OperationResult<Author?> result = await _mediator.Send(new GetAuthorByIdQuery(Guid.NewGuid()));

            // Assert
            Assert.That(result.IsSuccessfull, Is.False);
            Assert.That(result.Data, Is.Null);
            Assert.That(result.ErrorMessage, Is.EqualTo("Author not found."));
        }
    }
}