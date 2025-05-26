using CarRentalService.Domain.Data;
using CarRentalService.Domain.Model;
using CarRentalService.Domain.Services;

namespace CarRentalService.Domain.Services.InMemory;

/// <summary>
/// Имплементация репозитория для клиентов, которая хранит коллекцию в оперативной памяти
/// </summary>
public class ClientInMemoryRepository : IClientRepository
{
    private List<Client> _clients;

    public ClientInMemoryRepository()
    {
        _clients = DataSeeder.Clients;
    }

    public bool Add(Client entity)
    {
        try
        {
            _clients.Add(entity);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool Delete(int key)
    {
        try
        {
            var client = Get(key);
            if (client != null)
                _clients.Remove(client);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool Update(Client entity)
    {
        try
        {
            Delete(entity.Id);
            Add(entity);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public Client? Get(int key) =>
        _clients.FirstOrDefault(c => c.Id == key);

    public IList<Client> GetAll() =>
        _clients;

    public IList<Client> GetClientsByVehicleModel(string model)
    {
        return _clients
            .Where(client => client.RentalRecords != null &&
                   client.RentalRecords.Any(r => r.Car != null && 
                         r.Car.Model == model))
            .OrderBy(c => c.LastName)
            .ThenBy(c => c.FirstName)
            .ThenBy(c => c.Patronymic)
            .ToList();
    }
}
