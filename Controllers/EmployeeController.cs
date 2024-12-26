using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public EmployeeController(ApplicationDbContext context)
    {
        _context = context;
    }


    [HttpGet]
    public async Task<ActionResult> GetEmployees(int EmployeeId, string searchText)
    {
        var employees = await (from e in _context.Employee
                               join des in _context.Designation on e.DesignationId equals des.Id
                               join dp in _context.Department on e.DepartmentId equals dp.Id
                               where (EmployeeId == 0 || e.Id == EmployeeId) || e.FirstName.Contains(searchText)
                               select new EmployeeDTO
                               {
                                   FirstName = e.FirstName,
                                   LastName = e.LastName,
                                   Email = e.Email,
                                   PhoneNumber = e.PhoneNumber,
                                   Address = e.Address,
                                   City = e.City,
                                   DesignationId = e.DesignationId,
                                   DesignationName = des.Name,
                                   DepartmentId = e.DepartmentId,
                                   DepartmentName = dp.Name
                               }).ToListAsync();
        return Ok(employees);
    }

    [HttpPost]
    public async Task<ActionResult> AddEmployee([FromBody] EmployeePayload payload)
    {
        var employee = new Employee
        {
            FirstName = payload.FirstName,
            LastName = payload.LastName,
            Email = payload.Email,
            PhoneNumber = payload.PhoneNumber,
            Address = payload.Address,
            City = payload.City,
            DesignationId = payload.DesignationId,
            DepartmentId = payload.DepartmentId
        };
        await _context.Employee.AddAsync(employee);
        await _context.SaveChangesAsync();
        return Ok(employee);
    }

    [HttpPost]
    [Route("department")]
    public async Task<ActionResult> AddDepartment([FromBody] DepartmentPayload payload)
    {
        var department = new Department
        {
            Name = payload.Name,
            Code = payload.Code,
            IsActive = true
        };
        await _context.Department.AddAsync(department);
        await _context.SaveChangesAsync();
        return Ok(department);
    }

    [HttpGet]
    [Route("departmentDDL")]
    public async Task<ActionResult> GetDepartmentDDL()
    {
        var departments = await _context.Department
        .Select(d => new DepartmentDTO
        {
            value = d.Id,
            label = d.Name
        }).ToListAsync();
        return Ok(departments);
    }


    [HttpPost]
    [Route("designation")]
    public async Task<ActionResult> AddDesignation([FromBody] DesignationPayload payload)
    {
        var designation = new Designation
        {
            Name = payload.Name,
            Code = payload.Code,
            IsActive = true
        };
        await _context.Designation.AddAsync(designation);
        await _context.SaveChangesAsync();
        return Ok(designation);
    }

    [HttpGet]
    [Route("designationDDL")]
    public async Task<ActionResult> GetDesignationDDL()
    {
        var designations = await _context.Designation
        .Select(d => new DesignationDTO
        {
            value = d.Id,
            label = d.Name
        }).ToListAsync();
        return Ok(designations);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetEmployeeById(int id)
    {
        var employee = await _context.Employee.FindAsync(id);

        if (employee == null)
        {
            return NotFound();
        }
        return Ok(employee);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateEmployee(int id, EmployeePayload payload)
    {
        var employee = await _context.Employee.FindAsync(id);
        //     var employee = await _context.Employee
        //   .SingleAsync(e => e.Id == id);

        if (employee == null)
        {
            return NotFound();
        }

        employee.FirstName = payload.FirstName;
        employee.LastName = payload.LastName;
        employee.Email = payload.Email;
        employee.PhoneNumber = payload.PhoneNumber;
        employee.Address = payload.Address;
        employee.City = payload.City;

        try
        {
            await _context.SaveChangesAsync();
            return Ok(employee);
        }
        catch (DbUpdateConcurrencyException)
        {
            return NotFound();
        }
    }

}