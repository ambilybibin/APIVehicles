namespace Vehicles.Api.Repositories
{
    public interface IVehiclesRepository
    {
        Task<IEnumerable<Vehicle>> GetAllCarsList();
    }
}
