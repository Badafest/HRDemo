using HRDemoAdmin.Services.Models;
using System.Collections.Generic;
using System.Linq;

namespace HRDemoAdmin.Services
{
    public class PayrollService : ServiceBase
    {
        public PayrollService(string baseUrl, string bearerToken = null) : base(baseUrl, bearerToken)
        { 
        }
        public ApiResponse<IEnumerable<PayrollResponse>> GetPayrolls(int employee, int year, int month)
        {
            return Get<IEnumerable<PayrollResponse>>($"/payrolls", new { employee, year, month });
        }
        public ApiResponse<PayrollResponse> GetPayrollDetails(int id)
        {
            return Get<PayrollResponse>($"/payrolls/{id}");
        }
        public ApiResponse<PayrollResponse> DeletePayroll(int id)
        {
            return Delete<PayrollResponse>($"/payrolls/{id}");
        }
        public ApiResponse<PayrollResponse> CreatePayroll(PayrollRequest payrollRequest)
        {
            return Post<PayrollResponse>("/payrolls", payrollRequest);
        }
        public ApiResponse<PayrollResponse> EditPayroll(int id, PayrollRequest payrollRequest)
        {
            return Put<PayrollResponse>($"/payrolls/{id}", payrollRequest);
        }
        public ApiResponse<PayrollResponse> PayrollStatus(int id, bool processed = true)
        {
            return Post<PayrollResponse>($"/payrollstatus/{id}?processed={processed}");
        }
        public ApiResponse<EmployeeReport> EmployeeReport(int id, int year, int month, double timezoneoffset)
        {
            return Get<EmployeeReport>($"/employeereport/{id}", new { year, month, timezoneoffset });
        }
        public EmployeeResponse GetEmployeeByEmail(string email)
        {
            return Get<List<EmployeeResponse>>($"/employees", new { email, count = 1 }).Data?.FirstOrDefault();
        }
    }
}
