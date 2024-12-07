using Application.Interfaces.RepositoryInterfaces;
using Application.Users.Queries.GetUserById;
using Domain;
using FakeItEasy;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TestProject.User.Queries
{
    public class GetUserByIdTests
    {
        private IMediator _mediator;
        private IUserRepository _userRepository;

        [SetUp]
        public void Setup()
        {
            _userRepository = A.Fake<IUserRepository>();
            var services = new ServiceCollection();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetUserByIdQueryHandler).Assembly));
            services.AddSingleton(_userRepository);
            var provider = services.BuildServiceProvider();
            _mediator = provider.GetRequiredService<IMediator>();
        }

        [Test]
        public async Task When_Method_GetUserById_isCalled_With_ValidId_Then_UserIsReturned()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new Domain.User { Id = userId, UserName = "TestUser", Password = "TestPassword" };

            A.CallTo(() => _userRepository.GetUserById(userId)).Returns(Task.FromResult(user));

            // Act
            var result = await _mediator.Send(new GetUserByIdQuery(userId));

            // Assert
            Assert.That(result.IsSuccessfull, Is.True);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Data.Id, Is.EqualTo(userId));
        }

        [Test]
        public async Task When_Method_GetUserById_isCalled_With_InvalidId_Then_FailureResultIsReturned()
        {
            // Arrange
            var userId = Guid.NewGuid();

            A.CallTo(() => _userRepository.GetUserById(userId)).Returns(Task.FromResult<Domain.User>(null));

            // Act
            var result = await _mediator.Send(new GetUserByIdQuery(userId));

            // Assert
            Assert.That(result.IsSuccessfull, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("User not found."));
        }

        [Test]
        public async Task When_Method_GetUserById_isCalled_With_EmptyId_Then_FailureResultIsReturned()
        {
            // Arrange
            var userId = Guid.Empty;

            // Act
            var result = await _mediator.Send(new GetUserByIdQuery(userId));

            // Assert
            Assert.That(result.IsSuccessfull, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("User Id is required."));
        }

        [Test]
        public async Task When_Method_GetUserById_isCalled_And_ExceptionOccurs_Then_FailureResultIsReturned()
        {
            // Arrange
            var userId = Guid.NewGuid();

            A.CallTo(() => _userRepository.GetUserById(userId)).Throws<Exception>();

            // Act
            var result = await _mediator.Send(new GetUserByIdQuery(userId));

            // Assert
            Assert.That(result.IsSuccessfull, Is.False);
            Assert.That(result.ErrorMessage, Is.Not.Null);
        }
    }
}