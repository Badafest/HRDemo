namespace HRDemoAdmin.Services.Models
{
    public class LeaveRequest
    {
        public string type { get; set; }
        public System.DateTimeOffset startDate { get; set; }
        public System.DateTimeOffset endDate { get; set; }
        public string reason { get; set; }
        public int employeeId { get; set; }
    }
}