using System;
using System.ComponentModel.DataAnnotations.Schema;

public class EmployeeDTO
{
    public int? EmployeeId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }

    public int? DesignationId { get; set; }
    public string? DesignationName { get; set; }
    public int? DepartmentId { get; set; }
    public string? DepartmentName { get; set; }  
    public decimal? GrossSalary { get; set; }
    public DateTime? DateOfJoining { get; set; }

}
