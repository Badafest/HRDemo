namespace HRDemoAdmin.Services.Models
{
    public class DepartmentResponse
    {
        public int DepartmentID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ManagerResponse Manager { get; set; }
    }
}