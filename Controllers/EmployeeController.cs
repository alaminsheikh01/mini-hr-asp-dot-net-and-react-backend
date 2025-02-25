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
            Role = payload.Role,
            IsMasterUser = payload.IsMasterUser || false
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
                user.UserName,
                user.IsMasterUser
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
                                      EmployeeCode = e.EmployeeCode,
                                      Email = e.Email,
                                      PhoneNumber = e.PhoneNumber,
                                      Address = e.Address,
                                      Gender = e.Gender,
                                      Grade = e.Grade ?? 0,
                                      InsuranceNumber = e.InsuranceNumber,
                                      TINNumber = e.TINNumber ?? 0,
                                      EmployeeStatus = e.EmployeeStatus,
                                      DesignationId = e.DesignationId ?? 0,
                                      DesignationName = des.Name,
                                      DepartmentId = e.DepartmentId ?? 0,
                                      DepartmentName = dp.Name,
                                      DateOfJoining = e.DateOfJoining ?? System.DateTime.Now,
                                      DateOfBirth = e.DateOfBirth ?? System.DateTime.Now

                                  }).FirstOrDefaultAsync();
        return Ok(employeeById);
    }

    [HttpPost]
    public async Task<ActionResult> AddEmployee([FromBody] List<EmployeePayload> payload)
    {

        if (payload == null || payload.Count == 0)
        {
            return BadRequest("No data found to save");
        }

        var payloadEmails = payload.Select(p => p.Email).ToList();

        var existingEmails = await _context.Employee
            .Where(e => payloadEmails.Contains(e.Email))
            .Select(e => e.Email)
            .ToListAsync();

        if (existingEmails.Any())
        {
            return BadRequest(new
            {
                message = $"Emails already exist",
            });
        }

        var employees = payload.Select(data => new Employee
        {
            FirstName = data.FirstName,
            LastName = data.LastName,
            EmployeeCode = data.EmployeeCode ?? "",
            Email = data.Email,
            PhoneNumber = data.PhoneNumber,
            Address = data.Address ?? "",
            Gender = data.Gender ?? "",
            Grade = data.Grade ?? 0,
            InsuranceNumber = data.InsuranceNumber ?? "",
            TINNumber = data.TINNumber ?? 0,
            EmployeeStatus = data.EmployeeStatus ?? "",
            DesignationId = data.DesignationId ?? 0,
            DepartmentId = data.DepartmentId ?? 0,
            DateOfJoining = data.DateOfJoining ?? System.DateTime.Now,
            DateOfBirth = data.DateOfJoining ?? System.DateTime.Now
        }).ToList();

        var signUp = payload.Select(data => new SignUp
        {
            Email = data.Email,
            Password = "123456",
            UserName = data.FirstName + " " + data.LastName,
            Role = "User",
            IsMasterUser = false
        }).ToList();

        await _context.Employee.AddRangeAsync(employees);
        await _context.SignUp.AddRangeAsync(signUp);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Employee added successfully",
            data = employees,
            user = signUp
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
        employee.EmployeeCode = payload.EmployeeCode;
        employee.Email = payload.Email;
        employee.PhoneNumber = payload.PhoneNumber;
        employee.Address = payload.Address;
        employee.Gender = payload.Gender ?? "";
        employee.Grade = payload.Grade ?? 0;
        employee.InsuranceNumber = payload.InsuranceNumber ?? "";
        employee.TINNumber = payload.TINNumber ?? 0;
        employee.EmployeeStatus = payload.EmployeeStatus ?? "";
        employee.DesignationId = payload.DesignationId ?? 0;
        employee.DepartmentId = payload.DepartmentId ?? 0;
        employee.DateOfJoining = payload.DateOfJoining ?? System.DateTime.Now;
        employee.DateOfBirth = payload.DateOfBirth ?? System.DateTime.Now;
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
                                        Status = sa.Status || false,
                                        Grade = e.Grade ?? 0
                                    }).ToListAsync();
        return Ok(employeeAssign);
    }

    [HttpGet]
    [Route("salaryAssignById")]
    public async Task<ActionResult> GetSalaryAssignById(int SalaryAssignId)
    {
        var salaryAssign = await (from sa in _context.SalaryAssign
                                  join e in _context.Employee on sa.EmployeeId equals e.Id
                                  join des in _context.Designation on e.DesignationId equals des.Id
                                  join dp in _context.Department on e.DepartmentId equals dp.Id
                                  where sa.Id == SalaryAssignId
                                  select new SalaryAssignDTO
                                  {
                                      SalaryAssignId = sa.Id,
                                      EmployeeId = e.Id,
                                      EmployeeName = e.FirstName + " " + e.LastName,
                                      DepartmentId = e.DepartmentId ?? 0,
                                      DepartmentName = dp.Name,
                                      DesignationId = e.DesignationId ?? 0,
                                      DesignationName = des.Name,
                                      BasicSalary = sa.BasicSalary,
                                      GrossSalary = sa.GrossSalary,
                                      MedicalAllowance = sa.MedicalAllowance,
                                      HouseRent = sa.HouseRent,
                                      Conveyance = sa.Conveyance,
                                      AdvanceSalary = sa.AdvanceSalary,
                                      CarAllowance = sa.CarAllowance,
                                      CcCharge = sa.CcCharge,
                                      LunchDeduction = sa.LunchDeduction,
                                      LoanRepayment = sa.LoanRepayment,
                                      LastMonthLoanPayment = sa.LastMonthLoanPayment,
                                      PF = sa.PF,
                                      TotalDeductions = sa.TotalDeductions,
                                      NetSalary = sa.NetSalary,
                                      Grade = e.Grade ?? 0,
                                      PerformanceBonus = sa.PerformanceBonus,
                                      FestivalBonus = sa.FestivalBonus,
                                      IncomeTax = sa.IncomeTax
                                  }).FirstOrDefaultAsync();
        return Ok(salaryAssign);
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
            salaryAssign.MedicalAllowance = payload.MedicalAllowance;
            salaryAssign.Conveyance = payload.Conveyance;
            salaryAssign.Status = payload.Status;
            salaryAssign.AdvanceSalary = payload.AdvanceSalary;
            salaryAssign.CarAllowance = payload.CarAllowance;
            salaryAssign.CcCharge = payload.CcCharge;
            salaryAssign.LunchDeduction = payload.LunchDeduction;
            salaryAssign.LoanRepayment = payload.LoanRepayment;
            salaryAssign.LastMonthLoanPayment = payload.LastMonthLoanPayment;
            salaryAssign.PF = payload.PF;
            salaryAssign.TotalDeductions = payload.TotalDeductions;
            salaryAssign.NetSalary = payload.NetSalary;
            salaryAssign.HouseRent = payload.HouseRent;
            salaryAssign.PerformanceBonus = payload.PerformanceBonus;
            salaryAssign.FestivalBonus = payload.FestivalBonus;
            salaryAssign.IncomeTax = payload.IncomeTax;

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
                MedicalAllowance = payload.MedicalAllowance,
                Conveyance = payload.Conveyance,
                Status = payload.Status,
                AdvanceSalary = payload.AdvanceSalary,
                CarAllowance = payload.CarAllowance,
                CcCharge = payload.CcCharge,
                LunchDeduction = payload.LunchDeduction,
                LoanRepayment = payload.LoanRepayment,
                LastMonthLoanPayment = payload.LastMonthLoanPayment,
                PF = payload.PF,
                TotalDeductions = payload.TotalDeductions,
                NetSalary = payload.NetSalary,
                HouseRent = payload.HouseRent,
                PerformanceBonus = payload.PerformanceBonus,
                FestivalBonus = payload.FestivalBonus,
                IncomeTax = payload.IncomeTax

            };
            await _context.SalaryAssign.AddAsync(salaryAssign);
            await _context.SaveChangesAsync();
            return Ok(salaryAssign);
        }
    }

    [HttpGet]
    [Route("salaryAssignLanding")]
    public async Task<ActionResult> GetSalaryAssignLanding(int? EmployeeId)
    {
        var salaryAssign = await (from sa in _context.SalaryAssign
                                  join e in _context.Employee on sa.EmployeeId equals e.Id
                                  join des in _context.Designation on e.DesignationId equals des.Id
                                  join dp in _context.Department on e.DepartmentId equals dp.Id
                                  where EmployeeId == null || EmployeeId == 0 || sa.EmployeeId == EmployeeId
                                  select new SalaryAssignDTO
                                  {
                                      SalaryAssignId = sa.Id,
                                      EmployeeId = e.Id,
                                      EmployeeName = e.FirstName + " " + e.LastName,
                                      DepartmentId = e.DepartmentId ?? 0,
                                      DepartmentName = dp.Name,
                                      DesignationId = e.DesignationId ?? 0,
                                      DesignationName = des.Name,
                                      BasicSalary = sa.BasicSalary,
                                      GrossSalary = sa.GrossSalary,
                                      MedicalAllowance = sa.MedicalAllowance,
                                      HouseRent = sa.HouseRent,
                                      Conveyance = sa.Conveyance,
                                      AdvanceSalary = sa.AdvanceSalary,
                                      CarAllowance = sa.CarAllowance,
                                      CcCharge = sa.CcCharge,
                                      LunchDeduction = sa.LunchDeduction,
                                      LoanRepayment = sa.LoanRepayment,
                                      LastMonthLoanPayment = sa.LastMonthLoanPayment,
                                      PF = sa.PF,
                                      TotalDeductions = sa.TotalDeductions,
                                      NetSalary = sa.NetSalary,
                                      Grade = e.Grade ?? 0,
                                      PerformanceBonus = sa.PerformanceBonus,
                                      FestivalBonus = sa.FestivalBonus,
                                      IncomeTax = sa.IncomeTax
                                  }).ToListAsync();
        return Ok(salaryAssign);
    }

    [HttpPost]
    [Route("addEmployeeSalary")]
    public async Task<ActionResult> AddEmployeeSalary([FromBody] EmployeeSalaryPayload payload)
    {
        if (payload == null || payload.EmployeeIds == null || !payload.EmployeeIds.Any())
        {
            return BadRequest("Invalid payload or missing employee IDs.");
        }

        Random random = new Random();
        int randomNumber = random.Next(1, 1000);

        string salaryCode = $"SAL-{payload.SalaryYear}{payload.SalaryMonth.Substring(0, 3).ToUpper()}-{randomNumber}";


        var salaryHeader = new SalaryHeader
        {
            SalaryCode = salaryCode,
            Month = payload.SalaryMonth,
            Year = payload.SalaryYear,
            DepartmentId = payload.DepartmentId,
            DesignationId = payload.DesignationId
        };

        await _context.SalaryHeader.AddAsync(salaryHeader);
        await _context.SaveChangesAsync();

        var employeeSalaries = new List<EmployeeSalary>();

        foreach (var employeeId in payload.EmployeeIds)
        {
            var salaryAssign = await _context.SalaryAssign
                .FirstOrDefaultAsync(sa => sa.EmployeeId == employeeId);

            if (salaryAssign == null)
            {
                return NotFound(new
                {
                    message = $"No salary assignment found for Employee ID {employeeId}",
                });
            }

            var employeeSalary = new EmployeeSalary
            {
                EmployeeId = employeeId,
                GrossSalary = salaryAssign.GrossSalary,
                BasicSalary = salaryAssign.BasicSalary,
                MedicalAllowance = salaryAssign.MedicalAllowance,
                Conveyance = salaryAssign.Conveyance,
                Month = payload.SalaryMonth,
                Year = payload.SalaryYear,
                DepartmentId = payload.DepartmentId,
                DesignationId = payload.DesignationId,
                SalaryHeaderId = salaryHeader.Id
            };

            employeeSalaries.Add(employeeSalary);
        }

        await _context.EmployeeSalary.AddRangeAsync(employeeSalaries);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Employee salaries added successfully",
            salaryCode,
            employeesProcessed = payload.EmployeeIds.Count
        });
    }

    [HttpGet]
    [Route("employeeSalaryLanding")]
    public async Task<ActionResult> GetEmployeeSalaryLanding(int? EmployeeId)
    {
        var employeeSalaries = await (from es in _context.EmployeeSalary
                                      join sh in _context.SalaryHeader on es.SalaryHeaderId equals sh.Id
                                      join sa in _context.SalaryAssign on es.EmployeeId equals sa.EmployeeId
                                      join e in _context.Employee on es.EmployeeId equals e.Id
                                      join dp in _context.Department on e.DepartmentId equals dp.Id
                                      join des in _context.Designation on e.DesignationId equals des.Id
                                      where EmployeeId == null || es.EmployeeId == EmployeeId
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
                                          MedicalAllowance = es.MedicalAllowance,
                                          Conveyance = es.Conveyance,
                                          SalaryCode = sh.SalaryCode,
                                          AdvanceSalary = sa.AdvanceSalary,
                                          CarAllowance = sa.CarAllowance,
                                          CcCharge = sa.CcCharge,
                                          LunchDeduction = sa.LunchDeduction,
                                          LoanRepayment = sa.LoanRepayment,
                                          LastMonthLoanPayment = sa.LastMonthLoanPayment,
                                          PF = sa.PF,
                                          TotalDeductions = sa.TotalDeductions,
                                          NetSalary = sa.NetSalary,
                                          HouseRent = sa.HouseRent,
                                          PerformanceBonus = sa.PerformanceBonus,
                                          FestivalBonus = sa.FestivalBonus,
                                          IncomeTax = sa.IncomeTax,
                                          Grade = e.Grade ?? 0

                                      }).ToListAsync();

        if (!employeeSalaries.Any())
        {
            return NotFound(new { message = "No employee salary records found." });
        }

        return Ok(employeeSalaries);
    }


    [HttpGet]
    [Route("employeeSalaryHeader")]
    public async Task<ActionResult> GetEmployeeSalaryHeader(string salaryCode)
    {
        var employeeSalaryHeader = await (from sh in _context.SalaryHeader
                                          where string.IsNullOrEmpty(salaryCode) || sh.SalaryCode.Trim().ToLower() == salaryCode.Trim().ToLower()
                                          group sh by new
                                          {
                                              sh.Id,
                                              sh.SalaryCode,
                                              sh.Month,
                                              sh.Year
                                          } into groupedData
                                          select new SalaryHeaderDataDTO
                                          {
                                              SalaryHeaderId = groupedData.Key.Id,
                                              SalaryCode = groupedData.Key.SalaryCode,
                                              SalaryMonth = groupedData.Key.Month,
                                              SalaryYear = groupedData.Key.Year
                                          })
                                          .ToListAsync();

        if (!employeeSalaryHeader.Any())
        {
            return NotFound(new { message = "No records found for the provided SalaryCode." });
        }

        return Ok(employeeSalaryHeader);
    }


    [HttpGet]
    [Route("detailsBySalaryCode")]
    public async Task<ActionResult> GetDetailsBySalaryCode(string salaryCode)
    {
        var employeeSalaryDetails = await (from sh in _context.SalaryHeader
                                           where sh.SalaryCode == salaryCode

                                           join es in _context.EmployeeSalary on sh.Id equals es.SalaryHeaderId
                                           join sa in _context.SalaryAssign on es.EmployeeId equals sa.EmployeeId
                                           join e in _context.Employee on es.EmployeeId equals e.Id
                                           join dp in _context.Department on e.DepartmentId equals dp.Id
                                           join des in _context.Designation on e.DesignationId equals des.Id

                                           select new EmployeeSalaryDTO
                                           {
                                               EmployeeSalaryId = es.Id,
                                               EmployeeId = e.Id,
                                               EmployeeName = e.FirstName + " " + e.LastName,
                                               DepartmentName = dp.Name,
                                               DesignationName = des.Name,
                                               SalaryMonth = es.Month,
                                               SalaryYear = es.Year,
                                               BasicSalary = es.BasicSalary,
                                               GrossSalary = es.GrossSalary,
                                               MedicalAllowance = es.MedicalAllowance,
                                               Conveyance = es.Conveyance,
                                               SalaryCode = sh.SalaryCode,
                                               AdvanceSalary = sa.AdvanceSalary,
                                               CarAllowance = sa.CarAllowance,
                                               CcCharge = sa.CcCharge,
                                               LunchDeduction = sa.LunchDeduction,
                                               LoanRepayment = sa.LoanRepayment,
                                               LastMonthLoanPayment = sa.LastMonthLoanPayment,
                                               PF = sa.PF,
                                               TotalDeductions = sa.TotalDeductions,
                                               NetSalary = sa.NetSalary,
                                               HouseRent = sa.HouseRent,
                                               PerformanceBonus = sa.PerformanceBonus,
                                               FestivalBonus = sa.FestivalBonus,
                                               IncomeTax = sa.IncomeTax,
                                               Grade = e.Grade ?? 0
                                           }).ToListAsync();

        if (!employeeSalaryDetails.Any())
        {
            return NotFound(new { message = "No records found for the provided SalaryCode." });
        }

        return Ok(employeeSalaryDetails);
    }


    [HttpGet]
    [Route("salaryHeaderDDL")]
    public async Task<ActionResult> GetSalaryHeaderDDL()
    {
        var salaryHeaders = await _context.SalaryHeader
        .Select(sh => new SalaryHeaderDTO
        {
            value = sh.Id,
            label = sh.SalaryCode
        }).ToListAsync();
        return Ok(salaryHeaders);
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

    [HttpPost]
    [Route("loanCreate")]
    public async Task<ActionResult> CreateLoan([FromBody] LoanPayload payload)
    {
        var loan = new Loan
        {
            EmployeeId = payload.EmployeeId,
            LoanAmount = payload.LoanAmount,
            LoanDate = payload.LoanDate,
            LoanType = payload.LoanType,
            Installment = payload.Installment,
            InstallmentAmount = payload.InstallmentAmount,
            Description = payload.Description,
            Status = "Active"
        };
        await _context.Loan.AddAsync(loan);
        await _context.SaveChangesAsync();
        return Ok(loan);
    }

    [HttpGet]
    [Route("loanLanding")]
    public async Task<ActionResult> GetLoanList(int? EmployeeId)
    {
        var loanList = await (from l in _context.Loan
                              join e in _context.Employee on l.EmployeeId equals e.Id
                              where EmployeeId == null || EmployeeId == 0 || l.EmployeeId == EmployeeId
                              select new LoanDTO
                              {
                                  LoanId = l.Id,
                                  EmployeeId = l.EmployeeId,
                                  EmployeeName = e.FirstName + " " + e.LastName,
                                  LoanAmount = l.LoanAmount,
                                  LoanDate = l.LoanDate,
                                  LoanType = l.LoanType,
                                  Installment = l.Installment,
                                  InstallmentAmount = l.InstallmentAmount,
                                  Description = l.Description,

                              }).ToListAsync();
        return Ok(loanList);
    }
    [HttpPut]
    [Route("loanDelete")]
    public async Task<ActionResult> DeleteLoan(int LoanId)
    {
        var loan = await _context.Loan.FindAsync(LoanId);
        if (loan == null)
        {
            return NotFound();
        }
        _context.Loan.Remove(loan);
        await _context.SaveChangesAsync();
        return Ok(loan);
    }

}