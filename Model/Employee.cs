using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class Employee
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? EmployeeCode { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public string? Gender { get; set; }
    public int? Grade { get; set; }
    public string? InsuranceNumber { get; set; }
    public int? TINNumber { get; set; }
    public string? EmployeeStatus { get; set; }
    public DateTime? DateOfJoining { get; set; }
    public DateTime? DateOfBirth { get; set; }

    [ForeignKey("Designation")]
    public int? DesignationId { get; set; }
    public string? DesignationName { get; set; }
    [ForeignKey("Department")]
    public int? DepartmentId { get; set; }
    public string? DepartmentName { get; set; }
    [JsonIgnore]
    public Designation Designation { get; set; }
    [JsonIgnore]
    public Department Department { get; set; }

}
