using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace HRDemoAPICore.Models
{
    public class EmployeeRequest
    {
        [JsonProperty("firstName"), Required]
        public string? FirstName { get; set; }
        [JsonProperty("lastName"), Required]
        public string? LastName { get; set; }
        [JsonProperty("email"), EmailAddress, Required]
        public string? Email { get; set; }
        [JsonProperty("phone"), Phone, Required]
        public string? Phone { get; set; }
        [JsonProperty("jobTitle"), Required]
        public string? JobTitle { get; set; }

        [JsonProperty("salary"), Required]
        public double Salary { get; set; }
        [JsonProperty("departmentId")]
        public int DepartmentID { get; set; }
        [JsonProperty("address")]
        public EmployeeRequestAddress? Address { get; set; }
    }

    public class EmployeeRequestAddress
    {
        [JsonProperty("line1"), Required]
        public string? Line1 { get; set; }
        [JsonProperty("line2"), Required]
        public string? Line2 { get; set; }
        [JsonProperty("city"), Required]
        public string? City { get; set; }
        [JsonProperty("postalCode"), Required]
        public string? PostalCode { get; set; }
        [JsonProperty("state"), Required]
        [MaxLength(2), MinLength(2)]
        public string? State { get; set; }
        [JsonProperty("country")]
        public string? Country { get; set; } = "USA";
    }
}