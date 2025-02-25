namespace HRDemoAdmin.Services.Models
{
    public class PayrollRequest
    {
        public short month { get; set; }
        public short year { get; set; }
        public int EmployemployeeIdeeID { get; set; }
        public PayrollRequestSalary salary { get; set; }
    }
    public class PayrollRequestSalary
    {
        public double grossAmount { get; set; }
        public double preTaxDeduction { get; set; }
        public double taxDeduction { get; set; }
        public double postTaxDeduction { get; set; }
    }
}