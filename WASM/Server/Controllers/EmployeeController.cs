using WASM.Server.Data;
using WASM.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Xml.Linq;

namespace WASM.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(ApplicationDbContext context, ILogger<EmployeeController> logger)
        {
            _context = context;
            _logger = logger;
        }


        [HttpGet]
        public async Task<ActionResult<List<Employee>>> GetEmployees()
        {
            _logger.LogInformation("start getting employeees...");
            var Employee = await _context.Employees.Include(sh => sh.Country).ToListAsync();
            _logger.LogInformation("employee data has fetched....");
            return Ok(Employee);
        }

        [HttpGet("Country")]
        public async Task<ActionResult<List<Country>>> Country()
        {
            _logger.LogInformation("start getting all Countries...");
            var Country = await _context.Countries.ToListAsync();
            _logger.LogInformation("Countries data has fetched...");
            return Ok(Country);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Country>> GetEmployeeById(int id)
        {
            _logger.LogInformation("start getting  employee By Employee ID...");
            var emp = await _context.Employees
                .Include(h => h.Country)
                .FirstOrDefaultAsync(h => h.EmployeeId == id);
            if (emp == null)
            {
                _logger.LogInformation("Not Found any employee data...");
                return NotFound("Sorry, no Employee here. :/");
            }
            _logger.LogInformation("employee data has fetched by employee id....");

            return Ok(emp);
        }

        [HttpPost]
        public async Task<ActionResult<List<Employee>>> CreateEmployee(Employee employee)
        {
            _logger.LogInformation("createing employee data to stored in database....");

            employee.Country = null;
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            _logger.LogInformation("createing employee data Done....");
            return Ok(await GetDbEmployees());
        }


        [HttpPut("{id}")]

        public async Task<ActionResult<List<Employee>>> UpdateEmployee(Employee employee, int id)
        {
            _logger.LogInformation("start getting  employee By Employee ID to edited.....");
            var EmpD = await _context.Employees
                .Include(sh => sh.Country)
                .FirstOrDefaultAsync(sh => sh.EmployeeId == id);
            _logger.LogInformation("employee data has fetched by employee id....");
            if (EmpD == null)
            {
                _logger.LogInformation("Not Found any employee data...");
                return NotFound("Sorry, but no Employee . :/");
            }

            _context.Entry(EmpD).CurrentValues.SetValues(employee);
            await _context.SaveChangesAsync();
            _logger.LogInformation("employee data has edited succeded....");
            return Ok(await GetDbEmployees());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Employee>>> DeleteEmployee(int id)
        {
            _logger.LogInformation("start getting  employee By Employee ID to deleted.....");
            var dbEmployee = await _context.Employees.FirstOrDefaultAsync(sh => sh.EmployeeId == id);
            _logger.LogInformation("employee data has fetched by employee id....");
            if (dbEmployee == null)
            {
                _logger.LogInformation("Not Found any employee data...");
                return NotFound("Sorry, but no Employee for you. :/");
            }
            _context.Employees.Remove(dbEmployee);
            await _context.SaveChangesAsync();
            _logger.LogInformation("employee data has removed ....");
            return Ok(await GetDbEmployees());
        }

        private async Task<List<Employee>> GetDbEmployees()
        {
            _logger.LogInformation("employee data has fetched with countries....");
            return await _context.Employees.Include(sh => sh.Country).ToListAsync();
        }







    }
}
