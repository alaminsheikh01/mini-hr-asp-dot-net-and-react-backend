using System.Collections.Generic;

public class EmployeeSalaryPayload 
{
    public List<int> EmployeeIds { get; set; }
    public string SalaryMonth { get; set; }
    public int SalaryYear { get; set; }
    public int DepartmentId { get; set; }
    public int DesignationId { get; set; }
}
