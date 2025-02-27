using HRDemoAdmin.Services.Models;
using System.Threading.Tasks;

namespace HRDemoAdmin.Services
{
    public class UserService : ServiceBase
    {
        public UserService(string baseUrl) : base(baseUrl)
        { 
        }
        public UserResponse GetUserDetails()
        {
            return Get<UserResponse>("/user").Data;
        }
    }
}
