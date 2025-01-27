using System;

public class LoanDTO {
      public int LoanId { get; set; }
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; }
    public decimal LoanAmount { get; set; }
    public DateTime LoanDate { get; set; }
    public string LoanType { get; set; }
    public int Installment { get; set; }
    public decimal InstallmentAmount { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
}