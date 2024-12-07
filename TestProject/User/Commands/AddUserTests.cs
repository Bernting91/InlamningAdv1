using Application.Dtos;
using Application.Interfaces.RepositoryInterfaces;
using Application.Users.Commands.AddUser;
using Domain;
using FakeItEasy;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace TestProject.User.Commands
{
    public class AddUserTests
    {
        private IMediator _mediator;
        private IUserRepository _userRepository;

        [SetUp]
        public void Setup()
        {
            _userRepository = A.Fake<IUserRepository>();
            var services = new ServiceCollection();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AddNewUserCommandHandler).Assembly));
            services.AddSingleton(_userRepository);
            var provider = services.BuildServiceProvider();
            _mediator = provider.GetRequiredService<IMediator>();
        }

        [Test]
        public async Task When_Method_AddNewUser_isCalled_Then_UserAddedToList()
        {
            // Arrange
            var userDto = new UserDto { UserName = "TestUser", Password = "Password123" };
            var userToTest = new Domain.User { Id = Guid.NewGuid(), UserName = userDto.UserName, Password = userDto.Password };
            A.CallTo(() => _userRepository.AddUser(A<Domain.User>.Ignored)).Returns(Task.FromResult(userToTest));

            // Act
            var result = await _mediator.Send(new AddNewUserCommand(userDto));

            // Assert
            Assert.That(result.IsSuccessfull, Is.True);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Data.UserName, Is.EqualTo(userToTest.UserName));
        }

        [Test]
        public async Task When_Method_AddNewUser_isCalled_With_EmptyUserName_Then_FailureResult()
        {
            // Arrange
            var userDto = new UserDto { UserName = "", Password = "Password123" };

            // Act
            var result = await _mediator.Send(new AddNewUserCommand(userDto));

            // Assert
            Assert.That(result.IsSuccessfull, Is.False);
            Assert.That(result.Data, Is.Null);
            Assert.That(result.ErrorMessage, Is.EqualTo("UserName cannot be empty."));
        }

        [Test]
        public async Task IntegrationTest_AddNewUser()
        {
            // Arrange
            var userDto = new UserDto { UserName = "IntegrationTestUser", Password = "Password123" };
            var userToTest = new Domain.User { Id = Guid.NewGuid(), UserName = userDto.UserName, Password = userDto.Password };
            A.CallTo(() => _userRepository.AddUser(A<Domain.User>.Ignored)).Returns(Task.FromResult(userToTest));

            // Act
            var result = await _mediator.Send(new AddNewUserCommand(userDto));

            // Assert
            Assert.That(result.IsSuccessfull, Is.True);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Data.UserName, Is.EqualTo(userToTest.UserName));
        }
    }
}