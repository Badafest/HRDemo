using HRDemoReportUI.Service;
using HRDemoReportUI.Service.HRDemoReportService;
using System;
using System.Configuration;
using System.Web;
using System.Web.UI;

namespace HRDemoReportUI
{
    public partial class SiteMaster : MasterPage
    {
        private readonly ReportService reportService;

        public UserResponse UserData 
        { 
            get { return (UserResponse)ViewState["UserData"]; }
            set { ViewState["UserData"] = value; }
        }
        public SiteMaster()
        {
            var configuration = ConfigurationManager.AppSettings;
            reportService = new ReportService(configuration["ServiceApiUrl"], configuration["ServiceApiKey"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            UserData = reportService.GetUserData(new UserRequest
            {
                Username = HttpContext.Current.User.Identity.Name,
            });
        }
    }
}