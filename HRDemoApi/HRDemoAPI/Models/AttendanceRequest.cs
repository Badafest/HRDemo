using Newtonsoft.Json;

namespace HRDemoAPI.Models
{
    public class AttendanceRequest
    {
        [JsonProperty("date")]
        public System.DateTimeOffset Date { get; set; }
        [JsonProperty("checkIn")]
        public System.DateTimeOffset CheckInTime { get; set; }
        [JsonProperty("checkOut")]
        public System.DateTimeOffset CheckOutTime { get; set; }
        [JsonProperty("employeeId")]
        public int EmployeeID { get; set; }
    }
}