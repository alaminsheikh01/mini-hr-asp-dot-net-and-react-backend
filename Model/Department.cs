using System.Collections.Generic;
using System.Text.Json.Serialization;

public class Department {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public bool IsActive { get; set; }
    [JsonIgnore]
    public ICollection<Employee> Employees { get; set; }
}