using CarRentalService.Domain.Model;

namespace CarRentalService.Domain.Services;

/// <summary>
/// ��������� ������� ��� ������ � ������� �����������
/// </summary>
public interface IRentalService
{
    /// <summary>
    /// �������� ������ ���� �����������
    /// </summary>
    public IList<Car> GetAllCars();

    /// <summary>
    /// �������� ������ ��������, ������������ ������������ ������ ����������
    /// </summary>
    public IList<Client> GetClientsByCarModel(string model);

    /// <summary>
    /// �������� ������ ����������� � ������
    /// </summary>
    public IList<Car> GetCurrentlyRentedCars();

    /// <summary>
    /// �������� ���-5 ����� ���������� �����������
    /// </summary>
    public IList<Tuple<string, int>> GetTopRentedCars();

    /// <summary>
    /// �������� ���������� ����� �� ������� ����������
    /// </summary>
    public IList<Tuple<string, int>> GetCarRentalCounts();

    /// <summary>
    /// �������� ������ ������� � ������������ ������ �����
    /// </summary>
    public IList<RentalPoint> GetTopRentalPoints();

    ///// <summary>
    ///// �������� ������ ����������
    ///// </summary>
    //public bool RentalRecord(int clientId, int carId, int rentalPointId, int durationInDays);

    ///// <summary>
    ///// �������� ������� ����������
    ///// </summary>
    //bool ReturnCar(int rentalId, int returnPointId);
}