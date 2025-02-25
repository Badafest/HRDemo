namespace HRDemoAdmin.Services.Models
{
    public class AttendanceResponse
    {
        public int AttendanceID { get; set; }
        public System.DateTimeOffset Date { get; set; }
        public System.DateTimeOffset CheckInTime { get; set; }
        public System.DateTimeOffset CheckOutTime { get; set; }
        public string Status { get; set; }
        public EmployeeResponse Employee { get; set; }
    }
}