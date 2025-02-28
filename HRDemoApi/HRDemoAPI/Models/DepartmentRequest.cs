using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace HRDemoAPI.Models
{
    public class DepartmentRequest
    {
        [JsonProperty("name"), Required]
        public string Name { get; set; }
        [JsonProperty("description"), Required]
        public string Description { get; set; }
        [JsonProperty("managerId")]
        public int ManagerID { get; set; }
    }
}