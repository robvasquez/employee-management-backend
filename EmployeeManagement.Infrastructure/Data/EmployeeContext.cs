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
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.ToTable("employees");
            entity.HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentId);

            entity.HasMany(e => e.DepartmentHistories)
                .WithOne(dh => dh.Employee)
                .HasForeignKey(dh => dh.EmployeeId);
        });

        modelBuilder.Entity<Department>(entity => { entity.ToTable("departments"); });

        modelBuilder.Entity<DepartmentHistory>(entity =>
        {
            entity.ToTable("department_histories");
            entity.HasOne(dh => dh.Employee)
                .WithMany(e => e.DepartmentHistories)
                .HasForeignKey(dh => dh.EmployeeId);
            entity.HasOne(dh => dh.Department)
                .WithMany()
                .HasForeignKey(dh => dh.DepartmentId);

            // Configure StartDate to use UTC
            entity.Property(dh => dh.StartDate)
                .HasColumnType("timestamp with time zone")
                .IsRequired();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Username).IsRequired();
            entity.Property(u => u.Password).IsRequired();
            entity.Property(u => u.Role).IsRequired();
        });
    }
}