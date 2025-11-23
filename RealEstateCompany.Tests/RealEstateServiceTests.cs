using Microsoft.VisualStudio.TestTools.UnitTesting;
using RealEstateCompany.DAL.Entities;
using RealEstateCompany.DAL.Repositories;
using RealEstateCompany.BLL.Services;

namespace RealEstateCompany.Tests
{
    [TestClass]
    public class RealEstateServiceTests
    {
        private IRealEstateService _realEstateService = null!;
        private JsonRepository<RealEstate> _repository = null!;

        [TestInitialize]
        public void Setup()
        {
            if (File.Exists("test_realestates.json"))
                File.Delete("test_realestates.json");

            _repository = new JsonRepository<RealEstate>("test_realestates.json");
            _realEstateService = new RealEstateService(_repository);
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists("test_realestates.json"))
                File.Delete("test_realestates.json");
        }

        [TestMethod]
        public void AddRealEstate_ValidRealEstate_AddedSuccessfully()
        {
            // Arrange
            var realEstate = new RealEstate
            {
                Address = "Test Address 123",
                Type = RealEstateType.TwoRoomApartment,
                Price = 100000,
                Area = 75.5,
                Rooms = 2
            };

            // Act
            _realEstateService.AddRealEstate(realEstate);

            // Assert
            var realEstates = _realEstateService.GetAllRealEstates().ToList();
            Assert.AreEqual(1, realEstates.Count);
            Assert.AreEqual("Test Address 123", realEstates[0].Address);
            Assert.AreEqual(RealEstateType.TwoRoomApartment, realEstates[0].Type);
            Assert.AreEqual(100000, realEstates[0].Price);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddRealEstate_EmptyAddress_ThrowsArgumentException()
        {
            // Arrange
            var realEstate = new RealEstate
            {
                Address = "",
                Type = RealEstateType.TwoRoomApartment,
                Price = 100000
            };

            // Act & Assert
            _realEstateService.AddRealEstate(realEstate);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddRealEstate_InvalidPrice_ThrowsArgumentException()
        {
            // Arrange
            var realEstate = new RealEstate
            {
                Address = "Test Address",
                Type = RealEstateType.TwoRoomApartment,
                Price = -100
            };

            // Act & Assert
            _realEstateService.AddRealEstate(realEstate);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddRealEstate_InvalidArea_ThrowsArgumentException()
        {
            // Arrange
            var realEstate = new RealEstate
            {
                Address = "Test Address",
                Type = RealEstateType.TwoRoomApartment,
                Price = 100000,
                Area = -50
            };

            // Act & Assert
            _realEstateService.AddRealEstate(realEstate);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddRealEstate_NullRealEstate_ThrowsArgumentNullException()
        {
            // Arrange
            RealEstate realEstate = null!;

            // Act & Assert
            _realEstateService.AddRealEstate(realEstate);
        }

        [TestMethod]
        public void GetRealEstateById_ExistingRealEstate_ReturnsRealEstate()
        {
            // Arrange
            var realEstate = new RealEstate
            {
                Address = "Test Address",
                Type = RealEstateType.TwoRoomApartment,
                Price = 100000,
                Area = 75.5
            };
            _realEstateService.AddRealEstate(realEstate);

            // Act
            var retrieved = _realEstateService.GetRealEstateById(1);

            // Assert
            Assert.IsNotNull(retrieved);
            Assert.AreEqual("Test Address", retrieved.Address);
            Assert.AreEqual(1, retrieved.Id);
        }

        [TestMethod]
        public void GetRealEstateById_NonExistingRealEstate_ReturnsNull()
        {
            // Act
            var retrieved = _realEstateService.GetRealEstateById(999);

            // Assert
            Assert.IsNull(retrieved);
        }

        [TestMethod]
        public void GetAllRealEstates_EmptyRepository_ReturnsEmptyList()
        {
            // Act
            var realEstates = _realEstateService.GetAllRealEstates();

            // Assert
            Assert.IsFalse(realEstates.Any());
        }

        [TestMethod]
        public void GetAllRealEstates_MultipleRealEstates_ReturnsAllRealEstates()
        {
            // Arrange
            var realEstate1 = new RealEstate { Address = "Address 1", Price = 100000, Area = 50 };
            var realEstate2 = new RealEstate { Address = "Address 2", Price = 200000, Area = 75 };
            _realEstateService.AddRealEstate(realEstate1);
            _realEstateService.AddRealEstate(realEstate2);

            // Act
            var realEstates = _realEstateService.GetAllRealEstates().ToList();

            // Assert
            Assert.AreEqual(2, realEstates.Count);
        }

        [TestMethod]
        public void UpdateRealEstate_ValidData_UpdatesSuccessfully()
        {
            // Arrange
            var realEstate = new RealEstate { Address = "Old Address", Price = 100000, Area = 50 };
            _realEstateService.AddRealEstate(realEstate);

            var updated = _realEstateService.GetRealEstateById(1);
            updated!.Address = "New Address";
            updated.Price = 150000;

            // Act
            _realEstateService.UpdateRealEstate(updated);

            // Assert
            var result = _realEstateService.GetRealEstateById(1);
            Assert.AreEqual("New Address", result!.Address);
            Assert.AreEqual(150000, result.Price);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UpdateRealEstate_InvalidData_ThrowsException()
        {
            // Arrange
            var realEstate = new RealEstate { Address = "Valid", Price = 100000, Area = 50 };
            _realEstateService.AddRealEstate(realEstate);

            var updated = _realEstateService.GetRealEstateById(1);
            updated!.Price = -100; // Invalid price

            // Act & Assert
            _realEstateService.UpdateRealEstate(updated);
        }

        [TestMethod]
        public void DeleteRealEstate_ExistingRealEstate_RealEstateDeleted()
        {
            // Arrange
            var realEstate = new RealEstate { Address = "Test Address", Price = 100000, Area = 50 };
            _realEstateService.AddRealEstate(realEstate);

            // Act
            _realEstateService.DeleteRealEstate(1);

            // Assert
            var realEstates = _realEstateService.GetAllRealEstates();
            Assert.AreEqual(0, realEstates.Count());
        }

        [TestMethod]
        public void DeleteRealEstate_NonExistingRealEstate_NoExceptionThrown()
        {
            // Act & Assert (should not throw)
            _realEstateService.DeleteRealEstate(999);
        }

        [TestMethod]
        public void SortByType_RealEstatesSortedCorrectly()
        {
            // Arrange
            var realEstate1 = new RealEstate { Address = "A", Type = RealEstateType.TwoRoomApartment, Price = 100000, Area = 50 };
            var realEstate2 = new RealEstate { Address = "B", Type = RealEstateType.OneRoomApartment, Price = 80000, Area = 35 };

            _realEstateService.AddRealEstate(realEstate1);
            _realEstateService.AddRealEstate(realEstate2);

            // Act
            var sorted = _realEstateService.SortByType().ToList();

            // Assert
            Assert.AreEqual(RealEstateType.OneRoomApartment, sorted[0].Type);
            Assert.AreEqual(RealEstateType.TwoRoomApartment, sorted[1].Type);
        }

        [TestMethod]
        public void SortByPrice_RealEstatesSortedCorrectly()
        {
            // Arrange
            var realEstate1 = new RealEstate { Address = "A", Price = 200000, Area = 100 };
            var realEstate2 = new RealEstate { Address = "B", Price = 100000, Area = 50 };

            _realEstateService.AddRealEstate(realEstate1);
            _realEstateService.AddRealEstate(realEstate2);

            // Act
            var sorted = _realEstateService.SortByPrice().ToList();

            // Assert
            Assert.AreEqual(100000, sorted[0].Price);
            Assert.AreEqual(200000, sorted[1].Price);
        }

        [TestMethod]
        public void Search_KeywordFoundInAddress_ReturnsMatchingRealEstates()
        {
            // Arrange
            var realEstate1 = new RealEstate { Address = "Main Street 123", Price = 100000, Area = 50 };
            var realEstate2 = new RealEstate { Address = "Park Avenue 456", Price = 200000, Area = 75 };

            _realEstateService.AddRealEstate(realEstate1);
            _realEstateService.AddRealEstate(realEstate2);

            // Act
            var results = _realEstateService.Search("Main").ToList();

            // Assert
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Main Street 123", results[0].Address);
        }

        [TestMethod]
        public void Search_KeywordFoundInType_ReturnsMatchingRealEstates()
        {
            // Arrange
            var realEstate1 = new RealEstate { Address = "A", Type = RealEstateType.OneRoomApartment, Price = 100000, Area = 50 };
            var realEstate2 = new RealEstate { Address = "B", Type = RealEstateType.TwoRoomApartment, Price = 200000, Area = 75 };

            _realEstateService.AddRealEstate(realEstate1);
            _realEstateService.AddRealEstate(realEstate2);

            // Act
            var results = _realEstateService.Search("OneRoom").ToList();

            // Assert
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(RealEstateType.OneRoomApartment, results[0].Type);
        }

        [TestMethod]
        public void Search_KeywordFoundInPrice_ReturnsMatchingRealEstates()
        {
            // Arrange
            var realEstate1 = new RealEstate { Address = "A", Price = 150000, Area = 50 };
            var realEstate2 = new RealEstate { Address = "B", Price = 250000, Area = 75 };

            _realEstateService.AddRealEstate(realEstate1);
            _realEstateService.AddRealEstate(realEstate2);

            // Act
            var results = _realEstateService.Search("150").ToList();

            // Assert
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(150000, results[0].Price);
        }

        [TestMethod]
        public void Search_EmptyKeyword_ReturnsAllRealEstates()
        {
            // Arrange
            var realEstate1 = new RealEstate { Address = "A", Price = 100000, Area = 50 };
            var realEstate2 = new RealEstate { Address = "B", Price = 200000, Area = 75 };
            _realEstateService.AddRealEstate(realEstate1);
            _realEstateService.AddRealEstate(realEstate2);

            // Act
            var results = _realEstateService.Search("").ToList();

            // Assert
            Assert.AreEqual(2, results.Count);
        }

        [TestMethod]
        public void FindByCriteria_ByTypeOnly_ReturnsMatchingRealEstates()
        {
            // Arrange
            var realEstate1 = new RealEstate { Address = "A", Type = RealEstateType.OneRoomApartment, Price = 100000, Area = 50 };
            var realEstate2 = new RealEstate { Address = "B", Type = RealEstateType.TwoRoomApartment, Price = 200000, Area = 75 };
            _realEstateService.AddRealEstate(realEstate1);
            _realEstateService.AddRealEstate(realEstate2);

            // Act
            var results = _realEstateService.FindByCriteria(RealEstateType.OneRoomApartment, null).ToList();

            // Assert
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(RealEstateType.OneRoomApartment, results[0].Type);
        }

        [TestMethod]
        public void FindByCriteria_ByMaxPriceOnly_ReturnsMatchingRealEstates()
        {
            // Arrange
            var realEstate1 = new RealEstate { Address = "A", Type = RealEstateType.OneRoomApartment, Price = 100000, Area = 50 };
            var realEstate2 = new RealEstate { Address = "B", Type = RealEstateType.TwoRoomApartment, Price = 200000, Area = 75 };
            _realEstateService.AddRealEstate(realEstate1);
            _realEstateService.AddRealEstate(realEstate2);

            // Act
            var results = _realEstateService.FindByCriteria(null, 150000).ToList();

            // Assert
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(100000, results[0].Price);
        }

        [TestMethod]
        public void FindByCriteria_ByTypeAndMaxPrice_ReturnsMatchingRealEstates()
        {
            // Arrange
            var realEstate1 = new RealEstate { Address = "A", Type = RealEstateType.OneRoomApartment, Price = 100000, Area = 50 };
            var realEstate2 = new RealEstate { Address = "B", Type = RealEstateType.OneRoomApartment, Price = 200000, Area = 75 };
            _realEstateService.AddRealEstate(realEstate1);
            _realEstateService.AddRealEstate(realEstate2);

            // Act
            var results = _realEstateService.FindByCriteria(RealEstateType.OneRoomApartment, 150000).ToList();

            // Assert
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(RealEstateType.OneRoomApartment, results[0].Type);
            Assert.AreEqual(100000, results[0].Price);
        }

        [TestMethod]
        public void FindByCriteria_NoCriteria_ReturnsAllAvailableRealEstates()
        {
            // Arrange
            var realEstate1 = new RealEstate { Address = "A", Price = 100000, Area = 50, IsAvailable = true };
            var realEstate2 = new RealEstate { Address = "B", Price = 200000, Area = 75, IsAvailable = false };
            _realEstateService.AddRealEstate(realEstate1);
            _realEstateService.AddRealEstate(realEstate2);

            // Act
            var results = _realEstateService.FindByCriteria(null, null).ToList();

            // Assert
            Assert.AreEqual(1, results.Count); // Only available real estates
            Assert.AreEqual("A", results[0].Address);
        }
    }
}