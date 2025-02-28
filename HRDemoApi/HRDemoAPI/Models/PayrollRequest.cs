using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace HRDemoAPI.Models
{
    public class PayrollRequest
    {
        [JsonProperty("month"), Required]
        public short Month { get; set; }
        [JsonProperty("year"), Required]
        public short Year { get; set; }
        [JsonProperty("employeeId")]
        public int EmployeeID { get; set; }
        [JsonProperty("salary")]
        public PayrollRequestSalary Salary { get; set; }
    }
    public class PayrollRequestSalary
    {
        [JsonProperty("grossAmount")]
        public double GrossAmount { get; set; }
        [JsonProperty("preTaxDeduction")]
        public double PreTaxDeduction { get; set; }
        [JsonProperty("taxDeduction")]
        public double TaxDeduction { get; set; }
        [JsonProperty("postTaxDeduction")]
        public double PostTaxDeduction { get; set; }
    }
}