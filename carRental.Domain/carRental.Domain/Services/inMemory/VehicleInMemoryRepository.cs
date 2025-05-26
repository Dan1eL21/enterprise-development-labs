using CarRentalService.Domain.Data;
using CarRentalService.Domain.Model;
using CarRentalService.Domain.Services;

namespace CarRentalService.Domain.Services.InMemory;

/// <summary>
/// Имплементация репозитория для транспортных средств, которая хранит коллекцию в оперативной памяти
/// </summary>
public class VehicleInMemoryRepository : IVehicleRepository
{
    private readonly List<Car> _cars;

    public VehicleInMemoryRepository()
    {
        _cars = DataSeeder.Cars;
    }

    public bool Add(Car entity)
    {
        try
        {
            _cars.Add(entity);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool Delete(int key)
    {
        var car = Get(key);
        if (car == null) return false;
        return _cars.Remove(car);
    }

    public Car? Get(int key) =>
        _cars.FirstOrDefault(c => c.Id == key);

    public IList<Car> GetAll() => _cars;

    public IList<Car> GetCurrentlyRentedVehicles() =>
        _cars.Where(c => c.IsCurrentlyRented).ToList();

    public IList<Tuple<string, int>> GetRentalCountsByVehicle() =>
        _cars
            .Select(c => new Tuple<string, int>(c.ToString(), c.RentalCount))
            .OrderBy(t => t.Item1)
            .ToList();

    public IList<Tuple<string, int>> GetTop5MostRentedVehicles() =>
        _cars
            .OrderByDescending(c => c.RentalCount)
            .Take(5)
            .Select(c => new Tuple<string, int>(c.ToString(), c.RentalCount))
            .ToList();

    public bool Update(Car entity)
    {
        var car = Get(entity.Id);
        if (car == null)
        {
            return false; // Возвращаем false, если автомобиль не найден
        }

        // Обновляем свойства автомобиля
        car.RegistrationNumber = entity.RegistrationNumber;
        car.Model = entity.Model;
        car.Color = entity.Color;
        car.IsCurrentlyRented = entity.IsCurrentlyRented;
        car.RentalCount = entity.RentalCount;

        return true; // Возвращаем true, если обновление успешно
    }
}



