using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace HRDemoReportService.Data
{
    public class HRDemoReportDataService
    {
        private readonly string _connectionString;
        private readonly int _commandTimeout;
        public HRDemoReportDataService(string connectionString, int commandTimeout)
        {
            _commandTimeout = commandTimeout;
            _connectionString = connectionString;
        }

        public ReportResponse GetReportData(ReportRequest request)
        {
            var response = new ReportResponse();
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
                    using (var reader = command.ExecuteReader())
                    {
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
                }

                // payrolls
                using (var query = connection.CreateCommand())
                {
                    query.CommandText = "SELECT PayrollID, Month, Year, Salary_GrossAmount, Salary_PreTaxDeduction, Salary_TaxDeduction, Salary_PostTaxDeduction, Salary_NetAmount FROM dbo.Payrolls WHERE EmployeeID = @employeeId AND Year = @year AND Month = @month";
                    query.Parameters.Add(new SqlParameter("@employeeId", SqlDbType.Int) { Value = request.EmployeeID });
                    query.Parameters.Add(new SqlParameter("@year", SqlDbType.SmallInt) { Value = request.Year });
                    query.Parameters.Add(new SqlParameter("@month", SqlDbType.SmallInt) { Value = request.Month });
                    query.CommandTimeout = _commandTimeout;
                    using (var reader = query.ExecuteReader())
                    {
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
                        response.PayrollReports = payrolls.ToArray();
                    }
                }
            }
            return response;
        }
    }
}
