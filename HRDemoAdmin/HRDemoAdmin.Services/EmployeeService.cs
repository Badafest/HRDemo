using HRDemoAdmin.Services.Models;
using System.Collections.Generic;
using System.Linq;

namespace HRDemoAdmin.Services
{
    public class EmployeeService : ServiceBase
    {
        public EmployeeService(string baseUrl) : base(baseUrl)
        { 
        }
        public ApiResponse<IEnumerable<EmployeeResponse>> GetEmployees(string firstName = "", string lastName = "", string jobTitle = "", string phone = "", string email = "")
        {
            return Get<IEnumerable<EmployeeResponse>>($"/employees?firstName={firstName}&lastName={lastName}&jobTitle={jobTitle}&phone={phone}&email={email}");
        }
        public ApiResponse<EmployeeResponse> GetEmployeeDetails(int id, bool salary = false)
        {
            return Get<EmployeeResponse>($"/employees/{id}?salary={salary}");
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
        public ApiResponse<EmployeeReport> EmployeeReport(int id, int year, int month, double timezoneoffset)
        {
            return Get<EmployeeReport>($"/employeereport/{id}?year={year}&month={month}&timezoneoffset={timezoneoffset}");
        }

        public DepartmentResponse GetDepartmentByName(string name)
        {
            return Get<List<DepartmentResponse>>($"/departments?name={name}&count=1").Data?.FirstOrDefault();
        }
    }
}
