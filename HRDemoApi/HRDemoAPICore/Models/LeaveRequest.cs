using HRDemoAPI.DataCore.Models;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace HRDemoAPICore.Models
{
    public class LeaveRequest
    {
        [JsonProperty("type")]
        public LeaveType Type { get; set; }
        [JsonProperty("startDate")]
        public DateTimeOffset StartDate { get; set; } = DateTimeOffset.MinValue;
        [JsonProperty("endDate")]
        public DateTimeOffset EndDate { get; set; } = DateTimeOffset.MinValue;
        [JsonProperty("reason"), Required]
        public string? Reason { get; set; }

        [JsonProperty("employeeId")]
        public int EmployeeID { get; set; }
    }
}