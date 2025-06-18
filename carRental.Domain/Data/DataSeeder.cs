using CarRentalService.Domain.Model;

namespace CarRentalService.Domain.Data;

/// <summary>
/// Класс для заполнения коллекций данными
/// </summary>
public static class DataSeeder
{
    public static readonly List<Car> Cars =
    [
        new() { Id = 1, RegistrationNumber = "A123BC", Model = "Toyota Camry", Color = "Черный" },
        new() { Id = 2, RegistrationNumber = "B456DE", Model = "Hyundai Solaris", Color = "Белый" },
        new() { Id = 3, RegistrationNumber = "C789FG", Model = "Kia Rio", Color = "Синий" },
        new() { Id = 4, RegistrationNumber = "D101HI", Model = "Ford Focus", Color = "Красный" },
        new() { Id = 5, RegistrationNumber = "E112JK", Model = "BMW X5", Color = "Серебристый" },
    ];

    public static readonly List<Client> Clients =
    [
        new() { Id = 1, PassportNumber = "1234 567890", LastName = "Иванов", FirstName = "Иван", Patronymic = "Иванович", BirthDate = new DateTime(1990, 5, 12) },
        new() { Id = 2, PassportNumber = "2345 678901", LastName = "Петров", FirstName = "Петр", Patronymic = "Петрович", BirthDate = new DateTime(1985, 8, 23) },
        new() { Id = 3, PassportNumber = "3456 789012", LastName = "Сидоров", FirstName = "Сидор", Patronymic = "Сидорович", BirthDate = new DateTime(1992, 1, 15) },
        new() { Id = 4, PassportNumber = "4567 890123", LastName = "Кузнецов", FirstName = "Алексей", Patronymic = "Викторович",  BirthDate = new DateTime(1980, 3, 3) },
    ];

    public static readonly List<RentalPoint> RentalPoints =
    [
        new() { Id = 1, Name = "Центральный офис", Address = "ул. Ленина, 1" },
        new() { Id = 2, Name = "Северный филиал", Address = "ул. Победы, 45" },
        new() { Id = 3, Name = "Южный филиал", Address = "пр. Мира, 99" },
    ];

    public static readonly List<RentalRecord> Rentals =
    [
        new() { Id = 1, CarId = 1, ClientId = 1, RentPointId = 1, RentTime = DateTime.Now.AddDays(-10), ReturnTime = DateTime.Now.AddDays(-8), DurationInDays = 2 },
        new() { Id = 2, CarId = 2, ClientId = 2, RentPointId = 2, RentTime = DateTime.Now.AddDays(-7), ReturnTime = DateTime.Now.AddDays(-5), DurationInDays = 2 },
        new() { Id = 3, CarId = 1, ClientId = 3, RentPointId = 1, RentTime = DateTime.Now.AddDays(-3), ReturnTime = null,  DurationInDays = 7 },
        new() { Id = 4, CarId = 3, ClientId = 1, RentPointId = 3, RentTime = DateTime.Now.AddDays(-20), ReturnTime = DateTime.Now.AddDays(-15), DurationInDays = 5 },
        new() { Id = 5, CarId = 1, ClientId = 2, RentPointId = 1, RentTime = DateTime.Now.AddDays(-30), ReturnTime = DateTime.Now.AddDays(-29), DurationInDays = 1 },
    ];

    static DataSeeder()
    {
        // Инициализация коллекций
        foreach (var car in Cars)
        {
            car.RentalRecords = new List<RentalRecord>();
        }

        foreach (var client in Clients)
        {
            client.RentalRecords = new List<RentalRecord>();
        }

        foreach (var point in RentalPoints)
        {
            point.RentalsStartedHere = new List<RentalRecord>();
            point.RentalsReturnedHere = new List<RentalRecord>();
        }

        // Установка навигационных свойств
        foreach (var rental in Rentals)
        {
            rental.Car = Cars.FirstOrDefault(c => c.Id == rental.CarId);
            rental.Client = Clients.FirstOrDefault(c => c.Id == rental.ClientId);
            rental.RentPoint = RentalPoints.FirstOrDefault(rp => rp.Id == rental.RentPointId);
            
            if (rental.Car != null)
            {
                rental.Car.RentalRecords.Add(rental);
                rental.Car.IsCurrentlyRented = rental.ReturnTime == null;
                rental.Car.RentalCount = Rentals.Count(r => r.CarId == rental.CarId);
            }

            if (rental.Client != null)
            {
                rental.Client.RentalRecords.Add(rental);
            }

            if (rental.RentPoint != null)
            {
                rental.RentPoint.RentalsStartedHere.Add(rental);
            }
            /// <summary>
            // Установка пункта возврата, если автомобиль возвращен
            /// </summary>
            if (rental.ReturnTime.HasValue)
            {
                rental.ReturnPoint = rental.RentPoint; // По умолчанию возврат в тот же пункт
                if (rental.ReturnPoint != null)
                {
                    rental.ReturnPoint.RentalsReturnedHere.Add(rental);
                }
            }
        }
    }
}
