using HRDemoAdmin.Services.Models;

namespace HRDemoAdmin.Services
{
    public class UserService : ServiceBase
    {
        public UserService(string baseUrl, string bearerToken = null) : base(baseUrl, bearerToken)
        { 
        }
        public UserResponse GetUserDetails()
        {
            return Get<UserResponse>("/user").Data;
        }
    }
}
