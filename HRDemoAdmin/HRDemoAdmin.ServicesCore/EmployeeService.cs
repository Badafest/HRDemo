using HRDemoAdmin.Services.Models;
using System.Collections.Generic;
using System.Linq;

namespace HRDemoAdmin.Services
{
    public class EmployeeService : ServiceBase
    {
        public EmployeeService(string baseUrl, string bearerToken = null) : base(baseUrl, bearerToken)
        {
        }
        public ApiResponse<IEnumerable<EmployeeResponse>> GetEmployees(string firstName = "", string lastName = "", string jobTitle = "", string phone = "", string email = "")
        {
            return Get<IEnumerable<EmployeeResponse>>($"/employees", new { firstName, lastName, jobTitle, phone, email });
        }
        public ApiResponse<EmployeeResponse> GetEmployeeDetails(int id, bool salary = false)
        {
            return Get<EmployeeResponse>($"/employees/{id}", new { salary });
        }
        public ApiResponse<EmployeeResponse> DeleteEmployee(int id)
        {
            return Delete<EmployeeResponse>($"/employees/{id}");
        }
        public ApiResponse<EmployeeResponse> CreateEmployee(EmployeeRequest employeeRequest)
        {
            return Post<EmployeeResponse>("/employees", employeeRequest);
        }
        public ApiResponse<EmployeeResponse> EditEmployee(int id, EmployeeRequest employeeRequest)
        {
            return Put<EmployeeResponse>($"/employees/{id}", employeeRequest);
        }

        public ApiResponse<EmployeeResponse> EmployeeStatus(int id, bool hire = true)
        {
            return Post<EmployeeResponse>($"/employeestatus/{id}?hire={hire}");
        }

        public DepartmentResponse GetDepartmentByName(string name)
        {
            return Get<List<DepartmentResponse>>($"/departments", new { name, count = 1 }).Data?.FirstOrDefault();
        }
    }
}
