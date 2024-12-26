public class EmployeeSalaryPayload {
    public int EmployeeId { get; set; }
    public decimal GrossSalary { get; set; }
    public string SalaryMonth { get; set; }
    public int SalaryYear { get; set; }
    public int DepartmentId { get; set; }
    public int DesignationId { get; set; }
}