namespace HRDemoAdmin.Services.Models
{
    public class EmployeeReport
    {
        public int EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public double AnnualSalary { get; set; }
        public int PresentDays { get; set; }
        public int LateDays { get; set; }
        public int LeaveDays { get; set; }
        public int SickLeave { get; set; }
        public int CasualLeave { get; set; }
        public int ParentalLeave { get; set; }
        public int BereavementLeave { get; set; }
        public int UnpaidLeave { get; set; }
    }
}
