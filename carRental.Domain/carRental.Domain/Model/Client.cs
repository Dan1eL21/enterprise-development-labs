using System.ComponentModel.DataAnnotations;

namespace CarRentalService.Domain.Model;

/// <summary>
/// Клиент
/// </summary>
public class Client
{
    /// <summary>
    /// Уникальный идентификатор клиента
    /// </summary>
    [Key]
    public required int Id { get; set; }

    /// <summary>
    /// Паспортные данные
    /// </summary>
    public string? PassportNumber { get; set; }

    /// <summary>
    /// Фамилия
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// Имя
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// Отчество
    /// </summary>
    public string? Patronymic { get; set; }

    /// <summary>
    /// Дата рождения
    /// </summary>
    public DateTime? BirthDate { get; set; }

    /// <summary>
    /// Список аренд
    /// </summary>
    public virtual List<RentalRecord> RentalRecords { get; set; } = new();

    /// <summary>
    /// Полное имя клиента
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Строковое представление клиента
    /// </summary>
    public override string ToString() => 
        $"{FullName} ({PassportNumber})";
}
