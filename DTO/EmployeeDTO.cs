using System;

public class EmployeeDTO
{
    public int? EmployeeId { get; set; }
    public int? SalaryAssignId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? EmployeeCode { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Gender { get; set; }
    public int? Grade { get; set; }
    public string? InsuranceNumber { get; set; }
    public int? TINNumber { get; set; }
    public string? EmployeeStatus { get; set; }
    public int? DesignationId { get; set; }
    public string? DesignationName { get; set; }
    public int? DepartmentId { get; set; }
    public string? DepartmentName { get; set; }
    public DateTime? DateOfJoining { get; set; }
    public DateTime? DateOfBirth { get; set; }

    // Additional fields based on the updated Employee class
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

    // Additional employee type field (permanent, provisional, contractual)
    public string? EmployeeType { get; set; }

    // Additional employee salary grade field
    public string? EmployeeSalaryGrade { get; set; }

    // Additional status options (active, inactive, leave without pay)
    public bool? Status { get; set; }

    // Salary-related fields
    public decimal? GrossSalary { get; set; }
    public decimal? BasicSalary { get; set; }
}
