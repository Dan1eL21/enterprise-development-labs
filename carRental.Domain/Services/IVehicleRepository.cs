using CarRentalService.Domain.Model;
using CarRentalService.Domain.Services;

namespace CarRentalService.Domain.Services;

/// <summary>
/// Репозиторий для работы с транспортными средствами с дополнительной аналитикой
/// </summary>
public interface IVehicleRepository : IRepository<Car, int>
{
    /// <summary>
    /// Получить список автомобилей, которые находятся в аренде
    /// </summary>
    /// <returns>Список арендованных автомобилей</returns>
    public IList<Car> GetCurrentlyRentedVehicles();

    /// <summary>
    /// Получить топ 5 наиболее часто арендуемых автомобилей
    /// </summary>
    /// <returns>Список пар (описание автомобиля, количество аренд)</returns>
    public IList<Tuple<string, int>> GetTop5MostRentedVehicles();

    /// <summary>
    /// Получить количество аренд для каждого автомобиля
    /// </summary>
    /// <returns>Список пар (описание автомобиля, количество аренд)</returns>
    public IList<Tuple<string, int>> GetRentalCountsByVehicle();
}
