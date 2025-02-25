public class SalaryHeaderDataDTO
{
    public int SalaryHeaderId { get; set; }
    public string SalaryCode { get; set; }
    public string SalaryMonth { get; set; }
    public int SalaryYear { get; set; }
    public decimal BasicSalary { get; set; }
    public decimal GrossSalary { get; set; }
    public decimal TotalDeductions { get; set; }
    public decimal NetSalary { get; set; }
    public string EmployeeName { get; set; }
    public string DepartmentName { get; set; }
    public string DesignationName { get; set; }
}