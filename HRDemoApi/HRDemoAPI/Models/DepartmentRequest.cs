using Newtonsoft.Json;

namespace HRDemoApp.Models
{
    public class DepartmentRequest
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("managerId")]
        public int ManagerID { get; set; }
    }
}