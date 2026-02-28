using AppCore.Models;

namespace AppCore.Repositories;

public interface ICarRepository : IGenericRepository<Car>
{
    Task<Car?> FindByPlateNumber(string plate);
}