using DataAnnotationsExtensions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HRDemoAdmin.Services.Models
{
    public enum LeaveType : short
    {
        Sick = 0,
        Casual = 1,
        Parental = 2,
        Bereavement = 3,
        Unpaid = 4
    }
    public class LeaveRequest
    {
        [DisplayName("Leave Type")]
        public LeaveType type { get; set; }
        [DisplayName("Start Date")]
        public System.DateTimeOffset startDate { get; set; }
        [DisplayName("End Date")]
        public System.DateTimeOffset endDate { get; set; }
        [Required]
        public string reason { get; set; }
        [DisplayName("Employee Email"), Email]
        public string employeeEmail { get; set; }
        public int employeeId { get; set; }
    }
}