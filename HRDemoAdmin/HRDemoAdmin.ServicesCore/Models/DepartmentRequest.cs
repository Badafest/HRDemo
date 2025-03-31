namespace HRDemoAdmin.Services.Models
{
    public class DepartmentRequest
    {
        public string name { get; set; } = "";
        public string description { get; set; } = "";
        public int managerId { get; set; }
    }
}