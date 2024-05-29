namespace EmployeeManagement.Core.Entities
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string Name { get; set; }
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
    
    public class DepartmentDTO
    {
        public int DepartmentId { get; set; }
        public string Name { get; set; }
    }
    
    public class AssignDepartmentDTO
    {
        public int EmployeeId { get; set; }
        public int NewDepartmentId { get; set; }
    }
}
