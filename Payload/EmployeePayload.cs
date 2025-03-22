using System;

public class EmployeePayload
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? EmployeeCode { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public string? PresentAddress { get; set; }
    public string? PermanentAddress { get; set; }
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

    // Additional fields based on the Employee class:
    public string? EmergencyContact { get; set; } // Added field
    public string? NID { get; set; } // Added field
    public string? BloodGroup { get; set; } // Added field
    public DateTime? ConfirmationDate { get; set; } // Added field
    public DateTime? RetirementOrResignation { get; set; } // Added field
    public string? ServicePeriod { get; set; } // Added field
    public string? SalaryAccountNumber { get; set; } // Added field
    public string? ETIN { get; set; } // Added field
    public string? AcademicQualifications { get; set; } // Added field
    public bool? CertificateVerification { get; set; } // Added field
    public bool? PoliceVerification { get; set; } // Added field
    public bool? DisciplinaryAction { get; set; } // Added field
    public string? JobLocation { get; set; } // Added field
    public string? EmployeeType { get; set; } // Added field (permanent, provisional, contractual)
    public string? EmployeeSalaryGrade { get; set; } // Added field
    public string? Status { get; set; } // Added field (active, inactive, leave without pay)
}
