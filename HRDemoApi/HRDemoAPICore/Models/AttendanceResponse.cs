using HRDemoAPI.DataCore.Models;

namespace HRDemoAPICore.Models
{
    public class AttendanceResponse
    {
        public int AttendanceID { get; set; }
        public DateTimeOffset Date { get; set; }
        public DateTimeOffset CheckInTime { get; set; }
        public DateTimeOffset CheckOutTime { get; set; }
        public AttendanceStatus Status { get; set; }
        public EmployeePublicResponse? Employee { get; set; }
    }
}