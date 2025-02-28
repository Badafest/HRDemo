using HRDemoAPI.Data;
using HRDemoAPI.Filters;
using HRDemoAPI.Utilities;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace HRDemoAPI.Controllers
{
    [HRDemoAuthorize]
    public class EmployeeReportController : ApiController
    {
        private readonly HRDemoApiDbContainer _hRDemoAPIDb;
        public EmployeeReportController(HRDemoApiDbContainer hRDemoAPIDb)
        {
            _hRDemoAPIDb = hRDemoAPIDb;
        }

        // GET api/employees/5
        public HttpResponseMessage Get(int id, int year, int month, double timezoneoffset = -3)
        {
            GetEmployeeMonthlyReport_Result report = _hRDemoAPIDb.GetEmployeeMonthlyReport(id, year, month, timezoneoffset).FirstOrDefault();
            if (report == null || report.EmployeeID != id)
            {
                return HttpUtilities.CreateResponseMessage(null, System.Net.HttpStatusCode.NotFound);
            }
            return report.CreateResponseMessage();
        }
    }
}
