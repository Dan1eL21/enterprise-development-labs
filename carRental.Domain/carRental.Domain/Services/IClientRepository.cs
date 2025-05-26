using CarRentalService.Domain.Model;

namespace CarRentalService.Domain.Services;

public interface IClientRepository : IRepository<Client, int>
{
    /// <summary>
    /// Получить всех клиентов, арендовавших автомобили указанной модели
    /// </summary>
    public IList<Client> GetClientsByVehicleModel(string model);
}
