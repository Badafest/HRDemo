using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HRDemoAdmin.Services.Models
{
    public class PayrollRequest
    {
        [Required]
        public short month { get; set; }
        [Required]
        public short year { get; set; }
        public int offset { get; set; }
        [EmailAddress]
        [DisplayName("Employee Email")]
        public string employeeEmail { get; set; }
        public int employeeId { get; set; }
        public PayrollRequestSalary salary { get; set; }
    }
    public class PayrollRequestSalary
    {
        [DisplayName("Gross Amount")]
        public double grossAmount { get; set; } = 0;
        [DisplayName("Pre Tax Deduction")]
        public double preTaxDeduction { get; set; } = 0;
        [DisplayName("Tax Deduction")]
        public double taxDeduction { get; set; } = 0;
        [DisplayName("Post Tax Deduction")]
        public double postTaxDeduction { get; set; } = 0;
    }
}