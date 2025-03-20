using HRDemoReportUI.ServiceCore;
using HRDemoReportUI.ServiceCore.HRDemoReportService;

namespace HRDemoReportUICore
{
    public class UserService(IHttpContextAccessor httpContextAccessor, ReportService reportService)
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly ReportService _reportService = reportService;

        private UserResponse _userData { get; set; } = new();
        public UserResponse UserData {
            get {
                var username = _httpContextAccessor.HttpContext?.User?.Identity?.Name;

                if (_userData.EmployeeID == default && !string.IsNullOrEmpty(username))
                {
                    _userData = _reportService.GetUserData(new() { Username = username });
                }

                return _userData;
            }
            set {
                throw new NotImplementedException();
            }
        }
    }
}
