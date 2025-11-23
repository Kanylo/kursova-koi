using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateApp.BLL.Exceptions;
using RealEstateApp.BLL.Services;
using RealEstateApp.DAL.Interfaces;
using RealEstateApp.DAL.Models;

namespace RealEstateApp.Tests.Services
{
    [TestClass]
    public class ClientServiceTests
    {
        private Mock<IClientRepository> _mockRepository = null!;
        private ClientService _clientService = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepository = new Mock<IClientRepository>();
            _clientService = new ClientService(_mockRepository.Object);
        }

        [TestMethod]
        public void AddClient_ValidClient_ShouldCallRepositoryAdd()
        {
            // Arrange
            var client = new Client
            {
                FirstName = "John",
                LastName = "Doe",
                BankAccount = "1234567890",
                ContactInfo = "test@test.com"
            };
            _mockRepository.Setup(r => r.Add(client));

            // Act
            _clientService.AddClient(client);

            // Assert
            _mockRepository.Verify(r => r.Add(client), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void AddClient_InvalidBankAccount_ShouldThrowValidationException()
        {
            // Arrange
            var client = new Client
            {
                FirstName = "John",
                LastName = "Doe",
                BankAccount = "invalid",
                ContactInfo = "test@test.com"
            };

            // Act
            _clientService.AddClient(client);
        }

        [TestMethod]
        public void GetClientById_ExistingClient_ShouldReturnClient()
        {
            // Arrange
            var expectedClient = new Client { Id = 1, FirstName = "John" };
            _mockRepository.Setup(r => r.GetById(1)).Returns(expectedClient);

            // Act
            var result = _clientService.GetClientById(1);

            // Assert
            Assert.AreEqual(expectedClient, result);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public void GetClientById_NonExistingClient_ShouldThrowNotFoundException()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetById(1)).Returns((Client?)null);

            // Act
            _clientService.GetClientById(1);
        }
    }
}