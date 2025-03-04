﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using Microsoft.EntityFrameworkCore;

namespace HRDemoAPI.DataCore.Models;

public partial class HRDemoApiContext : DbContext
{
    public HRDemoApiContext()
    {
    }

    public HRDemoApiContext(DbContextOptions<HRDemoApiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Attendance> Attendances { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Leave> Leaves { get; set; }

    public virtual DbSet<Payroll> Payrolls { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Attendance>(entity =>
        {
            entity.HasIndex(e => e.EmployeeID, "IX_FK_EmployeeAttendance");

            entity.Property(e => e.AttendanceID).HasColumnName("AttendanceID");
            entity.Property(e => e.EmployeeID).HasColumnName("EmployeeID");

            entity.HasOne(d => d.Employee).WithMany(p => p.Attendances)
                .HasForeignKey(d => d.EmployeeID)
                .HasConstraintName("FK_EmployeeAttendance");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasIndex(e => e.ManagerID, "IX_FK_DepartmentManager");

            entity.Property(e => e.DepartmentID).HasColumnName("DepartmentID");
            entity.Property(e => e.Description).IsRequired();
            entity.Property(e => e.ManagerID).HasColumnName("ManagerID");
            entity.Property(e => e.Name).IsRequired();

            entity.HasOne(d => d.Manager).WithMany(p => p.Departments)
                .HasForeignKey(d => d.ManagerID)
                .HasConstraintName("FK_DepartmentManager");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasIndex(e => e.DepartmentID, "IX_FK_EmployeeDepartment");

            entity.Property(e => e.EmployeeID).HasColumnName("EmployeeID");
            entity.OwnsOne(e => e.Address, address =>
            {
                address.Property(a => a.City)
                    .IsRequired()
                    .HasColumnName("Address_City");
                address.Property(a => a.Country)
                    .IsRequired()
                    .HasColumnName("Address_Country");
                address.Property(a => a.Line1)
                    .IsRequired()
                    .HasColumnName("Address_Line1");
                address.Property(a => a.Line2)
                    .IsRequired()
                    .HasColumnName("Address_Line2");
                address.Property(a => a.PostalCode)
                    .IsRequired()
                    .HasColumnName("Address_PostalCode");
                address.Property(a => a.State)
                    .IsRequired()
                    .HasColumnName("Address_State");
            });
            entity.Property(e => e.DepartmentID).HasColumnName("DepartmentID");
            entity.Property(e => e.Email).IsRequired();
            entity.Property(e => e.FirstName).IsRequired();
            entity.Property(e => e.JobTitle).IsRequired();
            entity.Property(e => e.LastName).IsRequired();
            entity.Property(e => e.Phone).IsRequired();

            entity.HasOne(d => d.Department).WithMany(p => p.Employees)
                .HasForeignKey(d => d.DepartmentID)
                .HasConstraintName("FK_EmployeeDepartment");
        });

        modelBuilder.Entity<Leave>(entity =>
        {
            entity.HasKey(e => e.LeaveID);

            entity.HasIndex(e => e.EmployeeID, "IX_FK_EmployeeLeave");

            entity.Property(e => e.LeaveID).HasColumnName("LeaveID");
            entity.Property(e => e.EmployeeID).HasColumnName("EmployeeID");
            entity.Property(e => e.Reason).IsRequired();

            entity.HasOne(d => d.Employee).WithMany(p => p.Leaves)
                .HasForeignKey(d => d.EmployeeID)
                .HasConstraintName("FK_EmployeeLeave");
        });

        modelBuilder.Entity<Payroll>(entity =>
        {
            entity.HasIndex(e => e.EmployeeID, "IX_FK_EmployeePayroll");

            entity.Property(e => e.PayrollID).HasColumnName("PayrollID");
            entity.Property(e => e.EmployeeID).HasColumnName("EmployeeID");
            
            entity.OwnsOne(e => e.Salary, salary =>
            {
                salary.Property(s => s.GrossAmount).HasColumnName("Salary_GrossAmount");
                salary.Property(s => s.NetAmount).HasColumnName("Salary_NetAmount");
                salary.Property(s => s.PostTaxDeduction).HasColumnName("Salary_PostTaxDeduction");
                salary.Property(s => s.PreTaxDeduction).HasColumnName("Salary_PreTaxDeduction");
                salary.Property(s => s.TaxDeduction).HasColumnName("Salary_TaxDeduction");
            });

            entity.HasOne(d => d.Employee).WithMany(p => p.Payrolls)
                .HasForeignKey(d => d.EmployeeID)
                .HasConstraintName("FK_EmployeePayroll");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.EmployeeID, "IX_FK_UserEmployee");

            entity.Property(e => e.UserID).HasColumnName("UserID");
            entity.Property(e => e.EmployeeID).HasColumnName("EmployeeID");
            entity.Property(e => e.Name).IsRequired();

            entity.HasOne(d => d.Employee).WithMany(p => p.Users)
                .HasForeignKey(d => d.EmployeeID)
                .HasConstraintName("FK_UserEmployee");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}