using System.ComponentModel.DataAnnotations;

namespace CarRentalService.Domain.Model;

/// <summary>
/// Пункт проката
/// </summary>
public class RentalPoint
{
    [Key]
    public required int Id { get; set; }

    /// <summary>
    /// Название пункта проката
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Адрес пункта проката
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Список выданных автомобилей
    /// </summary>
    public virtual List<RentalRecord> RentalsStartedHere { get; set; } = new();

    /// <summary>
    /// Список возвращенных автомобилей
    /// </summary>
    public virtual List<RentalRecord> RentalsReturnedHere { get; set; } = new();

    public override string ToString() =>
        $"{Name} ({Address})";
}
