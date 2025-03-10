using HRDemoReportService.Data;
using System.Configuration;

namespace HRDemoReportService
{
	// NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
	public class HRDemoReportService : IHRDemoReportService
	{
        private readonly HRDemoReportDataService dataService;
        public HRDemoReportService()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["HRDemoReportDb"].ConnectionString;
            var commandTimeout = int.Parse(ConfigurationManager.AppSettings["CommandTimeout"]);
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
