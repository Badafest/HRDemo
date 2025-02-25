namespace HRDemoAdmin.Services.Models
{
    public class PayrollResponse
    {
        public int PayrollID { get; set; }
        public short Month { get; set; }
        public short Year { get; set; }
        public string Status { get; set; }
        public PayrollResponseSalary Salary { get; set; }
        public EmployeeResponse Employee { get; set; }
    }

    public class PayrollResponseSalary
    {
        public double GrossAmount { get; set; }
        public double PreTaxDeduction { get; set; }
        public double TaxDeduction { get; set; }
        public double PostTaxDeduction { get; set; }
        public double NetAmount { get; set; }
    }
}