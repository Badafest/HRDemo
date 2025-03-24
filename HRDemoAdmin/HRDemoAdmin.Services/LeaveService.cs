using HRDemoAdmin.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HRDemoAdmin.Services
{
    public class LeaveService : ServiceBase
    {
        public LeaveService(string baseUrl) : base(baseUrl)
        { 
        }
        public ApiResponse<IEnumerable<LeaveResponse>> GetLeaves(int employeeId, string type, DateTimeOffset? startDate, DateTimeOffset? endDate)
        {
            return Get<IEnumerable<LeaveResponse>>($"/leaves", new { employeeId, type, startDate, endDate });
        }
        public ApiResponse<LeaveResponse> GetLeaveDetails(int id)
        {
            return Get<LeaveResponse>($"/leaves/{id}");
        }
        public ApiResponse<LeaveResponse> DeleteLeave(int id)
        {
            return Delete<LeaveResponse>($"/leaves/{id}");
        }
        public ApiResponse<LeaveResponse> CreateLeave(LeaveRequest leaveRequest)
        {
            return Post<LeaveResponse>("/leaves", leaveRequest);
        }
        public ApiResponse<LeaveResponse> EditLeave(int id, LeaveRequest leaveRequest)
        {
            return Put<LeaveResponse>($"/leaves/{id}", leaveRequest);
        }

        public ApiResponse<LeaveResponse> LeaveStatus(int id, bool approve = true)
        {
            return Post<LeaveResponse>($"/leavestatus/{id}", new { approve });
        }
        public EmployeeResponse GetEmployeeByEmail(string email)
        {
            return Get<List<EmployeeResponse>>($"/employees", new { email, count = 1 }).Data?.FirstOrDefault();
        }
    }
}
