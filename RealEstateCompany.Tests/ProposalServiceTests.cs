using Microsoft.VisualStudio.TestTools.UnitTesting;
using RealEstateCompany.DAL.Entities;
using RealEstateCompany.DAL.Repositories;
using RealEstateCompany.BLL.Services;
using RealEstateCompany.BLL.Exceptions;

namespace RealEstateCompany.Tests
{
    [TestClass]
    public class ProposalServiceTests
    {
        private IProposalService _proposalService = null!;
        private JsonRepository<Client> _clientRepository = null!;
        private JsonRepository<RealEstate> _realEstateRepository = null!;

        [TestInitialize]
        public void Setup()
        {
            if (File.Exists("test_clients_proposal.json"))
                File.Delete("test_clients_proposal.json");
            if (File.Exists("test_realestates_proposal.json"))
                File.Delete("test_realestates_proposal.json");

            _clientRepository = new JsonRepository<Client>("test_clients_proposal.json");
            _realEstateRepository = new JsonRepository<RealEstate>("test_realestates_proposal.json");
            _proposalService = new ProposalService(_clientRepository, _realEstateRepository);
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists("test_clients_proposal.json"))
                File.Delete("test_clients_proposal.json");
            if (File.Exists("test_realestates_proposal.json"))
                File.Delete("test_realestates_proposal.json");
        }

        [TestMethod]
        public void AddToProposal_ValidIds_PropertyAddedToProposal()
        {
            // Arrange
            var client = new Client { FirstName = "John", LastName = "Doe", BankAccount = "123" };
            var realEstate = new RealEstate { Address = "Test Address", Price = 100000, Area = 75 };

            _clientRepository.Add(client);
            _realEstateRepository.Add(realEstate);

            // Act
            _proposalService.AddToProposal(1, 1);

            // Assert
            var updatedClient = _clientRepository.GetById(1);
            Assert.AreEqual(1, updatedClient!.ProposedRealEstateIds.Count);
            Assert.AreEqual(1, updatedClient.ProposedRealEstateIds[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(ClientNotFoundException))]
        public void AddToProposal_InvalidClientId_ThrowsClientNotFoundException()
        {
            // Act & Assert
            _proposalService.AddToProposal(999, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(RealEstateNotFoundException))]
        public void AddToProposal_InvalidRealEstateId_ThrowsRealEstateNotFoundException()
        {
            // Arrange
            var client = new Client { FirstName = "John", LastName = "Doe", BankAccount = "123" };
            _clientRepository.Add(client);

            // Act & Assert
            _proposalService.AddToProposal(1, 999);
        }

        [TestMethod]
        [ExpectedException(typeof(MaximumProposalsExceededException))]
        public void AddToProposal_MaxPropertiesReached_ThrowsMaximumProposalsExceededException()
        {
            // Arrange
            var client = new Client
            {
                FirstName = "John",
                LastName = "Doe",
                BankAccount = "123",
                ProposedRealEstateIds = new List<int> { 1, 2, 3, 4, 5 } // Already has 5 properties
            };

            var realEstate = new RealEstate { Address = "Test", Price = 100000, Area = 75 };

            _clientRepository.Add(client);
            _realEstateRepository.Add(realEstate);

            // Act & Assert
            _proposalService.AddToProposal(1, 6);
        }

        [TestMethod]
        public void AddToProposal_DuplicateProperty_NoDuplicateAdded()
        {
            // Arrange
            var client = new Client { FirstName = "John", LastName = "Doe", BankAccount = "123" };
            var realEstate = new RealEstate { Address = "Test", Price = 100000, Area = 75 };

            _clientRepository.Add(client);
            _realEstateRepository.Add(realEstate);

            // Act - Add same property twice
            _proposalService.AddToProposal(1, 1);
            _proposalService.AddToProposal(1, 1);

            // Assert
            var updatedClient = _clientRepository.GetById(1);
            Assert.AreEqual(1, updatedClient!.ProposedRealEstateIds.Count); // Still only one instance
        }

        [TestMethod]
        public void RemoveFromProposal_ValidIds_PropertyRemovedFromProposal()
        {
            // Arrange
            var client = new Client
            {
                FirstName = "John",
                LastName = "Doe",
                BankAccount = "123",
                ProposedRealEstateIds = new List<int> { 1, 2 }
            };
            var realEstate = new RealEstate { Address = "Test", Price = 100000, Area = 75 };

            _clientRepository.Add(client);
            _realEstateRepository.Add(realEstate);

            // Act
            _proposalService.RemoveFromProposal(1, 1);

            // Assert
            var updatedClient = _clientRepository.GetById(1);
            Assert.AreEqual(1, updatedClient!.ProposedRealEstateIds.Count);
            Assert.IsFalse(updatedClient.ProposedRealEstateIds.Contains(1));
        }

        [TestMethod]
        [ExpectedException(typeof(ClientNotFoundException))]
        public void RemoveFromProposal_InvalidClientId_ThrowsClientNotFoundException()
        {
            // Act & Assert
            _proposalService.RemoveFromProposal(999, 1);
        }

        [TestMethod]
        public void RemoveFromProposal_NonExistingProperty_NoExceptionThrown()
        {
            // Arrange
            var client = new Client
            {
                FirstName = "John",
                LastName = "Doe",
                BankAccount = "123",
                ProposedRealEstateIds = new List<int> { 1, 2 }
            };
            _clientRepository.Add(client);

            // Act & Assert (should not throw)
            _proposalService.RemoveFromProposal(1, 999);
        }

        [TestMethod]
        public void GetClientProposals_ClientWithProposals_ReturnsProposals()
        {
            // Arrange
            var client = new Client
            {
                FirstName = "John",
                LastName = "Doe",
                BankAccount = "123",
                ProposedRealEstateIds = new List<int> { 1, 2 }
            };
            var realEstate1 = new RealEstate { Address = "Address 1", Price = 100000, Area = 50 };
            var realEstate2 = new RealEstate { Address = "Address 2", Price = 200000, Area = 75 };

            _clientRepository.Add(client);
            _realEstateRepository.Add(realEstate1);
            _realEstateRepository.Add(realEstate2);

            // Act
            var proposals = _proposalService.GetClientProposals(1).ToList();

            // Assert
            Assert.AreEqual(2, proposals.Count);
            Assert.IsTrue(proposals.Any(p => p.Address == "Address 1"));
            Assert.IsTrue(proposals.Any(p => p.Address == "Address 2"));
        }

        [TestMethod]
        public void GetClientProposals_ClientWithoutProposals_ReturnsEmptyList()
        {
            // Arrange
            var client = new Client { FirstName = "John", LastName = "Doe", BankAccount = "123" };
            _clientRepository.Add(client);

            // Act
            var proposals = _proposalService.GetClientProposals(1).ToList();

            // Assert
            Assert.AreEqual(0, proposals.Count);
        }

        [TestMethod]
        public void GetClientProposals_InvalidClientId_ReturnsEmptyList()
        {
            // Act
            var proposals = _proposalService.GetClientProposals(999).ToList();

            // Assert
            Assert.AreEqual(0, proposals.Count);
        }

        [TestMethod]
        public void GetClientProposals_NonExistingRealEstateInProposal_ReturnsAvailableProposals()
        {
            // Arrange
            var client = new Client
            {
                FirstName = "John",
                LastName = "Doe",
                BankAccount = "123",
                ProposedRealEstateIds = new List<int> { 1, 999 } // 999 doesn't exist
            };
            var realEstate = new RealEstate { Address = "Address 1", Price = 100000, Area = 50 };

            _clientRepository.Add(client);
            _realEstateRepository.Add(realEstate);

            // Act
            var proposals = _proposalService.GetClientProposals(1).ToList();

            // Assert
            Assert.AreEqual(1, proposals.Count); // Only the existing real estate
            Assert.AreEqual("Address 1", proposals[0].Address);
        }

        [TestMethod]
        public void CheckAvailability_PropertyAvailable_ReturnsTrue()
        {
            // Arrange
            var realEstate = new RealEstate
            {
                Address = "Test Address",
                Type = RealEstateType.TwoRoomApartment,
                Price = 150000,
                Area = 75,
                IsAvailable = true
            };
            _realEstateRepository.Add(realEstate);

            // Act
            var isAvailable = _proposalService.CheckAvailability(RealEstateType.TwoRoomApartment, 200000);

            // Assert
            Assert.IsTrue(isAvailable);
        }

        [TestMethod]
        public void CheckAvailability_PropertyNotAvailable_ReturnsFalse()
        {
            // Arrange
            var realEstate = new RealEstate
            {
                Address = "Test Address",
                Type = RealEstateType.TwoRoomApartment,
                Price = 250000, // Higher than search price
                Area = 75,
                IsAvailable = true
            };
            _realEstateRepository.Add(realEstate);

            // Act
            var isAvailable = _proposalService.CheckAvailability(RealEstateType.TwoRoomApartment, 200000);

            // Assert
            Assert.IsFalse(isAvailable);
        }

        [TestMethod]
        public void CheckAvailability_PropertyNotAvailableStatus_ReturnsFalse()
        {
            // Arrange
            var realEstate = new RealEstate
            {
                Address = "Test Address",
                Type = RealEstateType.TwoRoomApartment,
                Price = 150000,
                Area = 75,
                IsAvailable = false // Not available
            };
            _realEstateRepository.Add(realEstate);

            // Act
            var isAvailable = _proposalService.CheckAvailability(RealEstateType.TwoRoomApartment, 200000);

            // Assert
            Assert.IsFalse(isAvailable);
        }

        [TestMethod]
        public void CheckAvailability_NoMatchingProperties_ReturnsFalse()
        {
            // Act
            var isAvailable = _proposalService.CheckAvailability(RealEstateType.ThreeRoomApartment, 100000);

            // Assert
            Assert.IsFalse(isAvailable);
        }

        [TestMethod]
        public void CheckAvailability_MultipleProperties_ReturnsTrueIfAtLeastOneMatches()
        {
            // Arrange
            var realEstate1 = new RealEstate
            {
                Address = "Address 1",
                Type = RealEstateType.TwoRoomApartment,
                Price = 250000, // Too expensive
                Area = 75,
                IsAvailable = true
            };
            var realEstate2 = new RealEstate
            {
                Address = "Address 2",
                Type = RealEstateType.TwoRoomApartment,
                Price = 150000, // Matches criteria
                Area = 60,
                IsAvailable = true
            };
            _realEstateRepository.Add(realEstate1);
            _realEstateRepository.Add(realEstate2);

            // Act
            var isAvailable = _proposalService.CheckAvailability(RealEstateType.TwoRoomApartment, 200000);

            // Assert
            Assert.IsTrue(isAvailable);
        }
    }
}