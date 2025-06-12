using CarRentalService.Domain.Data;
using CarRentalService.Domain.Model;

namespace CarRentalService.Domain.Services;

/// <summary>
/// —ервис дл€ работы с арендой автомобилей
/// </summary>
public class RentalService : IRentalService
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IClientRepository _clientRepository;
    private readonly IRentalPointRepository _rentalPointRepository;

    public RentalService(
        IVehicleRepository vehicleRepository,
        IClientRepository clientRepository,
        IRentalPointRepository rentalPointRepository)
    {
        _vehicleRepository = vehicleRepository;
        _clientRepository = clientRepository;
        _rentalPointRepository = rentalPointRepository;
    }

    /// <summary>
    /// ѕолучить список всех автомобилей
    /// </summary>
    public IList<Car> GetAllCars() => 
        _vehicleRepository.GetAll();

    /// <summary>
    /// ѕолучить список клиентов, арендовавших определенную модель автомобил€
    /// </summary>
    public IList<Client> GetClientsByCarModel(string model) => 
        _clientRepository.GetClientsByVehicleModel(model);

    /// <summary>
    /// ѕолучить список автомобилей в аренде
    /// </summary>
    public IList<Car> GetCurrentlyRentedCars() => 
        _vehicleRepository.GetCurrentlyRentedVehicles();

    /// <summary>
    /// ѕолучить топ-5 самых арендуемых автомобилей
    /// </summary>
    public IList<Tuple<string, int>> GetTopRentedCars() => 
        _vehicleRepository.GetTop5MostRentedVehicles();

    /// <summary>
    /// ѕолучить статистику аренд по каждому автомобилю
    /// </summary>
    public IList<Tuple<string, int>> GetCarRentalCounts() => 
        _vehicleRepository.GetRentalCountsByVehicle();

    /// <summary>
    /// ѕолучить пункты проката с максимальным числом аренд
    /// </summary>
    public IList<RentalPoint> GetTopRentalPoints() => 
        _rentalPointRepository.GetTopRentalPointsByUsage();

    /// <summary>
    /// ќформить аренду автомобил€
    /// </summary>
    public bool RentCar(int clientId, int carId, int rentalPointId, int durationInDays)
    {
        var car = _vehicleRepository.Get(carId);
        if (car == null || car.IsCurrentlyRented) return false;

        var client = _clientRepository.Get(clientId);
        if (client == null) return false;

        var rentalPoint = _rentalPointRepository.Get(rentalPointId);
        if (rentalPoint == null) return false;

        var rental = new RentalRecord
        {
            Id = DataSeeder.Rentals.Max(r => r.Id) + 1,
            ClientId = clientId,
            CarId = carId,
            RentPointId = rentalPointId,
            RentTime = DateTime.Now,
            DurationInDays = durationInDays
        };

        car.IsCurrentlyRented = true;
        car.RentalCount++;
        car.RentalRecords.Add(rental);

        DataSeeder.Rentals.Add(rental);
        return true;
    }

    /// <summary>
    /// ќформить возврат автомобил€
    /// </summary>
    public bool ReturnCar(int rentalId, int returnPointId)
    {
        var rental = DataSeeder.Rentals.FirstOrDefault(r => r.Id == rentalId);
        if (rental == null || rental.ReturnTime.HasValue) return false;

        var returnPoint = _rentalPointRepository.Get(returnPointId);
        if (returnPoint == null) return false;

        rental.ReturnTime = DateTime.Now;
        rental.ReturnPointId = returnPointId;
        rental.ReturnPoint = returnPoint;

        if (rental.Car != null)
        {
            rental.Car.IsCurrentlyRented = false;
        }

        return true;
    }
}