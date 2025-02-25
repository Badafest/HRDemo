
namespace HRDemoAdmin.Services.Models
{
    public class EmployeeRequest
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string jobTitle { get; set; }
        public double salary { get; set; }
        public int departmentId { get; set; }
        public EmployeeRequestAddress address { get; set; }
    }

    public class EmployeeRequestAddress
    {
        public string line1 { get; set; }
        public string line2 { get; set; }
        public string city { get; set; }
        public string postalCode { get; set; }
        public string state { get; set; }
        public string country { get; set; }
    }
}