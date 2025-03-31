using HRDemoAdmin.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HRDemoAdmin.Services
{
    public class AttendanceService : ServiceBase
    {
        public AttendanceService(string baseUrl, string bearerToken = null) : base(baseUrl, bearerToken)
        { 
        }
        public ApiResponse<IEnumerable<AttendanceResponse>> GetAttendances(int employeeId, DateTimeOffset? startDate, DateTimeOffset? endDate)
        {
            return Get<IEnumerable<AttendanceResponse>>($"/attendances", new { employeeId, startDate, endDate });
        }
        public ApiResponse<AttendanceResponse> GetAttendanceDetails(int id)
        {
            return Get<AttendanceResponse>($"/attendances/{id}");
        }
        public ApiResponse<AttendanceResponse> DeleteAttendance(int id)
        {
            return Delete<AttendanceResponse>($"/attendances/{id}");
        }
        public ApiResponse<AttendanceResponse> CreateAttendance(AttendanceRequest attendanceRequest)
        {
            return Post<AttendanceResponse>("/attendances", attendanceRequest);
        }
        public ApiResponse<AttendanceResponse> EditAttendance(int id, AttendanceRequest attendanceRequest)
        {
            return Put<AttendanceResponse>($"/attendances/{id}", attendanceRequest);
        }
        public EmployeeResponse GetEmployeeByEmail(string email)
        {
            return Get<List<EmployeeResponse>>($"/employees", new { email, count = 1 }).Data?.FirstOrDefault();
        }
    }
}
