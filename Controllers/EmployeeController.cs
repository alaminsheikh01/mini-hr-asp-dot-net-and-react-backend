using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly PasswordHasher _passwordHasher;

    public EmployeeController(ApplicationDbContext context)
    {
        _context = context;
        _passwordHasher = new PasswordHasher();
    }

    [HttpPost]
    [Route("signup")]
    public async Task<IActionResult> Signup([FromBody] SignupDto payload)
    {
        if (await _context.SignUp.AnyAsync(u => u.Email == payload.Email))
        {
            return BadRequest("Email already exists.");
        }

        var user = new SignUp
        {
            Email = payload.Email,
            // Password = _passwordHasher.HashPassword(payload.Password),
            Password = payload.Password,
            UserName = payload.UserName,
            Role = "User",
        };

        await _context.SignUp.AddAsync(user);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            Message = "Signup successful.",
        });
    }

    [HttpPost("signin")]
    public async Task<IActionResult> Signin([FromBody] SigninDto model)
    {
        var user = await _context.SignUp.FirstOrDefaultAsync(u => u.Email == model.Email);

        if (user == null)
        {
            return Unauthorized("Invalid email or password.");
        }

        if (user.Password != model.Password)
        {
            return Unauthorized("Invalid email or password.");
        }

        var token = GenerateJwtToken(user);

        return Ok(new
        {
            Message = "Signin successful.",
            Token = token,
            User = new
            {
                user.Id,
                user.Email,
                user.Role,
            }
        });
    }

    private string GenerateJwtToken(SignUp user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSuperSecretKey"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
        new Claim(JwtRegisteredClaimNames.Sub, user.Email),
        new Claim("role", user.Role),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

        var token = new JwtSecurityToken(
            issuer: "YourIssuer",
            audience: "YourAudience",
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    [HttpGet]
    public async Task<ActionResult> GetEmployees(int EmployeeId, string searchText)
    {
        var employees = await (from e in _context.Employee
                               join des in _context.Designation on e.DesignationId equals des.Id
                               join dp in _context.Department on e.DepartmentId equals dp.Id
                               where EmployeeId == 0 || e.Id == EmployeeId || e.FirstName.Contains(searchText)
                               select new EmployeeDTO
                               {
                                   EmployeeId = e.Id,
                                   FirstName = e.FirstName,
                                   LastName = e.LastName,
                                   Email = e.Email,
                                   PhoneNumber = e.PhoneNumber,
                                   Address = e.Address,
                                   City = e.City,
                                   DesignationId = e.DesignationId ?? 0,
                                   DesignationName = des.Name,
                                   DepartmentId = e.DepartmentId ?? 0,
                                   DepartmentName = dp.Name,
                                   DateOfJoining = e.DateOfJoining ?? System.DateTime.Now
                               }).ToListAsync();
        return Ok(employees);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetEmployeeById(int id)
    {
        var employeeById = await (from e in _context.Employee
                                  join des in _context.Designation on e.DesignationId equals des.Id
                                  join dp in _context.Department on e.DepartmentId equals dp.Id
                                  where e.Id == id
                                  select new EmployeeDTO
                                  {
                                      FirstName = e.FirstName,
                                      LastName = e.LastName,
                                      Email = e.Email,
                                      PhoneNumber = e.PhoneNumber,
                                      Address = e.Address,
                                      City = e.City,
                                      DesignationId = e.DesignationId ?? 0,
                                      DesignationName = des.Name,
                                      DepartmentId = e.DepartmentId ?? 0,
                                      DepartmentName = dp.Name,
                                      DateOfJoining = e.DateOfJoining ?? System.DateTime.Now
                                  }).FirstOrDefaultAsync();
        return Ok(employeeById);
    }


    [HttpGet]
    [Route("employeeAssign")]
    public async Task<ActionResult> GetEmployeeAssign(int? EmployeeId)
    {
        var employeeAssign = await (from e in _context.Employee
                                    join des in _context.Designation on e.DesignationId equals des.Id
                                    join dp in _context.Department on e.DepartmentId equals dp.Id
                                    join sa in _context.SalaryAssign on e.Id equals sa.EmployeeId into saGroup
                                    from sa in saGroup.DefaultIfEmpty()
                                    where EmployeeId == null || EmployeeId == 0 || e.Id == EmployeeId
                                    select new EmployeeDTO
                                    {
                                        SalaryAssignId = sa.Id,
                                        EmployeeId = e.Id,
                                        FirstName = e.FirstName,
                                        LastName = e.LastName,
                                        DesignationId = e.DesignationId ?? 0,
                                        DesignationName = des.Name,
                                        DepartmentId = e.DepartmentId ?? 0,
                                        DepartmentName = dp.Name,
                                        BasicSalary = sa.BasicSalary,
                                        GrossSalary = sa.GrossSalary,
                                        Status = sa.Status || false
                                    }).ToListAsync();
        return Ok(employeeAssign);
    }

    [HttpPost]
    [Route("assignSalary")]
    public async Task<ActionResult> AssignSalary([FromBody] SalaryAssign payload)
    {
        if (payload == null)
        {
            return BadRequest("No data found to save");
        }

        if (payload.Id > 0)
        {
            var salaryAssign = await _context.SalaryAssign.FindAsync(payload.Id);
            if (salaryAssign == null)
            {
                return NotFound("Record not found to update");
            }
            salaryAssign.EmployeeId = payload.EmployeeId;
            salaryAssign.BasicSalary = payload.BasicSalary;
            salaryAssign.GrossSalary = payload.GrossSalary;
            salaryAssign.HouseRent = payload.HouseRent;
            salaryAssign.MedicalAllowance = payload.MedicalAllowance;
            salaryAssign.Conveyance = payload.Conveyance;
            salaryAssign.Status = payload.Status;
            try
            {
                await _context.SaveChangesAsync();
                return Ok(salaryAssign);
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
        }
        else
        {
            var salaryAssign = new SalaryAssign
            {
                EmployeeId = payload.EmployeeId,
                BasicSalary = payload.BasicSalary,
                GrossSalary = payload.GrossSalary,
                HouseRent = payload.HouseRent,
                MedicalAllowance = payload.MedicalAllowance,
                Conveyance = payload.Conveyance,
                Status = payload.Status
            };
            await _context.SalaryAssign.AddAsync(salaryAssign);
            await _context.SaveChangesAsync();
            return Ok(salaryAssign);
        }
    }


    [HttpPost]
    public async Task<ActionResult> AddEmployee([FromBody] List<EmployeePayload> payload)
    {

        if (payload == null || payload.Count == 0)
        {
            return BadRequest("No data found to save");
        }

        var employees = payload.Select(data => new Employee
        {
            FirstName = data.FirstName,
            LastName = data.LastName,
            Email = data.Email,
            PhoneNumber = data.PhoneNumber,
            Address = data.Address,
            City = data.City,
            DesignationId = data.DesignationId ?? 0,
            DepartmentId = data.DepartmentId ?? 0,
            DateOfJoining = data.DateOfJoining ?? System.DateTime.Now
        }).ToList();

        await _context.Employee.AddRangeAsync(employees);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Employee added successfully",
            data = employees
        });

    }

    [HttpPut("update/{id}")]
    public async Task<ActionResult> UpdateEmployee([FromBody] EmployeePayload payload, int id)
    {
        var employee = await _context.Employee.FindAsync(id);

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
        employee.DesignationId = payload.DesignationId ?? 0;
        employee.DepartmentId = payload.DepartmentId ?? 0;
        employee.DateOfJoining = payload.DateOfJoining ?? System.DateTime.Now;
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

    [HttpPost]
    [Route("addEmployeeSalary")]
    public async Task<ActionResult> AddEmployeeSalary([FromBody] EmployeeSalaryPayload payload)
    {
        if (payload == null)
        {
            return BadRequest("Invalid payload");
        }

        var salaryAssign = await _context.SalaryAssign
            .FirstOrDefaultAsync(sa => sa.EmployeeId == payload.EmployeeId);

        if (salaryAssign == null)
        {
            return NotFound(new
            {
                message = $"No salary assignment found for this Employee",
            });
        }

        var employeeSalary = new EmployeeSalary
        {
            EmployeeId = payload.EmployeeId,
            GrossSalary = salaryAssign.GrossSalary,
            BasicSalary = salaryAssign.BasicSalary,
            HouseRent = salaryAssign.HouseRent,
            MedicalAllowance = salaryAssign.MedicalAllowance,
            Conveyance = salaryAssign.Conveyance,
            Month = payload.SalaryMonth,
            Year = payload.SalaryYear,
            DepartmentId = payload.DepartmentId,
            DesignationId = payload.DesignationId
        };

        await _context.EmployeeSalary.AddAsync(employeeSalary);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Employee salary added successfully",
            data = employeeSalary
        });
    }

    [HttpGet]
    [Route("employeeSalaryLanding")]
    public async Task<ActionResult> GetEmployeeSalaryLanding(int? EmployeeId)
    {
        var employeeSalary = await (from es in _context.EmployeeSalary
                                    join dp in _context.Department on es.DepartmentId equals dp.Id
                                    join des in _context.Designation on es.DesignationId equals des.Id
                                    join e in _context.Employee on es.EmployeeId equals e.Id
                                    where EmployeeId == null || EmployeeId == 0 || es.EmployeeId == EmployeeId
                                    select new EmployeeSalaryDTO
                                    {
                                        EmployeeSalaryId = es.Id,
                                        EmployeeId = es.EmployeeId,
                                        EmployeeName = e.FirstName + " " + e.LastName,
                                        DepartmentName = dp.Name,
                                        DesignationName = des.Name,
                                        SalaryMonth = es.Month,
                                        SalaryYear = es.Year
                                    }).ToListAsync();
        return Ok(employeeSalary);
    }

    [HttpGet]
    [Route("employeeSalaryDetails")]
    public async Task<ActionResult> GetEmployeeSalaryDetails(int EmployeeSalaryId)
    {
        var employeeSalaryDetails = await (from es in _context.EmployeeSalary
                                           join dp in _context.Department on es.DepartmentId equals dp.Id
                                           join des in _context.Designation on es.DesignationId equals des.Id
                                           join e in _context.Employee on es.EmployeeId equals e.Id
                                           where es.Id == EmployeeSalaryId
                                           select new EmployeeSalaryDTO
                                           {
                                               EmployeeSalaryId = es.Id,
                                               EmployeeId = es.EmployeeId,
                                               EmployeeName = e.FirstName + " " + e.LastName,
                                               DepartmentName = dp.Name,
                                               DesignationName = des.Name,
                                               SalaryMonth = es.Month,
                                               SalaryYear = es.Year,
                                               BasicSalary = es.BasicSalary,
                                               GrossSalary = es.GrossSalary,
                                               HouseRent = es.HouseRent,
                                               MedicalAllowance = es.MedicalAllowance,
                                               Conveyance = es.Conveyance
                                           }).FirstOrDefaultAsync();
        return Ok(employeeSalaryDetails);
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

}