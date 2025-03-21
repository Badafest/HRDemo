using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace HRDemoReportService.DataCore
{
    public class HRDemoReportDataService(string connectionString, int commandTimeout)
    {
        private readonly string _connectionString = connectionString;
        private readonly int _commandTimeout = commandTimeout;

        public UserResponse GetUserDetails(string username)
        {
            var response = new UserResponse();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using var command = connection.CreateCommand();
                command.CommandText = @"
                        SELECT
                            Users.UserID,
                            Users.EmployeeID, 
                            Users.Role as UserRole,
                            CONCAT(Employees.FirstName, ' ', Employees.LastName) as EmployeeName,
                            Departments.DepartmentID,
                            Departments.Name as DepartmentName,
	                        Departments.ManagerID
                        FROM Users
                        JOIN Employees ON Employees.EmployeeID = Users.EmployeeID
                        JOIN Departments ON Departments.ManagerID = Users.EmployeeID OR Users.Role = 1
                        WHERE Users.Name = @username";
                command.Parameters.Add(new SqlParameter("@username", SqlDbType.NVarChar) { Value = username });
                command.CommandTimeout = _commandTimeout;

                using var reader = command.ExecuteReader();
                var managedDepartments = new List<DepartmentResponse>();
                int rowCount = 0;
                while (reader.Read())
                {
                    if (rowCount == 0)
                    {
                        response.UserID = reader.GetInt32(0);
                        response.EmployeeID = reader.GetInt32(1);
                        response.UserRole = reader.GetInt16(2);
                        response.EmployeeName = reader.GetString(3);
                    }
                    managedDepartments.Add(new DepartmentResponse
                    {
                        DepartmentID = reader.GetInt32(4),
                        DepartmentName = reader.GetString(5)
                    });
                    rowCount++;
                }
                response.ManagedDepartments = managedDepartments.ToArray();
            }
            return response;
        }
        public ReportResponse GetReportData(ReportRequest request)
        {
            var response = new ReportResponse();

            response.EmployeeReport.WorkingDaysInMonth = ComputeWorkingDaysInMonth(request.Year, request.Month);
            response.EmployeeReport.WorkingDaysInYear = ComputeWorkingDaysInYear(request.Year);

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // employee report
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "dbo.GetEmployeeMonthlyReport";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@employeeId", SqlDbType.Int) { Value = request.EmployeeID });
                    command.Parameters.Add(new SqlParameter("@year", SqlDbType.SmallInt) { Value = request.Year });
                    command.Parameters.Add(new SqlParameter("@month", SqlDbType.SmallInt) { Value = request.Month });
                    command.Parameters.Add(new SqlParameter("@offset", SqlDbType.Float) { Value = request.TimezoneOffset });
                    command.CommandTimeout = _commandTimeout;

                    using var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        response.EmployeeReport.EmployeeID = reader.GetInt32(0);
                        response.EmployeeReport.FirstName = reader.GetString(1);
                        response.EmployeeReport.LastName = reader.GetString(2);
                        response.EmployeeReport.State = reader.GetString(3);
                        response.EmployeeReport.Country = reader.GetString(4);
                        response.EmployeeReport.AnnualSalary = reader.GetDouble(5);
                        response.EmployeeReport.PresentDays = reader.GetInt32(6);
                        response.EmployeeReport.LateDays = reader.GetInt32(7);
                        response.EmployeeReport.LeaveDays = reader.GetInt32(8);
                        response.EmployeeReport.SickLeave = reader.GetInt32(9);
                        response.EmployeeReport.CasualLeave = reader.GetInt32(10);
                        response.EmployeeReport.ParentalLeave = reader.GetInt32(11);
                        response.EmployeeReport.BereavementLeave = reader.GetInt32(12);
                        response.EmployeeReport.UnpaidLeave = reader.GetInt32(13);
                    }
                }
                response.EmployeeReport.MonthSalary = Math.Round(response.EmployeeReport.WorkingDaysInMonth * (response.EmployeeReport.AnnualSalary / response.EmployeeReport.WorkingDaysInYear), 2);
                response.EmployeeReport.AbsentDays = response.EmployeeReport.WorkingDaysInMonth - response.EmployeeReport.PresentDays - response.EmployeeReport.LateDays - response.EmployeeReport.LeaveDays;
                // payrolls
                using (var query = connection.CreateCommand())
                {
                    query.CommandText = "SELECT PayrollID, Month, Year, Salary_GrossAmount, Salary_PreTaxDeduction, Salary_TaxDeduction, Salary_PostTaxDeduction, Salary_NetAmount FROM dbo.Payrolls WHERE EmployeeID = @employeeId AND Year = @year AND Month = @month";
                    query.Parameters.Add(new SqlParameter("@employeeId", SqlDbType.Int) { Value = request.EmployeeID });
                    query.Parameters.Add(new SqlParameter("@year", SqlDbType.SmallInt) { Value = request.Year });
                    query.Parameters.Add(new SqlParameter("@month", SqlDbType.SmallInt) { Value = request.Month });
                    query.CommandTimeout = _commandTimeout;

                    using var reader = query.ExecuteReader();
                    var payrolls = new List<PayrollReport>();
                    while (reader.Read())
                    {
                        payrolls.Add(new PayrollReport
                        {
                            PayrollID = reader.GetInt32(0),
                            Month = reader.GetInt16(1),
                            Year = reader.GetInt16(2),
                            GrossAmount = reader.GetDouble(3),
                            PreTaxDeduction = reader.GetDouble(4),
                            TaxDeduction = reader.GetDouble(5),
                            PostTaxDeduction = reader.GetDouble(6),
                            NetAmount = reader.GetDouble(7),
                        });
                    }
                    response.PayrollReports = payrolls;
                }
            }
            return response;
        }
        public EmployeeResponse[] GetEmployeeOptions(EmployeeRequest request)
        {
            var response = Array.Empty<EmployeeResponse>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using var command = connection.CreateCommand();
                command.CommandText = @"
                        SELECT TOP 5
	                        EmployeeID,
	                        CONCAT(FirstName, ' ', LastName, ' (', Email, ')') as EmployeeName
                        FROM Employees
                        WHERE 
                            DepartmentID IN (SELECT VALUE FROM string_split(@departmentIds, ','))
                            AND 
                            (@searchString = '' OR (FirstName LIKE '%' + @searchString + '%' OR LastName LIKE '%' + @searchString + '%'));";

                command.Parameters.Add(new SqlParameter("@searchString", SqlDbType.NVarChar) { Value = request.SearchName });
                command.Parameters.Add(new SqlParameter("@departmentIds", SqlDbType.NVarChar) { Value = string.Join(",", request.ManagedDepartments) });
                command.CommandTimeout = _commandTimeout;

                using var reader = command.ExecuteReader();
                var employeeOptions = new List<EmployeeResponse>();
                while (reader.Read())
                {
                    employeeOptions.Add(new EmployeeResponse
                    {
                        EmployeeID = reader.GetInt32(0),
                        EmployeeName = reader.GetString(1)
                    });
                }
                response = employeeOptions.ToArray();
            }
            return response;
        }
        private static short ComputeWorkingDaysInMonth(short year, short month)
        {
            // Get the total number of days in the month
            int totalDaysInMonth = DateTime.DaysInMonth(year, month);

            // Get the day of the week for the first day of the month (0 = Sunday, ..., 6 = Saturday)
            int firstDayOfWeek = (int)new DateTime(year, month, 1).DayOfWeek;

            return ComputeWorkingDays(totalDaysInMonth, firstDayOfWeek);
        }
        private static short ComputeWorkingDaysInYear(short year)
        {
            // Check if the year is a leap year
            bool isLeapYear = DateTime.IsLeapYear(year);

            // Total days in the year
            int totalDaysInYear = isLeapYear ? 366 : 365;

            // Day of the week for January 1 (0 = Sunday, ..., 6 = Saturday)
            int firstDayOfWeek = (int)new DateTime(year, 1, 1).DayOfWeek;

            return ComputeWorkingDays(totalDaysInYear, firstDayOfWeek);
        }

        private static short ComputeWorkingDays(int totalDays, int firstDayOfWeek)
        {
            // Calculate total Saturdays
            int saturdayOffset = (6 - firstDayOfWeek + 7) % 7; // First Saturday
            int saturdays = (totalDays - saturdayOffset) / 7 + 1;

            // Calculate total Sundays
            int sundayOffset = (7 - firstDayOfWeek) % 7; // First Sunday
            int sundays = (totalDays - sundayOffset) / 7 + 1;

            // Return the computed working days
            return (short)(totalDays - saturdays - sundays);
        }
    }
}
