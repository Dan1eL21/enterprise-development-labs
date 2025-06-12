using CarRentalService.Domain.Data;
using CarRentalService.Domain.Model;
using CarRentalService.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CarRentalService.Domain.Services.InMemory;

public class VehicleInMemoryRepository : IVehicleRepository
{
    private readonly List<Car> _cars;
    private readonly List<RentalRecord> _rentals;
    private readonly List<RentalPoint> _rentalPoints;
    private readonly List<Client> _clients;

    public VehicleInMemoryRepository()
    {
        _cars = DataSeeder.Cars;
        _rentals = DataSeeder.Rentals;
        _rentalPoints = DataSeeder.RentalPoints;
        _clients = DataSeeder.Clients;
    }

    #region IRepository<Car, int> Implementation
    public IList<Car> GetAll() => _cars;

    public Car? Get(int key) => _cars.FirstOrDefault(c => c.Id == key);

    public bool Add(Car entity)
    {
        if (entity == null || _cars.Any(c => c.Id == entity.Id))
            return false;

        _cars.Add(entity);
        return true;
    }

    public bool Update(Car entity)
    {
        var existingCar = Get(entity.Id);
        if (existingCar == null)
            return false;

        existingCar.RegistrationNumber = entity.RegistrationNumber;
        existingCar.Model = entity.Model;
        existingCar.Color = entity.Color;
        existingCar.IsCurrentlyRented = entity.IsCurrentlyRented;
        existingCar.RentalCount = entity.RentalCount;

        return true;
    }

    public bool Delete(int key)
    {
        var car = Get(key);
        if (car == null)
            return false;

        return _cars.Remove(car);
    }
    #endregion

    #region IVehicleRepository Implementation
    public IList<Car> GetCurrentlyRentedVehicles() =>
        _cars.Where(c => c.IsCurrentlyRented).ToList();

    public IList<Tuple<string, int>> GetRentalCountsByVehicle() =>
        _cars
            .Select(c => new Tuple<string, int>($"{c.RegistrationNumber} - {c.Model}", c.RentalCount))
            .OrderBy(t => t.Item1)
            .ToList();

    public IList<Tuple<string, int>> GetTop5MostRentedVehicles() =>
        _cars
            .OrderByDescending(c => c.RentalCount)
            .Take(5)
            .Select(c => new Tuple<string, int>($"{c.RegistrationNumber} - {c.Model}", c.RentalCount))
            .ToList();

    public IList<string> GetAllVehiclesInfo()
    {
        return _cars.Select(car =>
                $"ID: {car.Id}, Номер: {car.RegistrationNumber}, Модель: {car.Model}, " +
                $"Цвет: {car.Color}, В аренде: {(car.IsCurrentlyRented ? "Да" : "Нет")}, " +
                $"Количество аренд: {car.RentalCount}")
            .ToList();
    }

    public IList<string> GetCustomersByVehicleModel(string model)
    {
        return _rentals
            .Where(rental => rental.Car != null &&
                            rental.Car.Model == model &&
                            rental.Client != null)
            .Select(rental => rental.Client)
            .Distinct()
            .OrderBy(client => client.LastName)
            .ThenBy(client => client.FirstName)
            .Select(client =>
                $"Клиент: {client.LastName} {client.FirstName} {client.Patronymic}, " +
                $"Паспорт: {client.PassportNumber}, Дата рождения: {client.BirthDate:dd.MM.yyyy}")
            .ToList();
    }

    public IList<string> GetRentedVehicles()
    {
        return _cars
            .Where(car => car.IsCurrentlyRented)
            .Select(car =>
                $"ID: {car.Id}, Номер: {car.RegistrationNumber}, " +
                $"Модель: {car.Model}, Цвет: {car.Color}, " +
                $"Дата начала аренды: {car.RentalRecords?.LastOrDefault(r => r.ReturnTime == null)?.RentTime:dd.MM.yyyy}")
            .ToList();
    }

    public IList<Tuple<string, int>> GetTop5RentedVehicles()
    {
        return _cars
            .OrderByDescending(car => car.RentalCount)
            .Take(5)
            .Select(car => new Tuple<string, int>(
                $"Номер: {car.RegistrationNumber}, Модель: {car.Model}",
                car.RentalCount))
            .ToList();
    }

    public IList<string> GetRentalCountPerVehicle()
    {
        return _cars
            .OrderByDescending(car => car.RentalCount)
            .Select(car =>
                $"Номер: {car.RegistrationNumber}, Модель: {car.Model}, " +
                $"Количество аренд: {car.RentalCount}")
            .ToList();
    }

    public IList<string> GetRentalPointsWithMaxRentals()
    {
        var rentalCounts = _rentalPoints
            .ToDictionary(
                rp => rp,
                rp => _rentals.Count(r => r.RentPoint?.Id == rp.Id));

        if (rentalCounts.Count == 0)
            return new List<string>();

        var maxCount = rentalCounts.Values.Max();

        return rentalCounts
            .Where(kvp => kvp.Value == maxCount)
            .OrderBy(kvp => kvp.Key.Name)
            .Select(kvp =>
                $"Пункт проката: {kvp.Key.Name}, Адрес: {kvp.Key.Address}, " +
                $"Количество аренд: {kvp.Value}")
            .ToList();
    }

    public (int Min, double Average, int Max) GetRentalStatisticsByRentalPoint()
    {
        var rentalCounts = _rentalPoints
            .Select(rp => _rentals.Count(r => r.RentPoint?.Id == rp.Id))
            .ToList();

        if (rentalCounts.Count == 0)
            return (0, 0, 0);

        return (rentalCounts.Min(), rentalCounts.Average(), rentalCounts.Max());
    }
    #endregion
}