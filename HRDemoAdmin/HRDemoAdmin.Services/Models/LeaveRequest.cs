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
        public LeaveType type { get; set; }
        public System.DateTimeOffset startDate { get; set; }
        public System.DateTimeOffset endDate { get; set; }
        [Required]
        public string reason { get; set; }
        public int employeeId { get; set; }
    }
}