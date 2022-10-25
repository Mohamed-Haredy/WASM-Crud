using WASM.Shared;
namespace WASM.Client.Services
{
    public interface IEmployeeServices
    {
        List<Employee> employees { get; set; }
        List<Country>  countries { get; set; }
        Task GetCountry();
        Task GetEmployees();
        Task<Employee> GetEmployeeById(int id);
        Task CreateEmployee(Employee emp);
        Task UpdateEmployee(Employee emp);
        Task DeleteEmployee(int id);

    }
}
