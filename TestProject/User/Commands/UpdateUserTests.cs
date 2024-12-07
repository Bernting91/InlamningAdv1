using Application.Dtos;
using Application.Interfaces.RepositoryInterfaces;
using Application.Users.Commands.UpdateUser;
using Domain;
using FakeItEasy;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace TestProject.User.Commands
{
    public class UpdateUserTests
    {
        private IMediator _mediator;
        private IUserRepository _userRepository;

        [SetUp]
        public void Setup()
        {
            _userRepository = A.Fake<IUserRepository>();
            var services = new ServiceCollection();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UpdateUserCommandHandler).Assembly));
            services.AddSingleton(_userRepository);
            var provider = services.BuildServiceProvider();
            _mediator = provider.GetRequiredService<IMediator>();
        }

        [Test]
        public async Task When_Method_UpdateUser_isCalled_Then_UserIsUpdated()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userDto = new UserDto { UserName = "UpdatedUser", Password = "UpdatedPassword123" };
            var existingUser = new Domain.User { Id = userId, UserName = "OldUser", Password = "OldPassword123" };
            var updatedUser = new Domain.User { Id = userId, UserName = userDto.UserName, Password = userDto.Password };

            A.CallTo(() => _userRepository.GetUserById(userId)).Returns(Task.FromResult(existingUser));
            A.CallTo(() => _userRepository.UpdateUser(userId, A<Domain.User>.Ignored)).Returns(Task.FromResult(updatedUser));

            var user = new Domain.User
            {
                Id = userId,
                UserName = userDto.UserName,
                Password = userDto.Password
            };

            // Act
            var result = await _mediator.Send(new UpdateUserCommand(userId, user));

            // Assert
            Assert.That(result.IsSuccessfull, Is.True);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Data.UserName, Is.EqualTo(updatedUser.UserName));
        }

        [Test]
        public async Task When_Method_UpdateUser_isCalled_With_NonExistentUser_Then_FailureResult()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userDto = new UserDto { UserName = "NonExistentUser", Password = "Password123" };

            A.CallTo(() => _userRepository.GetUserById(userId)).Returns(Task.FromResult<Domain.User>(null));

            var user = new Domain.User
            {
                Id = userId,
                UserName = userDto.UserName,
                Password = userDto.Password
            };

            // Act
            var result = await _mediator.Send(new UpdateUserCommand(userId, user));

            // Assert
            Assert.That(result.IsSuccessfull, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("User not found."));
        }

        [Test]
        public async Task IntegrationTest_UpdateUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userDto = new UserDto { UserName = "IntegrationTestUser", Password = "Password123" };
            var existingUser = new Domain.User { Id = userId, UserName = "OldUser", Password = "OldPassword123" };
            var updatedUser = new Domain.User { Id = userId, UserName = userDto.UserName, Password = userDto.Password };

            A.CallTo(() => _userRepository.GetUserById(userId)).Returns(Task.FromResult(existingUser));
            A.CallTo(() => _userRepository.UpdateUser(userId, A<Domain.User>.Ignored)).Returns(Task.FromResult(updatedUser));

            var user = new Domain.User
            {
                Id = userId,
                UserName = userDto.UserName,
                Password = userDto.Password
            };

            // Act
            var result = await _mediator.Send(new UpdateUserCommand(userId, user));

            // Assert
            Assert.That(result.IsSuccessfull, Is.True);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Data.UserName, Is.EqualTo(updatedUser.UserName));
        }
    }
}