using Microsoft.VisualStudio.TestTools.UnitTesting;
using RealEstateCompany.DAL.Entities;
using RealEstateCompany.DAL.Repositories;
using RealEstateCompany.BLL.Services;
using RealEstateCompany.BLL.Exceptions;

namespace RealEstateCompany.Tests
{
    [TestClass]
    public class ClientServiceTests
    {
        private IClientService _clientService = null!;
        private JsonRepository<Client> _repository = null!;

        [TestInitialize]
        public void Setup()
        {
            // Clean up test file before each test
            if (File.Exists("test_clients.json"))
                File.Delete("test_clients.json");

            _repository = new JsonRepository<Client>("test_clients.json");
            _clientService = new ClientService(_repository);
        }

        [TestCleanup]
        public void Cleanup()
        {
            // Clean up after tests
            if (File.Exists("test_clients.json"))
                File.Delete("test_clients.json");
        }

        [TestMethod]
        public void AddClient_ValidClient_ClientAddedSuccessfully()
        {
            // Arrange
            var client = new Client
            {
                FirstName = "John",
                LastName = "Doe",
                BankAccount = "1234567890",
                DesiredType = RealEstateType.TwoRoomApartment,
                DesiredPrice = 100000
            };

            // Act
            _clientService.AddClient(client);

            // Assert
            var clients = _clientService.GetAllClients().ToList();
            Assert.AreEqual(1, clients.Count);
            Assert.AreEqual("John", clients[0].FirstName);
            Assert.AreEqual("Doe", clients[0].LastName);
            Assert.AreEqual("1234567890", clients[0].BankAccount);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddClient_EmptyFirstName_ThrowsArgumentException()
        {
            // Arrange
            var client = new Client
            {
                FirstName = "",
                LastName = "Doe",
                BankAccount = "1234567890"
            };

            // Act & Assert
            _clientService.AddClient(client);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddClient_EmptyLastName_ThrowsArgumentException()
        {
            // Arrange
            var client = new Client
            {
                FirstName = "John",
                LastName = "",
                BankAccount = "1234567890"
            };

            // Act & Assert
            _clientService.AddClient(client);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddClient_EmptyBankAccount_ThrowsArgumentException()
        {
            // Arrange
            var client = new Client
            {
                FirstName = "John",
                LastName = "Doe",
                BankAccount = ""
            };

            // Act & Assert
            _clientService.AddClient(client);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddClient_NullClient_ThrowsArgumentNullException()
        {
            // Arrange
            Client client = null!;

            // Act & Assert
            _clientService.AddClient(client);
        }

        [TestMethod]
        public void GetClientById_ExistingClient_ReturnsClient()
        {
            // Arrange
            var client = new Client
            {
                FirstName = "John",
                LastName = "Doe",
                BankAccount = "1234567890"
            };
            _clientService.AddClient(client);

            // Act
            var retrieved = _clientService.GetClientById(1);

            // Assert
            Assert.IsNotNull(retrieved);
            Assert.AreEqual("John", retrieved.FirstName);
            Assert.AreEqual(1, retrieved.Id);
        }

        [TestMethod]
        public void GetClientById_NonExistingClient_ReturnsNull()
        {
            // Act
            var retrieved = _clientService.GetClientById(999);

            // Assert
            Assert.IsNull(retrieved);
        }

        [TestMethod]
        public void GetAllClients_EmptyRepository_ReturnsEmptyList()
        {
            // Act
            var clients = _clientService.GetAllClients();

            // Assert
            Assert.IsFalse(clients.Any());
        }

        [TestMethod]
        public void GetAllClients_MultipleClients_ReturnsAllClients()
        {
            // Arrange
            var client1 = new Client { FirstName = "John", LastName = "Doe", BankAccount = "1" };
            var client2 = new Client { FirstName = "Jane", LastName = "Smith", BankAccount = "2" };
            _clientService.AddClient(client1);
            _clientService.AddClient(client2);

            // Act
            var clients = _clientService.GetAllClients().ToList();

            // Assert
            Assert.AreEqual(2, clients.Count);
        }

        [TestMethod]
        public void UpdateClient_ValidClient_ClientUpdatedSuccessfully()
        {
            // Arrange
            var client = new Client
            {
                FirstName = "John",
                LastName = "Doe",
                BankAccount = "1234567890"
            };
            _clientService.AddClient(client);

            var updatedClient = _clientService.GetClientById(1);
            updatedClient!.FirstName = "Jane";
            updatedClient.LastName = "Johnson";

            // Act
            _clientService.UpdateClient(updatedClient);

            // Assert
            var retrieved = _clientService.GetClientById(1);
            Assert.AreEqual("Jane", retrieved!.FirstName);
            Assert.AreEqual("Johnson", retrieved.LastName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UpdateClient_InvalidData_ThrowsException()
        {
            // Arrange
            var client = new Client
            {
                FirstName = "John",
                LastName = "Doe",
                BankAccount = "1234567890"
            };
            _clientService.AddClient(client);

            var updatedClient = _clientService.GetClientById(1);
            updatedClient!.FirstName = ""; // Invalid

            // Act & Assert
            _clientService.UpdateClient(updatedClient);
        }

        [TestMethod]
        public void DeleteClient_ExistingClient_ClientDeleted()
        {
            // Arrange
            var client = new Client
            {
                FirstName = "John",
                LastName = "Doe",
                BankAccount = "1234567890"
            };
            _clientService.AddClient(client);

            // Act
            _clientService.DeleteClient(1);

            // Assert
            var clients = _clientService.GetAllClients();
            Assert.AreEqual(0, clients.Count());
        }

        [TestMethod]
        public void DeleteClient_NonExistingClient_NoExceptionThrown()
        {
            // Act & Assert (should not throw)
            _clientService.DeleteClient(999);
        }

        [TestMethod]
        public void SortByName_ClientsSortedCorrectly()
        {
            // Arrange
            var client1 = new Client { FirstName = "Bob", LastName = "Smith", BankAccount = "1" };
            var client2 = new Client { FirstName = "Alice", LastName = "Johnson", BankAccount = "2" };

            _clientService.AddClient(client1);
            _clientService.AddClient(client2);

            // Act
            var sorted = _clientService.SortByName().ToList();

            // Assert
            Assert.AreEqual("Alice", sorted[0].FirstName);
            Assert.AreEqual("Bob", sorted[1].FirstName);
        }

        [TestMethod]
        public void SortByLastName_ClientsSortedCorrectly()
        {
            // Arrange
            var client1 = new Client { FirstName = "John", LastName = "Smith", BankAccount = "1" };
            var client2 = new Client { FirstName = "Jane", LastName = "Adams", BankAccount = "2" };

            _clientService.AddClient(client1);
            _clientService.AddClient(client2);

            // Act
            var sorted = _clientService.SortByLastName().ToList();

            // Assert
            Assert.AreEqual("Adams", sorted[0].LastName);
            Assert.AreEqual("Smith", sorted[1].LastName);
        }

        [TestMethod]
        public void SortByBankAccountFirstDigit_ClientsSortedCorrectly()
        {
            // Arrange
            var client1 = new Client { FirstName = "John", LastName = "Doe", BankAccount = "234" };
            var client2 = new Client { FirstName = "Jane", LastName = "Smith", BankAccount = "123" };

            _clientService.AddClient(client1);
            _clientService.AddClient(client2);

            // Act
            var sorted = _clientService.SortByBankAccountFirstDigit().ToList();

            // Assert
            Assert.AreEqual("123", sorted[0].BankAccount);
            Assert.AreEqual("234", sorted[1].BankAccount);
        }

        [TestMethod]
        public void Search_KeywordFoundInFirstName_ReturnsMatchingClients()
        {
            // Arrange
            var client1 = new Client { FirstName = "John", LastName = "Doe", BankAccount = "123" };
            var client2 = new Client { FirstName = "Jane", LastName = "Smith", BankAccount = "456" };

            _clientService.AddClient(client1);
            _clientService.AddClient(client2);

            // Act
            var results = _clientService.Search("John").ToList();

            // Assert
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("John", results[0].FirstName);
        }

        [TestMethod]
        public void Search_KeywordFoundInLastName_ReturnsMatchingClients()
        {
            // Arrange
            var client1 = new Client { FirstName = "John", LastName = "Doe", BankAccount = "123" };
            var client2 = new Client { FirstName = "Jane", LastName = "Smith", BankAccount = "456" };

            _clientService.AddClient(client1);
            _clientService.AddClient(client2);

            // Act
            var results = _clientService.Search("Smith").ToList();

            // Assert
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Smith", results[0].LastName);
        }

        [TestMethod]
        public void Search_KeywordFoundInBankAccount_ReturnsMatchingClients()
        {
            // Arrange
            var client1 = new Client { FirstName = "John", LastName = "Doe", BankAccount = "123456" };
            var client2 = new Client { FirstName = "Jane", LastName = "Smith", BankAccount = "789012" };

            _clientService.AddClient(client1);
            _clientService.AddClient(client2);

            // Act
            var results = _clientService.Search("123").ToList();

            // Assert
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("123456", results[0].BankAccount);
        }

        [TestMethod]
        public void Search_KeywordNotFound_ReturnsEmptyCollection()
        {
            // Arrange
            var client = new Client { FirstName = "John", LastName = "Doe", BankAccount = "123" };
            _clientService.AddClient(client);

            // Act
            var results = _clientService.Search("Nonexistent").ToList();

            // Assert
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void Search_EmptyKeyword_ReturnsAllClients()
        {
            // Arrange
            var client1 = new Client { FirstName = "John", LastName = "Doe", BankAccount = "123" };
            var client2 = new Client { FirstName = "Jane", LastName = "Smith", BankAccount = "456" };
            _clientService.AddClient(client1);
            _clientService.AddClient(client2);

            // Act
            var results = _clientService.Search("").ToList();

            // Assert
            Assert.AreEqual(2, results.Count);
        }

        [TestMethod]
        public void AdvancedSearch_ByLastNameOnly_ReturnsMatchingClients()
        {
            // Arrange
            var client1 = new Client { FirstName = "John", LastName = "Doe", BankAccount = "123", DesiredType = RealEstateType.OneRoomApartment };
            var client2 = new Client { FirstName = "Jane", LastName = "Smith", BankAccount = "456", DesiredType = RealEstateType.TwoRoomApartment };
            _clientService.AddClient(client1);
            _clientService.AddClient(client2);

            // Act
            var results = _clientService.AdvancedSearch("Doe", null).ToList();

            // Assert
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Doe", results[0].LastName);
        }

        [TestMethod]
        public void AdvancedSearch_ByDesiredTypeOnly_ReturnsMatchingClients()
        {
            // Arrange
            var client1 = new Client { FirstName = "John", LastName = "Doe", BankAccount = "123", DesiredType = RealEstateType.OneRoomApartment };
            var client2 = new Client { FirstName = "Jane", LastName = "Smith", BankAccount = "456", DesiredType = RealEstateType.TwoRoomApartment };
            _clientService.AddClient(client1);
            _clientService.AddClient(client2);

            // Act
            var results = _clientService.AdvancedSearch("", RealEstateType.OneRoomApartment).ToList();

            // Assert
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(RealEstateType.OneRoomApartment, results[0].DesiredType);
        }

        [TestMethod]
        public void AdvancedSearch_ByLastNameAndDesiredType_ReturnsMatchingClients()
        {
            // Arrange
            var client1 = new Client { FirstName = "John", LastName = "Doe", BankAccount = "123", DesiredType = RealEstateType.OneRoomApartment };
            var client2 = new Client { FirstName = "Jane", LastName = "Doe", BankAccount = "456", DesiredType = RealEstateType.TwoRoomApartment };
            _clientService.AddClient(client1);
            _clientService.AddClient(client2);

            // Act
            var results = _clientService.AdvancedSearch("Doe", RealEstateType.OneRoomApartment).ToList();

            // Assert
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Doe", results[0].LastName);
            Assert.AreEqual(RealEstateType.OneRoomApartment, results[0].DesiredType);
        }

        [TestMethod]
        public void AdvancedSearch_NoCriteria_ReturnsAllClients()
        {
            // Arrange
            var client1 = new Client { FirstName = "John", LastName = "Doe", BankAccount = "123", DesiredType = RealEstateType.OneRoomApartment };
            var client2 = new Client { FirstName = "Jane", LastName = "Smith", BankAccount = "456", DesiredType = RealEstateType.TwoRoomApartment };
            _clientService.AddClient(client1);
            _clientService.AddClient(client2);

            // Act
            var results = _clientService.AdvancedSearch("", null).ToList();

            // Assert
            Assert.AreEqual(2, results.Count);
        }
    }
}