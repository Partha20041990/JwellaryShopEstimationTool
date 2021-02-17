using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using PriceEstimation.JwellaryShop.WebApi.Entities;
using PriceEstimation.JwellaryShop.WebApi.Repositories.Interfaces;
using PriceEstimation.JwellaryShop.WebApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using Xunit;

namespace PriceEstimation.JwellaryShop.Test.Services
{
    public class EstimationServiceTests
    {
        private MockRepository mockRepository;

        private Mock<IUserRepository> mockUserRepository;
        private Mock<IHttpContextAccessor> mockHttpContextAccessor;
        private Mock<ILogger<EstimationService>> mockLogger;

        public EstimationServiceTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Loose);

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

            var fakeHttpContext = new Mock<HttpContext>();
            var principal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "1"),
                new Claim(ClaimTypes.Role, "Privileged")
            }));
            fakeHttpContext.Setup(t => t.User).Returns(principal);

            this.mockUserRepository = this.mockRepository.Create<IUserRepository>();
            this.mockUserRepository.Setup(p => p.GetAll()).Returns(userList.AsQueryable().OrderBy(s => s.Id));
            this.mockHttpContextAccessor = this.mockRepository.Create<IHttpContextAccessor>();
            mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(fakeHttpContext.Object);
            this.mockLogger = this.mockRepository.Create<ILogger<EstimationService>>();
            mockLogger.Setup(
            p => p.Log(It.IsAny<LogLevel>(),
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => true),
            It.IsAny<Exception>(),
            It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true))).Verifiable();
        }

        private EstimationService CreateService()
        {
            return new EstimationService(
                this.mockUserRepository.Object,
                this.mockHttpContextAccessor.Object,
                this.mockLogger.Object);
        }

        [Fact]
        public void CalculatePrice_PriceCalculation_ShouldReturnActualPrice()
        {
            // Arrange
            var service = this.CreateService();
            EstimationModel estimationModel = new EstimationModel
            {
                DiscountPercentage = 5,
                PricePerGram = 5500,
                Weight = 2
            };

            // Act
            var svcResult = service.CalculatePrice(
                estimationModel);

            // Assert
            Assert.True(Convert.ToDecimal(svcResult.Result).Equals((decimal)10450.00));
            this.mockRepository.Verify();
        }
    }
}
