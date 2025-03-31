using HRDemoAdmin.Services.Models;
using System.Collections.Generic;
using System.Linq;

namespace HRDemoAdmin.Services
{
    public class DepartmentService : ServiceBase
    {
        public DepartmentService(string baseUrl) : base(baseUrl)
        { 
        }
        public ApiResponse<IEnumerable<DepartmentResponse>> GetDepartments(string name = "")
        {
            return Get<IEnumerable<DepartmentResponse>>($"/departments", new { name });
        }
        public ApiResponse<DepartmentResponse> GetDepartmentDetails(int id)
        {
            return Get<DepartmentResponse>($"/departments/{id}");
        }
        public ApiResponse<DepartmentResponse> DeleteDepartment(int id)
        {
            return Delete<DepartmentResponse>($"/departments/{id}");
        }
        public ApiResponse<DepartmentResponse> CreateDepartment(DepartmentRequest departmentRequest)
        {
            return Post<DepartmentResponse>("/departments", departmentRequest);
        }
        public ApiResponse<DepartmentResponse> EditDepartment(int id, DepartmentRequest departmentRequest)
        {
            return Put<DepartmentResponse>($"/departments/{id}", departmentRequest);
        }
        public EmployeeResponse GetManagerByEmail(string email) 
        {
            return Get<List<EmployeeResponse>>($"/employees", new { email, count = 1}).Data?.FirstOrDefault();
        }
    }
}
