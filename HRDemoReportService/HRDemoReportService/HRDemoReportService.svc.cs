using HRDemoReportService.Data;
using System.Configuration;

namespace HRDemoReportService
{
	// NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
	public class HRDemoReportService : IHRDemoReportService
	{
		public ReportResponse GetDataUsingDataContract(ReportRequest request)
		{
			var connectionString = ConfigurationManager.ConnectionStrings["HRDemoReportDb"].ConnectionString;
            var commandTimeout = int.Parse(ConfigurationManager.AppSettings["CommandTimeout"]);
            var dataService = new HRDemoReportDataService(connectionString, commandTimeout);
			return dataService.GetReportData(request);
        }
	}
}
