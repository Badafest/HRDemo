using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HRDemoAdmin.Services.Models
{
    public class LeaveResponse
    {
        public int LeaveID { get; set; }
        public LeaveType Type { get; set; }
        [DisplayName("Start Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTimeOffset StartDate { get; set; }
        [DisplayName("End Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTimeOffset EndDate { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public EmployeeResponse Employee { get; set; }
    }
}