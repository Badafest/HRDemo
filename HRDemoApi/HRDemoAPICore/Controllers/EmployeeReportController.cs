using HRDemoAPI.DataCore.Models;
using HRDemoAPICore.Filters;
using HRDemoAPICore.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace HRDemoAPICore.Controllers
{
    [ServiceFilter<HRDemoAuthorizeFilter>]
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeReportController(HRDemoApiContext hrDemoApi) : ControllerBase
    {
        private readonly HRDemoApiContext _hRDemoAPIDb = hrDemoApi;

        [HttpGet("{id}")]

        // GET api/employees/5
        public async Task<ObjectResult> Get(int id, int year, int month, double timezoneoffset = -3)
        {
            GetEmployeeMonthlyReportResult? report = (await _hRDemoAPIDb.Procedures.GetEmployeeMonthlyReportAsync(id, year, month, timezoneoffset)).FirstOrDefault();
            if (report == null || report.EmployeeID != id)
            {
                return HttpUtilities.CreateResponseMessage(null, System.Net.HttpStatusCode.NotFound);
            }
            return report.CreateResponseMessage();
        }
    }
}
