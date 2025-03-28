using DataAnnotationsExtensions;
using System.ComponentModel;

namespace HRDemoAdmin.Services.Models
{
    public class AttendanceRequest
    {
        public System.DateTimeOffset date { get; set; }
        [DisplayName("Check In")]
        public System.DateTimeOffset checkIn { get; set; }
        [DisplayName("Check Out")]
        public System.DateTimeOffset checkOut { get; set; }
        public int employeeId { get; set; }
        [Email, DisplayName("Employee Email")]
        public string employeeEmail { get; set; }
    }
}