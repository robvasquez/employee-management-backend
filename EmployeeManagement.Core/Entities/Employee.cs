namespace EmployeeManagement.Core.Entities
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime HireDate { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public ICollection<DepartmentHistory> DepartmentHistories { get; set; } = new List<DepartmentHistory>();
    }
    
    public class EmployeeUpdateDTO
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime HireDate { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }
        public int DepartmentId { get; set; }
        public DepartmentDTO Department { get; set; }
        public ICollection<DepartmentHistoryDTO> DepartmentHistories { get; set; }
    }
}
