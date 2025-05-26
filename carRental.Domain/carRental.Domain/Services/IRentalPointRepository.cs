using CarRentalService.Domain.Model;

namespace CarRentalService.Domain.Services;

public interface IRentalPointRepository : IRepository<RentalPoint, int>
{
    /// <summary>
    /// Получить пункты проката с максимальным числом аренд, упорядоченные по названию
    /// </summary>
    public IList<RentalPoint> GetTopRentalPointsByUsage();
}
