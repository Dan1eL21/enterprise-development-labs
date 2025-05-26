using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRentalService.Domain.Model;

/// <summary>
/// Запись об аренде автомобиля
/// </summary>
public class RentalRecord
{
    /// <summary>
    /// Уникальный идентификатор записи
    /// </summary>
    [Key]
    public required int Id { get; set; }

    /// <summary>
    /// Идентификатор клиента
    /// </summary>
    public required int ClientId { get; set; }

    /// <summary>
    /// Клиент, взявший автомобиль в аренду
    /// </summary>
    public virtual Client? Client { get; set; }

    /// <summary>
    /// Идентификатор автомобиля
    /// </summary>
    public required int CarId { get; set; }

    /// <summary>
    /// Автомобиль, взятый в аренду
    /// </summary>
    public virtual Car? Car { get; set; }

    /// <summary>
    /// Идентификатор пункта аренды (выдачи)
    /// </summary>
    public required int RentPointId { get; set; }

    /// <summary>
    /// Пункт аренды, где выдали автомобиль
    /// </summary>
    public virtual RentalPoint? RentPoint { get; set; }

    /// <summary>
    /// Время выдачи
    /// </summary>
    public required DateTime RentTime { get; set; }

    /// <summary>
    /// Срок аренды (в днях)
    /// </summary>
    public required int DurationInDays { get; set; }

    /// <summary>
    /// Время возврата (null — если авто не возвращено)
    /// </summary>
    public DateTime? ReturnTime { get; set; }

    /// <summary>
    /// Идентификатор пункта возврата
    /// </summary>
    public int? ReturnPointId { get; set; }

    /// <summary>
    /// Пункт возврата
    /// </summary>
    public virtual RentalPoint? ReturnPoint { get; set; }

    /// <summary>
    /// Строковое представление записи об аренде
    /// </summary>
    public override string ToString() =>
        $"{Client?.FullName} арендовал {Car?.Model} на {DurationInDays} дн.";
}
    