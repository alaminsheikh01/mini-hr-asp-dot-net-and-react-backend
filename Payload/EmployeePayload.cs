using System;

public class EmployeePayload
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email {get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public int? DesignationId { get; set; }
    public int? DepartmentId { get; set; }   
    public DateTime? DateOfJoining { get; set; }
}
