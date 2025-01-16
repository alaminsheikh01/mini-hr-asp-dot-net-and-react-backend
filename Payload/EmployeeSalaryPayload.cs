public class EmployeeSalaryPayload {
    public int EmployeeId { get; set; }
    public decimal GrossSalary { get; set; }
    public decimal BasicSalary { get; set; }
    public decimal HouseRent { get; set; }
    public decimal MedicalAllowance { get; set; }
    public decimal Conveyance { get; set; }
    public string SalaryMonth { get; set; }
    public int SalaryYear { get; set; }
    public int DepartmentId { get; set; }
    public int DesignationId { get; set; }
}