using HRDemoAPI.Data;
using HRDemoAPI.Models;
using System;
using System.Configuration;

namespace HRDemoAPI.Utilities
{
    public static class AttendanceMapper
    {
        public static AttendanceResponse MapQueryResult(this Attendance attendance)
        {
            return new AttendanceResponse
            {
               AttendanceID = attendance.AttendanceID,
               Date = attendance.Date,
               CheckInTime = attendance.CheckInTime,
               CheckOutTime = attendance.CheckOutTime,
               Status = attendance.Status,
               Employee = attendance.Employee?.MapQueryResult()
            };
        }
        public static Attendance MapPostRequest(this AttendanceRequest attendanceRequest)
        {
            return MapRequest(attendanceRequest);
        }

        public static Attendance MapPutRequest(this AttendanceRequest attendanceRequest, int attendanceId)
        {
            Attendance attendance = MapRequest(attendanceRequest);
            attendance.AttendanceID = attendanceId;
            return attendance;
        }

        private static Attendance MapRequest(AttendanceRequest attendanceRequest)
        {
            return new Attendance()
            {
                Date = attendanceRequest.Date,
                CheckInTime = attendanceRequest.CheckInTime,
                CheckOutTime= attendanceRequest.CheckOutTime,
                Status = GetAttendanceStatus(attendanceRequest.Date, attendanceRequest.CheckInTime, attendanceRequest.CheckOutTime),
                EmployeeID = attendanceRequest.EmployeeID,
            };
        }

        public static AttendanceStatus GetAttendanceStatus(DateTimeOffset date, DateTimeOffset checkInTime, DateTimeOffset checkOutTime)
        {
            TimeSpan duration = checkOutTime - checkInTime;
            var MinAttendanceHours = int.Parse(ConfigurationManager.AppSettings.Get("MinAttendanceHours"));

            if (duration < TimeSpan.FromHours(MinAttendanceHours))
            {
                return AttendanceStatus.Absent;
            }

            if (checkInTime.TimeOfDay > date.TimeOfDay)
            {
                return AttendanceStatus.Late;
            }
            return AttendanceStatus.Present;
        }
    }
}