using CarRentalService.Domain.Model;

namespace CarRentalService.Domain.Services;

/// <summary>
/// »нтерфейс сервиса дл€ работы с арендой автомобилей
/// </summary>
public interface IRentalService
{
    /// <summary>
    /// ѕолучить список всех автомобилей
    /// </summary>
    public IList<Car> GetAllCars();

    /// <summary>
    /// ѕолучить список клиентов, арендовавших определенную модель автомобил€
    /// </summary>
    public IList<Client> GetClientsByCarModel(string model);

    /// <summary>
    /// ѕолучить список автомобилей в аренде
    /// </summary>
    public IList<Car> GetCurrentlyRentedCars();

    /// <summary>
    /// ѕолучить топ-5 самых арендуемых автомобилей
    /// </summary>
    public IList<Tuple<string, int>> GetTopRentedCars();

    /// <summary>
    /// ѕолучить статистику аренд по каждому автомобилю
    /// </summary>
    public IList<Tuple<string, int>> GetCarRentalCounts();

    /// <summary>
    /// ѕолучить пункты проката с максимальным числом аренд
    /// </summary>
    public IList<RentalPoint> GetTopRentalPoints();

    ///// <summary>
    ///// ќформить аренду автомобил€
    ///// </summary>
    //public bool RentalRecord(int clientId, int carId, int rentalPointId, int durationInDays);

    ///// <summary>
    ///// ќформить возврат автомобил€
    ///// </summary>
    //bool ReturnCar(int rentalId, int returnPointId);
}