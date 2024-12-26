using System.Collections.Generic;

public class Department {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public bool IsActive { get; set; }
    public ICollection<Employee> Employees { get; set; }
}