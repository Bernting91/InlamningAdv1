using Application.Users.Queries.Login;
using Application.Users.Queries.Login.Helpers;
using Application.Interfaces.RepositoryInterfaces;
using FakeItEasy;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Application.Dtos;

namespace TestProject.User.Queries
{
    public class LoginTests
    {
        private IMediator _mediator;
        private IUserRepository _userRepository;
        private TokenHelper _tokenHelper;

        [SetUp]
        public void Setup()
        {
            _userRepository = A.Fake<IUserRepository>();

            // Load configuration from appsettings.json
            IConfiguration configuration = new ConfigurationBuilder().AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"), optional: false, reloadOnChange: true).Build();

            _tokenHelper = new TokenHelper(configuration);

            var services = new ServiceCollection();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(LoginUserQueryHandler).Assembly));
            services.AddSingleton(_userRepository);
            services.AddSingleton(_tokenHelper);
            var provider = services.BuildServiceProvider();
            _mediator = provider.GetRequiredService<IMediator>();
        }

        [Test]
        public async Task When_ValidCredentials_Then_ReturnsToken()
        {
            // Arrange
            var userDto = new UserDto { UserName = "testuser", Password = "password" };
            var user = new Domain.User { Id = Guid.NewGuid(), UserName = userDto.UserName, Password = userDto.Password };

            A.CallTo(() => _userRepository.LoginUser(A<Domain.User>.That.Matches(u => u.UserName == userDto.UserName && u.Password == userDto.Password)))
                .Returns(Task.FromResult(user));

            var query = new LoginUserQuery(userDto);

            // Act
            var result = await _mediator.Send(query);

            // Assert
            Assert.That(result.IsSuccessfull, Is.True, "Expected the login to be successful.");
            Assert.That(result.Data, Is.Not.Null.And.Not.Empty, "Expected a non-null and non-empty token.");
        }

        [Test]
        public async Task When_InvalidCredentials_Then_ReturnsFailure()
        {
            // Arrange
            var userDto = new UserDto { UserName = "invaliduser", Password = "wrongpassword" };

            A.CallTo(() => _userRepository.LoginUser(A<Domain.User>.That.Matches(u => u.UserName == userDto.UserName && u.Password == userDto.Password)))
                .Returns(Task.FromResult<Domain.User?>(null));

            var query = new LoginUserQuery(userDto);

            // Act
            var result = await _mediator.Send(query);

            // Assert
            Assert.That(result.IsSuccessfull, Is.False, "Expected the login to fail.");
            Assert.That(result.ErrorMessage, Is.EqualTo("Invalid username or password"), "Expected an invalid username or password error message.");
        }

        [Test]
        public async Task When_ExceptionOccurs_Then_ReturnsFailure()
        {
            // Arrange
            var userDto = new UserDto { UserName = "testuser", Password = "password" };

            A.CallTo(() => _userRepository.LoginUser(A<Domain.User>.That.Matches(u => u.UserName == userDto.UserName && u.Password == userDto.Password)))
                .Throws<Exception>();

            var query = new LoginUserQuery(userDto);

            // Act
            var result = await _mediator.Send(query);

            // Assert
            Assert.That(result.IsSuccessfull, Is.False, "Expected the login to fail due to an exception.");
            Assert.That(result.ErrorMessage, Is.Not.Null.And.Contains("An error occurred while logging in"), "Expected an error message indicating an exception.");
        }
    }
}