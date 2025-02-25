using HRDemoAPI.Data;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace HRDemoApp.Models
{
    public class EmployeeRequest
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set; }
        [JsonProperty("lastName")]
        public string LastName { get; set; }
        [JsonProperty("email"), EmailAddress]
        public string Email { get; set; }
        [JsonProperty("phone"), Phone]
        public string Phone { get; set; }
        [JsonProperty("jobTitle")]
        public string JobTitle { get; set; }

        [JsonProperty("salary")]
        public double Salary { get; set; }
        [JsonProperty("departmentId")]
        public int DepartmentID { get; set; }
        [JsonProperty("address")]
        public EmployeeRequestAddress Address { get; set; }
    }

    public class EmployeeRequestAddress
    {
        [JsonProperty("line1")]
        public string Line1 { get; set; }
        [JsonProperty("line2")]
        public string Line2 { get; set; }
        [JsonProperty("city")]
        public string City { get; set; }
        [JsonProperty("postalCode")]
        public string PostalCode { get; set; }
        [JsonProperty("state")]
        [MaxLength(2), MinLength(2)]
        public string State { get; set; }
        [JsonProperty("country")]
        public string Country { get; set; } = "USA";
    }
}