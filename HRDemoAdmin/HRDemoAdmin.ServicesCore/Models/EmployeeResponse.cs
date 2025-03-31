using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HRDemoAdmin.Services.Models
{
    public class EmployeeResponse: ManagerResponse
    {
        public int EmployeeID { get; set; }
        [DisplayName("Annual Salary (USD)")]
        public double Salary { get; set; }
        public DepartmentResponse Department { get; set; }
    }

    public class ManagerResponse
    {
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Phone { get; set; }
        [DisplayName("Job Title")]
        public string JobTitle { get; set; }
        public string Status { get; set; }
        public EmployeeResponseAddress Address { get; set; }
    }

    public class EmployeeResponseAddress
    {
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }
}