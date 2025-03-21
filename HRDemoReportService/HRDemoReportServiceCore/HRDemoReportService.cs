using HRDemoReportService.DataCore;

namespace HRDemoReportServiceCore
{
    public class HRDemoReportService : IHRDemoReportService
    {
          private readonly HRDemoReportDataService dataService;
        public HRDemoReportService(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("HRDemoReportDb");
            var commandTimeout = int.Parse(configuration["CommandTimeout"] ?? "30");
            dataService = new HRDemoReportDataService(connectionString, commandTimeout);
        }
        public ReportResponse GetEmployeeMonthlyReport(ReportRequest request)
        {
            return dataService.GetReportData(request);
        }

        public UserResponse GetUserDetails(UserRequest username)
        {
            return dataService.GetUserDetails(username.Username);
        }

        public EmployeeResponse[] GetEmployeeOptions(EmployeeRequest request)
        {
            return dataService.GetEmployeeOptions(request);
        }
    }
}
