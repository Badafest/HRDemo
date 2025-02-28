using HRDemoAPI.Data;

namespace HRDemoAPI.Models
{
    public class AttendanceResponse
    {
        public int AttendanceID { get; set; }
        public System.DateTimeOffset Date { get; set; }
        public System.DateTimeOffset CheckInTime { get; set; }
        public System.DateTimeOffset CheckOutTime { get; set; }
        public AttendanceStatus Status { get; set; }
        public EmployeePublicResponse Employee { get; set; }
    }
}