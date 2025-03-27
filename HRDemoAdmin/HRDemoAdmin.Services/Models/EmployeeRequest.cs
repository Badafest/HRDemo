
using DataAnnotationsExtensions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HRDemoAdmin.Services.Models
{
    public class EmployeeRequest
    {
        [DisplayName("First Name"), Required]
        public string firstName { get; set; }
        [DisplayName("Last Name"), Required]
        public string lastName { get; set; }
        [Email, Required]
        public string email { get; set; }
        [Phone, Required]
        public string phone { get; set; }
        [DisplayName("Job Title"), Required]
        public string jobTitle { get; set; }
        [DisplayName("Annual Salary (USD)"), Required]
        public double salary { get; set; }
        public int departmentId { get; set; }
        [DisplayName("Department Name"), Required]
        public string departmentName { get; set; }
        public EmployeeRequestAddress address { get; set; }
    }

    public class EmployeeRequestAddress
    {
        [DisplayName("Line 1"), Required]
        public string line1 { get; set; }
        [DisplayName("Line 2")]
        public string line2 { get; set; } = "";
        [Required]
        public string city { get; set; }
        [DisplayName("Postal Code"), Required]
        public string postalCode { get; set; }
        [MinLength(2), MaxLength(2), Required]
        public string state { get; set; }
        public string country { get; set; } = "USA";
    }
}