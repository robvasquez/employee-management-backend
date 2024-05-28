using EmployeeManagement.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Infrastructure.Data;

public class EmployeeContext : DbContext
{
    public EmployeeContext(DbContextOptions<EmployeeContext> options) : base(options)
    {
    }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<DepartmentHistory> DepartmentHistories { get; set; }
    public DbSet<User> Users { get; set; } // Add this line

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.ToTable("employees");
            entity.HasOne(e => e.Department)
                .WithMany()
                .HasForeignKey(e => e.DepartmentId);
        });

        modelBuilder.Entity<Department>(entity => { entity.ToTable("departments"); });

        modelBuilder.Entity<DepartmentHistory>(entity =>
        {
            entity.ToTable("department_histories");
            entity.HasOne(dh => dh.Employee)
                .WithMany()
                .HasForeignKey(dh => dh.EmployeeId);
            entity.HasOne(dh => dh.Department)
                .WithMany()
                .HasForeignKey(dh => dh.DepartmentId);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Username).IsRequired().HasMaxLength(50);
            entity.Property(u => u.Password).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Role).IsRequired().HasMaxLength(20);
        });
    }
}