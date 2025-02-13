public class SalaryAssign
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public int SalaryAssignId { get; set; } 
    public decimal BasicSalary { get; set; }
    public decimal GrossSalary { get; set; }
    public decimal CarAllowance { get; set; } 
    public decimal CcCharge { get; set; } 
    public decimal MedicalAllowance { get; set; }
    public decimal Conveyance { get; set; } 
    public decimal PF { get; set; }
    public decimal LunchDeduction { get; set; }
    public decimal LoanRepayment { get; set; } 
    public decimal LastMonthLoanPayment { get; set; }
    public decimal AdvanceSalary { get; set; }
    public decimal TotalDeductions { get; set; }
    public decimal NetSalary { get; set; }
    public bool Status { get; set; }
}
