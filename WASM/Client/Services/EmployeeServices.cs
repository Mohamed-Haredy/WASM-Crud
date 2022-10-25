using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
 

namespace EmployeeCrud.Server.Services
{
  
    public class EmployeeServices : IEmployeeServices
    {
        private readonly HttpClient _http;
        private readonly NavigationManager _navigationManager;

        public EmployeeServices(HttpClient http, NavigationManager navigationManager)
        {
            _http = http;
            _navigationManager = navigationManager;
        }

        public List<Employee> employees { get; set; } = new List<Employee>();
        public List<Country> countries { get; set; } = new List<Country>();
        

        public async Task CreateEmployee(Employee emp)
        {
            var result = await _http.PostAsJsonAsync("api/employee", emp);
            await SetEmployee(result);
        }

        private async Task SetEmployee(HttpResponseMessage result)
        {
            
            _navigationManager.NavigateTo("employee");
        }

        public async Task DeleteEmployee(int id)
        {
            var result = await _http.DeleteAsync($"api/employee/{id}");
            await SetEmployee(result);
        }

        public async Task GetCountry()
        {
            var result = await _http.GetFromJsonAsync<List<Country>>("api/employee/country");
            if (result != null)
                countries = result;
        }

        public async Task<Employee> GetEmployeeById(int id)
        {
            var result = await _http.GetFromJsonAsync<Employee>($"api/employee/{id}");
            if (result != null)
                return result;
            throw new Exception("Employee not found!");
        }

        public async Task GetEmployees()
        {
            var result = await _http.GetFromJsonAsync<List<Employee>>("api/employee");
            if (result != null)
                employees = result;
        }

        public async Task UpdateEmployee(Employee emp)
        {
            var result = await _http.PutAsJsonAsync($"api/employee/{emp.EmployeeId}", emp);
            await SetEmployee(result);
        }
    }
}



 

 
   
 
