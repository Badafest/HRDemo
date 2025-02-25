using HRDemoAPI.Data;

namespace HRDemoApp.Models
{
    public class EmployeeResponse: EmployeePublicResponse
    {
        public double Salary { get; set; }
    }

    public class EmployeePublicResponse: ManagerResponse
    {
        public int EmployeeID { get; set; }
        public DepartmentResponse Department { get; set; }
    }

    public class ManagerResponse
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string JobTitle { get; set; }
        public EmployeeStatus Status { get; set; }
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