

public class EmployeeSalaryDTO
{

    public int EmployeeSalaryId { get; set; }
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; }
    public string DepartmentName { get; set; }
    public string DesignationName { get; set; }
    public string SalaryMonth { get; set; }
    public decimal HouseRent { get; set; }
    public int SalaryYear { get; set; }
    public decimal PerformanceBonus { get; set; }
    public decimal FestivalBonus { get; set; }
    public decimal IncomeTax { get; set; }
    
    public decimal BasicSalary { get; set; }
    public decimal MedicalAllowance { get; set; }
    public decimal Conveyance { get; set; }
    public decimal GrossSalary { get; set; }
    public decimal AdvanceSalary { get; set; }
    public decimal TotalDeductions { get; set; }
    public decimal NetSalary { get; set; }
    public decimal CarAllowance { get; set; }
    public decimal CcCharge { get; set; }
    public decimal LunchDeduction { get; set; }
    public decimal LoanRepayment { get; set; }
    public decimal LastMonthLoanPayment { get; set; }
    public decimal PF { get; set; }
    public int? Grade { get; set; }
    public string SalaryCode { get; set; }


}