using HRDemoReportUI.Service;
using HRDemoReportUI.Service.HRDemoReportService;
using System;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace HRDemoReportUI
{
    public partial class _Default : Page
    {
        private readonly ReportService reportService;

        private readonly EmployeeResponse[] defaultEmployeeOptions =new EmployeeResponse[] 
        {
            new EmployeeResponse()
            {
                EmployeeName = "-- Select an Employee --",
                EmployeeID = 0
            }
        };

        public EmployeeResponse[] employeeOptions = new EmployeeResponse[] { };

        private DateTimeOffset lastEmployeeSearched = DateTimeOffset.MinValue;

        public ReportResponse ReportData { get; set; }
        public _Default()
        {
            var configuration = ConfigurationManager.AppSettings;
            reportService = new ReportService(configuration["ServiceApiUrl"], configuration["ServiceApiKey"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }
            employeeOptions = defaultEmployeeOptions.Concat(GetEmployeeOptions()).ToArray();
            BindEmployeeOptions(employeeOptions);
            BindReportYearAndMonthOptions();
        }

        protected void SearchText_Change(object sender, EventArgs e)
        {
            DateTimeOffset newSearchedDate = DateTimeOffset.Now;
            if (newSearchedDate - lastEmployeeSearched < TimeSpan.FromSeconds(0.5))
            {
                return;
            }
            lastEmployeeSearched = newSearchedDate;
            employeeOptions = defaultEmployeeOptions.Concat(GetEmployeeOptions(employeeSearch.Text)).ToArray();
            BindEmployeeOptions(employeeOptions);
        }

        private void BindEmployeeOptions(EmployeeResponse[] employeeOptions)
        {
            employeeSelectList.DataSource = employeeOptions;
            employeeSelectList.DataTextField = "EmployeeName";
            employeeSelectList.DataValueField = "EmployeeID";

            employeeSelectList.DataBind();
        }

        private void BindReportYearAndMonthOptions()
        {
            var currentYear = DateTimeOffset.Now.Year;
            reportYearSelectList.DataSource = Enumerable.Range(currentYear - 4, 5).Reverse();
            reportYearSelectList.DataBind();

            dynamic monthOptions = DateTimeFormatInfo.InvariantInfo.MonthNames
                .Where(option => !string.IsNullOrEmpty(option))
                .Select((option, index) => new
                {
                    Text = option,
                    Value = index + 1
                }).ToArray();

            reportMonthSelectList.DataSource = monthOptions;
            reportMonthSelectList.DataTextField = "Text";
            reportMonthSelectList.DataValueField = "Value";
            reportMonthSelectList.SelectedValue = DateTime.Now.Month.ToString();
            reportMonthSelectList.DataBind();
        }
        private EmployeeResponse[] GetEmployeeOptions(string searchName = "")
        {
            // obtain user data from master page if available else fetch from API
            var userData = ((SiteMaster)Master).UserData ?? reportService.GetUserData(new UserRequest
            {
                Username = HttpContext.Current.User.Identity.Name,
            });

            return reportService.GetEmployeeOptions(new EmployeeRequest()
            {
                ManagedDepartments = userData.ManagedDepartments.Select(d => d.DepartmentID).ToArray(),
                SearchName = searchName
            });
        }
        protected void GenerateMyReportBtn_Click(object sender, EventArgs e)
        {
            var reportRequest = new ReportRequest()
            {
                EmployeeID = ((SiteMaster)Master).UserData.EmployeeID,
                Month = short.TryParse(reportMonthSelectList.SelectedValue, out short month) ? month : (short)0,
                Year = short.TryParse(reportYearSelectList.SelectedValue, out short year) ? year : (short)1900,
                TimezoneOffset = float.TryParse(timezoneOffset.Value, out float offset) ? offset : 0
            };
            ReportData = reportService.GetReportData(reportRequest);
        }

        protected void GenerateEmployeeReportBtn_Click(object sender, EventArgs e)
        {
            var reportRequest = new ReportRequest()
            {
                EmployeeID = int.TryParse(employeeSelectList.SelectedValue,out int id) ? id : 0,
                Month = short.TryParse(reportMonthSelectList.SelectedValue, out short month) ? month : (short)0,
                Year = short.TryParse(reportYearSelectList.SelectedValue, out short year) ? year : (short)1900,
                TimezoneOffset = float.TryParse(timezoneOffset.Value, out float offset) ? offset : 0
            };
            ReportData = reportService.GetReportData(reportRequest);
        }

        protected void GetPDFReportBtn_Click(object sender, EventArgs e)
        {

        }
    }
}