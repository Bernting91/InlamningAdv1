using Application.Interfaces.RepositoryInterfaces;
using Application.Users.Commands.RemoveUser;
using Domain;
using FakeItEasy;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace TestProject.User.Commands
{
    public class RemoveUserTests
    {
        private IMediator _mediator;
        private IUserRepository _userRepository;

        [SetUp]
        public void Setup()
        {
            _userRepository = A.Fake<IUserRepository>();
            var services = new ServiceCollection();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(RemoveUserCommandHandler).Assembly));
            services.AddSingleton(_userRepository);
            var provider = services.BuildServiceProvider();
            _mediator = provider.GetRequiredService<IMediator>();
        }

        [Test]
        public async Task When_Method_RemoveUser_isCalled_Then_UserRemovedFromList()
        {
            // Arrange
            var userId = Guid.NewGuid();
            A.CallTo(() => _userRepository.DeleteUserById(userId)).Returns(Task.FromResult("User Deleted"));

            // Act
            var result = await _mediator.Send(new RemoveUserCommand(userId));

            // Assert
            Assert.That(result.IsSuccessfull, Is.True);
            Assert.That(result.Data, Is.EqualTo("User Deleted"));
        }

        [Test]
        public async Task When_Method_RemoveUser_isCalled_With_NonExistentUser_Then_UserNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            A.CallTo(() => _userRepository.DeleteUserById(userId)).Returns(Task.FromResult("User Not Found"));

            // Act
            var result = await _mediator.Send(new RemoveUserCommand(userId));

            // Assert
            Assert.That(result.IsSuccessfull, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("User not found."));
        }

        [Test]
        public async Task When_Method_RemoveUser_isCalled_With_EmptyId_Then_FailureResult()
        {
            // Arrange
            var userId = Guid.Empty;

            // Act
            var result = await _mediator.Send(new RemoveUserCommand(userId));

            // Assert
            Assert.That(result.IsSuccessfull, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("Id cannot be empty."));
        }
    }
}