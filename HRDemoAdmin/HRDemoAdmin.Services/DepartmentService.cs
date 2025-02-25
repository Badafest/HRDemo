using HRDemoAdmin.Services.Models;
using System.Collections.Generic;

namespace HRDemoAdmin.Services
{
    public class DepartmentService : ServiceBase
    {
        public DepartmentService(string baseUrl) : base(baseUrl)
        { 
        }
        public IEnumerable<DepartmentResponse> GetDepartments(string name = "")
        {
            var details = Get<List<DepartmentResponse>>($"/departments?name={name}");
            return details;
        }
        public DepartmentResponse GetDepartmentDetails(int id)
        {
            var details = Get<DepartmentResponse>($"/departments/{id}");
            return details;
        }
        public DepartmentResponse DeleteDepartment(int id)
        {
            var details = Delete<DepartmentResponse>($"/departments/{id}");
            return details;
        }
        public DepartmentResponse CreateDepartment(DepartmentRequest departmentRequest)
        {
            var details = Post<DepartmentResponse>("/departments", departmentRequest);
            return details;
        }
        public DepartmentResponse EditDepartment(DepartmentRequest departmentRequest)
        {
            var details = Put<DepartmentResponse>("/departments", departmentRequest);
            return details;
        }
    }
}
