using HRDemoAPI.Data;
using HRDemoApp.Models;

namespace HRDemoApp.Utilities
{
    public static class LeaveMapper
    {
        public static LeaveResponse MapQueryResult(this Leave leave)
        {
            return new LeaveResponse
            {
               LeaveID = leave.LeaveID,
               Type = leave.Type,
               StartDate = leave.StartDate,
               EndDate = leave.EndDate,
               Status = leave.Status,
               Reason = leave.Reason,
               Employee = leave.Employee?.MapQueryResult()
            };
        }
        public static Leave MapPostRequest(this LeaveRequest leaveRequest)
        {
            var leave = MapRequest(leaveRequest);
            leave.Status = LeaveStatus.Pending;
            return leave;
        }

        public static Leave MapPutRequest(this LeaveRequest leaveRequest, int leaveId)
        {
            Leave leave = MapRequest(leaveRequest);
            leave.LeaveID = leaveId;
            return leave;
        }

        private static Leave MapRequest(LeaveRequest leaveRequest)
        {
            return new Leave()
            {
                Type = leaveRequest.Type,
                StartDate = leaveRequest.StartDate,
                EndDate = leaveRequest.EndDate,
                Reason = leaveRequest.Reason,
                EmployeeID = leaveRequest.EmployeeID,
            };
        }
    }
}