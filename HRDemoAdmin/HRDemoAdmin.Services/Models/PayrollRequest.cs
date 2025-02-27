using System.ComponentModel.DataAnnotations;

namespace HRDemoAdmin.Services.Models
{
    public class PayrollRequest
    {
        [Required]
        public short month { get; set; }
        [Required]
        public short year { get; set; }
        public int EmployemployeeIdeeID { get; set; }
        public PayrollRequestSalary salary { get; set; }
    }
    public class PayrollRequestSalary
    {
        public double grossAmount { get; set; } = 0;
        public double preTaxDeduction { get; set; } = 0;
        public double taxDeduction { get; set; } = 0;
        public double postTaxDeduction { get; set; } = 0;
    }
}