using Application.Interfaces.RepositoryInterfaces;
using Application.Users.Queries.GetAllUsers;
using Domain;
using FakeItEasy;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TestProject.User.Queries
{
    public class GetAllUsersTests
    {
        private IMediator _mediator;
        private IUserRepository _userRepository;

        [SetUp]
        public void Setup()
        {
            _userRepository = A.Fake<IUserRepository>();
            var services = new ServiceCollection();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllUsersQueryHandler).Assembly));
            services.AddSingleton(_userRepository);
            var provider = services.BuildServiceProvider();
            _mediator = provider.GetRequiredService<IMediator>();
        }

        [Test]
        public async Task When_Method_GetAllUsers_isCalled_Then_AllUsersAreReturned()
        {
            // Arrange
            var users = new List<Domain.User>
            {
                new Domain.User { Id = Guid.NewGuid(), UserName = "User1", Password = "Password1" },
                new Domain.User { Id = Guid.NewGuid(), UserName = "User2", Password = "Password2" }
            };

            A.CallTo(() => _userRepository.GetAllUsers()).Returns(Task.FromResult(users));

            // Act
            var result = await _mediator.Send(new GetAllUsersQuery());

            // Assert
            Assert.That(result.IsSuccessfull, Is.True);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Data.Count, Is.EqualTo(users.Count));
        }

        [Test]
        public async Task When_Method_GetAllUsers_isCalled_And_ExceptionOccurs_Then_FailureResultIsReturned()
        {
            // Arrange
            A.CallTo(() => _userRepository.GetAllUsers()).Throws<Exception>();

            // Act
            var result = await _mediator.Send(new GetAllUsersQuery());

            // Assert
            Assert.That(result.IsSuccessfull, Is.False);
            Assert.That(result.ErrorMessage, Is.Not.Null);
        }
    }
}