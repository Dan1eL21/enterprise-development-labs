using CarRentalService.Domain.Model;
using CarRentalService.Domain.Services.InMemory;
using Xunit;
using System.Linq;
using System;

namespace CarRentalService.Domain.Tests;

    public class VehicleInMemoryRepositoryTests
    {
        private readonly VehicleInMemoryRepository _repository;

        public VehicleInMemoryRepositoryTests()
        {
            _repository = new VehicleInMemoryRepository();
        }

        #region Basic CRUD Tests
        [Fact]
        public void GetAll_ShouldReturnAllCars()
        {
            // Act
            var result = _repository.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

    [Fact]
    public void Get_WithValidId_ShouldReturnCar()
    {
        // Arrange - берем реальный ID из существующих данных
        var existingCar = _repository.GetAll().First();
        var expectedId = existingCar.Id;

        // Act
        var result = _repository.Get(expectedId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedId, result.Id);
    }

    [Fact]
        public void Get_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            var invalidId = -1;

            // Act
            var result = _repository.Get(invalidId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Add_NewCar_ShouldReturnTrueAndAddCar()
        {
            // Arrange
            var newCar = new Car
            {
                Id = 999,
                RegistrationNumber = "TEST001",
                Model = "Test Model",
                Color = "Red"
            };

            // Act
            var result = _repository.Add(newCar);
            var addedCar = _repository.Get(newCar.Id);

            // Assert
            Assert.True(result);
            Assert.NotNull(addedCar);
            Assert.Equal(newCar.RegistrationNumber, addedCar.RegistrationNumber);
        }

        [Fact]
        public void Add_DuplicateCar_ShouldReturnFalse()
        {
            // Arrange
            var existingCar = _repository.GetAll().First();

            // Act
            var result = _repository.Add(existingCar);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Update_ExistingCar_ShouldReturnTrueAndUpdateProperties()
        {
            // Arrange
            var existingCar = _repository.GetAll().First();
            var updatedCar = new Car
            {
                Id = existingCar.Id,
                RegistrationNumber = "UPDATED001",
                Model = "Updated Model",
                Color = "Updated Color",
                IsCurrentlyRented = !existingCar.IsCurrentlyRented,
                RentalCount = existingCar.RentalCount + 1
            };

            // Act
            var result = _repository.Update(updatedCar);
            var carAfterUpdate = _repository.Get(existingCar.Id);

            // Assert
            Assert.True(result);
            Assert.Equal(updatedCar.RegistrationNumber, carAfterUpdate.RegistrationNumber);
            Assert.Equal(updatedCar.Model, carAfterUpdate.Model);
            Assert.Equal(updatedCar.Color, carAfterUpdate.Color);
            Assert.Equal(updatedCar.IsCurrentlyRented, carAfterUpdate.IsCurrentlyRented);
            Assert.Equal(updatedCar.RentalCount, carAfterUpdate.RentalCount);
        }

        [Fact]
        public void Update_NonExistingCar_ShouldReturnFalse()
        {
            // Arrange
            var nonExistingCar = new Car { Id = -1 };

            // Act
            var result = _repository.Update(nonExistingCar);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Delete_ExistingCar_ShouldReturnTrueAndRemoveCar()
        {
            // Arrange
            var existingCar = _repository.GetAll().First();

            // Act
            var result = _repository.Delete(existingCar.Id);
            var carAfterDelete = _repository.Get(existingCar.Id);

            // Assert
            Assert.True(result);
            Assert.Null(carAfterDelete);
        }

        [Fact]
        public void Delete_NonExistingCar_ShouldReturnFalse()
        {
            // Arrange
            var nonExistingId = -1;

            // Act
            var result = _repository.Delete(nonExistingId);

            // Assert
            Assert.False(result);
        }
        #endregion

        #region Vehicle-Specific Tests
        [Fact]
        public void GetCurrentlyRentedVehicles_ShouldReturnOnlyRentedCars()
        {
            // Act
            var result = _repository.GetCurrentlyRentedVehicles();

            // Assert
            Assert.All(result, car => Assert.True(car.IsCurrentlyRented));
        }

        [Fact]
        public void GetRentalCountsByVehicle_ShouldReturnAllVehiclesWithCounts()
        {
            // Act
            var result = _repository.GetRentalCountsByVehicle();

            // Assert
            Assert.Equal(_repository.GetAll().Count, result.Count);
            Assert.All(result, item => Assert.True(item.Item2 >= 0));
        }

        [Fact]
        public void GetTop5MostRentedVehicles_ShouldReturn5OrLessItems()
        {
            // Act
            var result = _repository.GetTop5MostRentedVehicles();

            // Assert
            Assert.True(result.Count <= 5);
            if (result.Count > 1)
            {
                for (var i = 0; i < result.Count - 1; i++)
                {
                    Assert.True(result[i].Item2 >= result[i + 1].Item2);
                }
            }
        }

        [Fact]
        public void GetAllVehiclesInfo_ShouldReturnFormattedStrings()
        {
            // Act
            var result = _repository.GetAllVehiclesInfo();

            // Assert
            Assert.Equal(_repository.GetAll().Count, result.Count);
            Assert.All(result, s => Assert.Contains("ID:", s));
            Assert.All(result, s => Assert.Contains("Номер:", s));
            Assert.All(result, s => Assert.Contains("Модель:", s));
        }

        [Fact]
        public void GetCustomersByVehicleModel_WithExistingModel_ShouldReturnCustomers()
        {
            // Arrange
            var existingModel = _repository.GetAll().First().Model;

            // Act
            var result = _repository.GetCustomersByVehicleModel(existingModel);

            // Assert
            Assert.NotEmpty(result);
            Assert.All(result, s => Assert.Contains("Клиент:", s));
        }

        [Fact]
        public void GetCustomersByVehicleModel_WithNonExistingModel_ShouldReturnEmpty()
        {
            // Arrange
            var nonExistingModel = "NonExistingModel123";

            // Act
            var result = _repository.GetCustomersByVehicleModel(nonExistingModel);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetRentedVehicles_ShouldReturnFormattedStrings()
        {
            // Act
            var result = _repository.GetRentedVehicles();

            // Assert
            Assert.All(result, s => Assert.Contains("ID:", s));
            Assert.All(result, s => Assert.Contains("Номер:", s));
            Assert.All(result, s => Assert.Contains("Модель:", s));
        }

        [Fact]
        public void GetTop5RentedVehicles_ShouldReturn5OrLessItems()
        {
            // Act
            var result = _repository.GetTop5RentedVehicles();

            // Assert
            Assert.True(result.Count <= 5);
            if (result.Count > 1)
            {
                for (var i = 0; i < result.Count - 1; i++)
                {
                    Assert.True(result[i].Item2 >= result[i + 1].Item2);
                }
            }
        }

        [Fact]
        public void GetRentalCountPerVehicle_ShouldReturnAllVehicles()
        {
            // Act
            var result = _repository.GetRentalCountPerVehicle();

            // Assert
            Assert.Equal(_repository.GetAll().Count, result.Count);
            Assert.All(result, s => Assert.Contains("Количество аренд:", s));
        }

        [Fact]
        public void GetRentalPointsWithMaxRentals_ShouldReturnAtLeastOnePoint()
        {
            // Act
            var result = _repository.GetRentalPointsWithMaxRentals();

            // Assert
            Assert.NotEmpty(result);
            Assert.All(result, s => Assert.Contains("Пункт проката:", s));
        }

        [Fact]
        public void GetRentalStatisticsByRentalPoint_ShouldReturnValidStatistics()
        {
            // Act
            var result = _repository.GetRentalStatisticsByRentalPoint();

            // Assert
            Assert.True(result.Min <= result.Average);
            Assert.True(result.Average <= result.Max);
        }
        #endregion
    }