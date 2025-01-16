using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    // Add DbSet properties for your tables
    public DbSet<Employee> Employee { get; set; }
    public DbSet<Designation> Designation { get; set; }
    public DbSet<Department> Department { get; set; }
    public DbSet<EmployeeSalary> EmployeeSalary { get; set; }
    public DbSet<SalaryAssign> SalaryAssign { get; set; }
}