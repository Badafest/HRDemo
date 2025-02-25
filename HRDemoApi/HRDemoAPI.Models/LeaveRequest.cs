using HRDemoAPI.Data;
using Newtonsoft.Json;

namespace HRDemoApp.Models
{
    public class LeaveRequest
    {
        [JsonProperty("type")]
        public LeaveType Type { get; set; }
        [JsonProperty("startDate")]
        public System.DateTimeOffset StartDate { get; set; }
        [JsonProperty("endDate")]
        public System.DateTimeOffset EndDate { get; set; }
        [JsonProperty("reason")]
        public string Reason { get; set; }

        [JsonProperty("employeeId")]
        public int EmployeeID { get; set; }
    }
}