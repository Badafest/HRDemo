using System;

namespace HRDemoAdmin.Services.Models
{
    public class LeaveResponse
    {
        public int LeaveID { get; set; }
        public LeaveType Type { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public EmployeeResponse Employee { get; set; }
    }
}