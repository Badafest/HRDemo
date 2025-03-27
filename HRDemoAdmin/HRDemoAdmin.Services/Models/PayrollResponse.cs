using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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
        [DisplayName("Gross Amount")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public double GrossAmount { get; set; }
        [DisplayName("Pre Tax Deduction")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public double PreTaxDeduction { get; set; }
        [DisplayName("Tax Deduction")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public double TaxDeduction { get; set; }
        [DisplayName("Post Tax Deduction")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public double PostTaxDeduction { get; set; }
        [DisplayName("Net Amount")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public double NetAmount { get; set; }
    }
}