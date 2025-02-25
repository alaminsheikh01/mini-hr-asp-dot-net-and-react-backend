using System.Collections.Generic;

public class SalaryHeader
{
    public int Id { get; set; }
    public string SalaryCode { get; set; } // Unique identifier
    public string Month { get; set; }
    public int Year { get; set; }
    public int DepartmentId { get; set; }
    public int DesignationId { get; set; }

    // Navigation Property - One Header has many Salary Details
    public ICollection<EmployeeSalary> EmployeeSalaries { get; set; }
}
