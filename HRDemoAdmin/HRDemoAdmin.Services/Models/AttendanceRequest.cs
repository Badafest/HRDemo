namespace HRDemoAdmin.Services.Models
{
    public class AttendanceRequest
    {
        public System.DateTimeOffset date { get; set; }
        public System.DateTimeOffset checkIn { get; set; }
        public System.DateTimeOffset checkOut { get; set; }
        public int employeeId { get; set; }
    }
}