using EmployeeManagement.Core.Entities;
using EmployeeManagement.Infrastructure.Data;

namespace EmployeeManagement.WebAPI;

public static class DataSeeder
{
    public static async Task SeedAsync(EmployeeContext context)
    {
        if (!context.Users.Any())
        {
            context.Users.Add(new User
            {
                Username = "admin",
                Password = BCrypt.Net.BCrypt.HashPassword("admin"),
                Role = "admin"
            });

            await context.SaveChangesAsync();
        }

        if (!context.Departments.Any())
        {
            var department = new Department
            {
                Name = "IT"
            };

            var department2 = new Department
            {
                Name = "Marketing"
            };

            context.Departments.Add(department);
            context.Departments.Add(department2);
            await context.SaveChangesAsync();

            var employee = new Employee
            {
                FirstName = "John",
                LastName = "Doe",
                DepartmentId = department.DepartmentId,
                HireDate = DateTime.UtcNow,
                Phone = "123-456-7890",
                Address = "123 Main St",
                IsActive = true
            };

            var employee2 = new Employee
            {
                FirstName = "NextJohn",
                LastName = "NextDoe",
                DepartmentId = department2.DepartmentId,
                HireDate = DateTime.UtcNow,
                Phone = "123-456-7890",
                Address = "123 Main St",
                IsActive = true
            };

            context.Employees.Add(employee);
            context.Employees.Add(employee2);
            await context.SaveChangesAsync();

            var departmentHistory = new DepartmentHistory
            {
                EmployeeId = employee.EmployeeId,
                DepartmentId = department.DepartmentId,
                StartDate = DateTime.UtcNow
            };

            context.DepartmentHistories.Add(departmentHistory);
            await context.SaveChangesAsync();
        }
    }
}