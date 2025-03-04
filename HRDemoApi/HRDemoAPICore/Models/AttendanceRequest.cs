using Newtonsoft.Json;

namespace HRDemoAPICore.Models
{
    public class AttendanceRequest
    {
        [JsonProperty("date")]
        public DateTimeOffset Date { get; set; } = DateTimeOffset.MinValue;
        [JsonProperty("checkIn")]
        public DateTimeOffset CheckInTime { get; set; } = DateTimeOffset.MinValue;
        [JsonProperty("checkOut")]
        public DateTimeOffset CheckOutTime { get; set; } = DateTimeOffset.MinValue;
        [JsonProperty("employeeId")]
        public int EmployeeID { get; set; }
    }
}