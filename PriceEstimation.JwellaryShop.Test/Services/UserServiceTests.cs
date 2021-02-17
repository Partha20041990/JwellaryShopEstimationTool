using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using PriceEstimation.JwellaryShop.WebApi.Entities;
using PriceEstimation.JwellaryShop.WebApi.Helpers;
using PriceEstimation.JwellaryShop.WebApi.Repositories.Interfaces;
using PriceEstimation.JwellaryShop.WebApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PriceEstimation.JwellaryShop.Test.Services
{
    public class UserServiceTests
    {
        private MockRepository mockRepository;

        private IOptions<AppSettings> mockOptions;
        private Mock<IUserRepository> mockUserRepository;
        private Mock<ILogger<UserService>> mockLogger;
        private readonly IConfigurationRoot _config;

        public UserServiceTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Loose);

            _config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

            var userList = new List<User>();
            userList.Add(new User
            {
                Id = 1,
                FirstName = "Privileged",
                LastName = "User",
                Username = "admin",
                Password = "admin",
                Role = Role.Privileged
            });
            userList.Add(new User
            {
                Id = 2,
                FirstName = "Regular",
                LastName = "User",
                Username = "user",
                Password = "user",
                Role = Role.Regular
            });

            var optionValue = _config.GetSection("AppSettings").Get<AppSettings>();
            mockOptions = Options.Create(optionValue);
            //this.mockOptions.Se
            this.mockUserRepository = this.mockRepository.Create<IUserRepository>();
            this.mockUserRepository.Setup(p => p.GetAll()).Returns(userList.AsQueryable().OrderBy(s=>s.Id));
            this.mockLogger = this.mockRepository.Create<ILogger<UserService>>();
            mockLogger.Setup(
            p => p.Log(It.IsAny<LogLevel>(),
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => true),
            It.IsAny<Exception>(),
            It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true))).Verifiable();
        }

        private UserService CreateService()
        {
            return new UserService(
                this.mockOptions,
                this.mockUserRepository.Object,
                this.mockLogger.Object);
        }

        [Fact]
        public void Authenticate_PrivilegedUser_ShouldReturnAdminUser()
        {
            // Arrange
            var service = this.CreateService();
            string username = "admin";
            string password = "admin";

            // Act
            var result = service.Authenticate(
                username,
                password);

            // Assert
            Assert.True(result.Id ==1);
            Assert.True(result.FirstName == "Privileged");
            this.mockRepository.Verify();
        }

        [Fact]
        public void GetAll_UsersCount_ShouldMatchUsersCount()
        {
            // Arrange
            var service = this.CreateService();

            // Act
            var result = service.GetAll();

            // Assert
            Assert.True(result.Count()==2);
            this.mockRepository.Verify();
        }

        [Fact]
        public void Authenticate_InvalidUser_ShouldReturnNull()
        {
            // Arrange
            var service = this.CreateService();
            string username = "partha";
            string password = "abcd";

            // Act
            var result = service.Authenticate(username,password);

            // Assert
            Assert.True(result==null);
            this.mockRepository.Verify();
        }
    }
}
