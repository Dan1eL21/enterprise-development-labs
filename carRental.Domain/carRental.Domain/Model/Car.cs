using System.ComponentModel.DataAnnotations;

namespace CarRentalService.Domain.Model;

/// <summary>
/// Транспортное средство
/// </summary>
public class Car
{
    /// <summary>
    /// Уникальный идентификатор транспортного средства
    /// </summary>
    [Key]
    public required int Id { get; set; }

    /// <summary>
    /// Регистрационный номер
    /// </summary>
    public string? RegistrationNumber { get; set; }

    /// <summary>
    /// Модель автомобиля
    /// </summary>
    public string? Model { get; set; }

    /// <summary>
    /// Цвет автомобиля
    /// </summary>
    public string? Color { get; set; }

    /// <summary>
    /// Список записей об аренде данного автомобиля
    /// </summary>
    public virtual List<RentalRecord> RentalRecords { get; set; } = new();

    /// <summary>
    /// Признак, находится ли автомобиль в аренде в данный момент
    /// </summary>
    public bool IsCurrentlyRented { get; set; }

    /// <summary>
    /// Количество аренд данного автомобиля
    /// </summary>
    public int RentalCount { get; set; }

    /// <summary>
    /// Перегрузка метода, возвращающего строковое представление объекта
    /// </summary>
    /// <returns>Описание автомобиля</returns>
    public override string ToString() =>
        $"{RegistrationNumber} - {Model} ({Color})";
}
