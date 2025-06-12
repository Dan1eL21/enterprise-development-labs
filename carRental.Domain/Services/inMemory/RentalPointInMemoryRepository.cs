using CarRentalService.Domain.Data;
using CarRentalService.Domain.Model;
using CarRentalService.Domain.Services;

namespace CarRentalService.Domain.Services.InMemory;

/// <summary>
/// Имплементация репозитория для пунктов проката, которая хранит коллекцию в оперативной памяти
/// </summary>
public class RentalPointInMemoryRepository : IRentalPointRepository
{
    private readonly List<RentalPoint> _rentalPoints;

    public RentalPointInMemoryRepository()
    {
        _rentalPoints = DataSeeder.RentalPoints;
    }

    public bool Add(RentalPoint entity)
    {
        try
        {
            _rentalPoints.Add(entity);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool Delete(int key)
    {
        var point = Get(key);
        if (point == null) return false;
        return _rentalPoints.Remove(point);
    }

    public RentalPoint? Get(int key) =>
        _rentalPoints.FirstOrDefault(p => p.Id == key);

    public IList<RentalPoint> GetAll() => _rentalPoints;

    public bool Update(RentalPoint entity)
    {
        var point = Get(entity.Id);
        if (point == null) return false;

        point.Name = entity.Name;
        point.Address = entity.Address;
        return true;
    }

    public IList<RentalPoint> GetTopRentalPointsByUsage()
    {
        var maxRentals = _rentalPoints.Max(p => p.RentalsStartedHere.Count);
        
        return _rentalPoints
            .Where(p => p.RentalsStartedHere.Count == maxRentals)
            .OrderBy(p => p.Name)
            .ToList();
    }
}