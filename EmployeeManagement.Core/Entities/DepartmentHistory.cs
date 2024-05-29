namespace EmployeeManagement.Core.Entities
{
    public class DepartmentHistory
    {
        public int DepartmentHistoryId { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public DateTime StartDate { get; set; }
    }
    
    public class DepartmentHistoryDTO
    {
        public int DepartmentHistoryId { get; set; }
        public int EmployeeId { get; set; }
        public int DepartmentId { get; set; }
        public DateTime StartDate { get; set; }
    }
}
