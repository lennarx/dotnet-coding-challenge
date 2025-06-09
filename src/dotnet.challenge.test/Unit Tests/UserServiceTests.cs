using dotnet.challenge.api.Errors;
using dotnet.challenge.api.Services.Users;
using dotnet.challenge.api.Utils.Forms;
using dotnet.challenge.data.Cache;
using dotnet.challenge.data.Entities;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace dotnet.challenge.test.Unit_Tests
{
    public class UserServiceTests
    {
        private readonly Mock<ISimpleObjectCache<Guid, User>> _mockCache;
        private readonly Mock<ILogger<UserService>> _mockLogger;
        private readonly UserService _service;

        public UserServiceTests()
        {
            _mockCache = new Mock<ISimpleObjectCache<Guid, User>>();
            _mockLogger = new Mock<ILogger<UserService>>();
            _service = new UserService(_mockCache.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task CreateUserAsync_ShouldReturnFailure_WhenEmailAlreadyExists()
        {
            var existingUser = new User { Email = "test@example.com" };
            var userForm = new UserForm { Email = "test@example.com" };
            _mockCache.Setup(c => c.GetAllAsync()).ReturnsAsync(new List<User> { existingUser });

            var result = await _service.CreateUserAsync(userForm);

            Assert.True(result.Error != null);
            Assert.Equal(UserErrors.UserEmailIsAlreadyRegistered, result.Error);
        }

        [Fact]
        public async Task CreateUserAsync_ShouldSucceed_WhenEmailIsUnique()
        {
            var userForm = new UserForm
            {
                Email = "new@example.com",
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = "1990-01-01"
            };
            _mockCache.Setup(c => c.GetAllAsync()).ReturnsAsync(new List<User>());
            _mockCache.Setup(c => c.AddAsync(It.IsAny<Guid>(), It.IsAny<User>())).Returns(Task.CompletedTask);

            var result = await _service.CreateUserAsync(userForm);

            Assert.True(result.Value != null);
            Assert.Equal(userForm.Email, result.Value.Email);
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldReturnFailure_WhenUserNotFound()
        {
            _mockCache.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsAsync((User)null);

            var result = await _service.DeleteUserAsync(Guid.NewGuid());

            Assert.True(result.Error != null);
            Assert.Equal(UserErrors.UserNotFound, result.Error);
        }

        [Fact]
        public async Task GetUserAsync_ShouldReturnUser_WhenFound()
        {
            var user = new User { Email = "test@example.com" };
            _mockCache.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsAsync(user);

            var result = await _service.GetUserAsync(Guid.NewGuid());

            Assert.True(result.Value != null);
            Assert.Equal("test@example.com", result.Value.Email);
        }

        [Fact]
        public async Task GetUsersAsync_ShouldReturnFailure_WhenInvalidPagination()
        {
            var result = await _service.GetUsersAsnyc(0, 10);

            Assert.True(result.Error != null);
            Assert.Equal(UserErrors.InvalidPagination, result.Error);
        }

        [Fact]
        public async Task UpdateUserAsync_ShouldReturnFailure_WhenUserNotFound()
        {
            _mockCache.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsAsync((User)null);

            var result = await _service.UpdateUserAsync(Guid.NewGuid(), new UserForm());

            Assert.True(result.Error != null);
            Assert.Equal(UserErrors.UserNotFound, result.Error);
        }
    }
}
