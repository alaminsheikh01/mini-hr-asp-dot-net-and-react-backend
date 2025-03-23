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

    // Additional fields
    public string? EmergencyContact { get; set; }
    public string? NID { get; set; }
    public string? PresentAddress { get; set; }
    public string? PermanentAddress { get; set; }
    public string? BloodGroup { get; set; }
    public DateTime? ConfirmationDate { get; set; }
    public DateTime? RetirementOrResignation { get; set; }
    public string? ServicePeriod { get; set; }
    public string? SalaryAccountNumber { get; set; }
    public string? ETIN { get; set; }
    public string? AcademicQualifications { get; set; }
    public bool? CertificateVerification { get; set; }
    public bool? PoliceVerification { get; set; }
    public bool? DisciplinaryAction { get; set; }
    public string? JobLocation { get; set; }

    // Foreign key references
    [ForeignKey("Designation")]
    public int? DesignationId { get; set; }
    public string? DesignationName { get; set; }
    [ForeignKey("Department")]
    public int? DepartmentId { get; set; }
    public string? DepartmentName { get; set; }

    // Navigation properties
    [JsonIgnore]
    public Designation Designation { get; set; }
    [JsonIgnore]
    public Department Department { get; set; }

    // Additional employee type field (permanent, provisional, contractual)
    public string? EmployeeType { get; set; }

    // Additional employee salary grade field
    public string? EmployeeSalaryGrade { get; set; }

    // Additional status options (active, inactive, leave without pay)
    public bool? Status { get; set; }
}
