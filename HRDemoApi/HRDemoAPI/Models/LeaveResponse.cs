using HRDemoAPI.Data;
using System;

namespace HRDemoApp.Models
{
    public class LeaveResponse
    {
        public int LeaveID { get; set; }
        public LeaveType Type { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public string Reason { get; set; }
        public LeaveStatus Status { get; set; }
        public EmployeePublicResponse Employee { get; set; }
    }
}