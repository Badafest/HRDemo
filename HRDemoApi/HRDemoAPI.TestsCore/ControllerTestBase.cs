using HRDemoAPI.DataCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HRDemoAPI.TestsCore
{
    [TestClass]
    public class ControllerTestBase
    {
        public HRDemoApiContext _dbContext = null!;

        [TestMethod]
        public void TestDbEntities()
        {
            Assert.AreNotEqual(0, _dbContext.Employees.Count());
            Assert.AreNotEqual(0, _dbContext.Departments.Count());
        }

        [TestInitialize]
        public async Task Initialize()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.Test.json")
                .AddJsonFile("appsettings.Local.json", optional: true)
                .Build();
            var dbContextOptions = new DbContextOptionsBuilder<HRDemoApiContext>()
                .UseSqlServer(configuration.GetConnectionString("HRDemoApiDbTest"))
                .Options;
            _dbContext = new HRDemoApiContext(dbContextOptions);

            // Ensure the database is created
            _dbContext.Database.EnsureCreated();

            // Ensure the database is seeded
            _dbContext.Departments.AddRange(
                new Department { 
                    Name = "Engineering", 
                    Description = "This is the Engineering Department" 
                },
                new Department { 
                    Name = "Human Resources", 
                    Description = "This is the HR Department" 
                }
            );

            await _dbContext.SaveChangesAsync();

            _dbContext.Employees.AddRange(
                new Employee { 
                    FirstName = "John", 
                    LastName = "Smith", 
                    Email = "john@mail.com",
                    Phone = "1234567890",
                    JobTitle = "Software Developer",
                    DateOfHire = DateTimeOffset.Now,
                    Status = EmployeeStatus.Active,
                    Address = new EmployeeAddress
                    {
                        Line1 = "123 Main St",
                        Line2 = "",
                        City = "Anytown",
                        State = "NY",
                        PostalCode = "12345",
                        Country = "USA"
                    },
                    DepartmentID = 1 
                },
                new Employee { 
                    FirstName = "Jane", 
                    LastName = "Smith",
                    Email = "jane@mail.com",
                    Phone = "1234567890",
                    JobTitle = "HR Manager",
                    DateOfHire = DateTimeOffset.Now,
                    Status = EmployeeStatus.Active,
                    Address = new EmployeeAddress
                    {
                        Line1 = "456 Main St",
                        Line2 = "Sub St",
                        City = "Anytown",
                        State = "AZ",
                        PostalCode = "67890",
                        Country = "USA"
                    },
                    DepartmentID = 2 
                }
            );

            await _dbContext.SaveChangesAsync();
        }

        [TestCleanup]
        public void Cleanup() { 
            // Disable foreign key constraints
            _dbContext.Database.ExecuteSqlRaw("EXEC sp_MSforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'");

            // Delete all records from each table
            foreach (var entity in _dbContext.Model.GetEntityTypes())
            {
                var tableName = entity.GetTableName();
                if (tableName != null)
                {
                    _dbContext.Database.ExecuteSqlRaw($"DELETE FROM {tableName}");
                    _dbContext.Database.ExecuteSqlRaw($"DBCC CHECKIDENT ('{tableName}', RESEED, 0)");
                }
            }

            // Re-enable foreign key constraints
            _dbContext.Database.ExecuteSqlRaw("EXEC sp_MSforeachtable 'ALTER TABLE ? CHECK CONSTRAINT ALL'");

            _dbContext.SaveChanges();
        }
    }
}

