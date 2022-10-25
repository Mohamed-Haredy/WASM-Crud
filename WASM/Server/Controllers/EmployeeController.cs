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
            var Country = await _context.Countries.ToListAsync();
            return Ok(Country);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Country>> GetEmployeeById(int id)
        {
            var emp = await _context.Employees
                .Include(h => h.Country)
                .FirstOrDefaultAsync(h => h.EmployeeId == id);
            if (emp == null)
            {
                return NotFound("Sorry, no Employee here. :/");
            }
            return Ok(emp);
        }

        [HttpPost]
        public async Task<ActionResult<List<Employee>>> CreateEmployee(Employee employee)
        {
            employee.Country = null;
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return Ok(await GetDbEmployees());
        }


        [HttpPut("{id}")]

        public async Task<ActionResult<List<Employee>>> UpdateEmployee(Employee employee, int id)
        {
            var EmpD = await _context.Employees
                .Include(sh => sh.Country)
                .FirstOrDefaultAsync(sh => sh.EmployeeId == id);
            if (EmpD == null)
                return NotFound("Sorry, but no Employee . :/");

            //EmpD.FirstName = employee.FirstName;
            //EmpD.LastName = employee.LastName;
            //EmpD.Country = employee.Country;
            //EmpD.PhoneNumber = employee.PhoneNumber;
            //EmpD.Comment = employee.Comment;
            //EmpD.BirthDate = employee.BirthDate;
            //EmpD.JoinedDate = employee.JoinedDate;
            //EmpD.Email = employee.Email;
            //EmpD.ExitDate = employee.ExitDate;

            //_context.Employees.Add(EmpD);
            //_context.Entry(EmpD).State = EntityState.Modified;
            _context.Entry(EmpD).CurrentValues.SetValues(employee);


            await _context.SaveChangesAsync();

            return Ok(await GetDbEmployees());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Employee>>> DeleteEmployee(int id)
        {
            var dbEmployee = await _context.Employees
                .Include(sh => sh.Country)
                .FirstOrDefaultAsync(sh => sh.EmployeeId == id);
            if (dbEmployee == null)
                return NotFound("Sorry, but no Employee for you. :/");

            _context.Employees.Remove(dbEmployee);
            await _context.SaveChangesAsync();

            return Ok(await GetDbEmployees());
        }

        private async Task<List<Employee>> GetDbEmployees()
        {
            return await _context.Employees.Include(sh => sh.Country).ToListAsync();
        }







    }
}
