using Application.Authors.Queries.GetAllAuthors;
using Application.Interfaces.RepositoryInterfaces;
using Domain;
using FakeItEasy;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TestProject.Authors.Queries
{
    public class GetAllAuthorsQueryHandlerTests
    {
        private IMediator _mediator;
        private IAuthorRepository _authorRepository;

        [SetUp]
        public void Setup()
        {
            _authorRepository = A.Fake<IAuthorRepository>();
            var services = new ServiceCollection();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllAuthorsQueryHandler).Assembly));
            services.AddSingleton(_authorRepository);
            var provider = services.BuildServiceProvider();
            _mediator = provider.GetRequiredService<IMediator>();
        }

        [Test]
        public async Task When_Method_GetAllAuthors_isCalled_Then_AllAuthorsReturned()
        {
            // Arrange
            var expectedAuthors = new List<Author>
            {
                new Author(Guid.NewGuid(), "Author 1"),
                new Author(Guid.NewGuid(), "Author 2")
            };
            A.CallTo(() => _authorRepository.GetAllAuthors()).Returns(Task.FromResult(expectedAuthors));

            // Act
            OperationResult<IEnumerable<Author>> result = await _mediator.Send(new GetAllAuthorsQuery());

            // Assert
            Assert.That(result.IsSuccessfull, Is.True);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Data, Is.EquivalentTo(expectedAuthors));
        }

        [Test]
        public async Task When_Method_GetAllAuthors_isCalled_Then_NoAuthorsReturned()
        {
            // Arrange
            A.CallTo(() => _authorRepository.GetAllAuthors()).Returns(Task.FromResult<List<Author>>(null));

            // Act
            OperationResult<IEnumerable<Author>> result = await _mediator.Send(new GetAllAuthorsQuery());

            // Assert
            Assert.That(result.IsSuccessfull, Is.False);
            Assert.That(result.Data, Is.Null);
            Assert.That(result.ErrorMessage, Is.EqualTo("No authors found."));
        }
    }
}