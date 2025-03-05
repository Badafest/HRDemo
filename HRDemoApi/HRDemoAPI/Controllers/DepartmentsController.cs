using HRDemoAPI.Data;
using HRDemoAPI.Filters;
using HRDemoAPI.Models;
using HRDemoAPI.Utilities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace HRDemoAPI.Controllers
{
    [HRDemoAuthorize]
    public class DepartmentsController : ApiController
    {
        private readonly HRDemoApiDbContainer _hRDemoAPIDb;
        public DepartmentsController(HRDemoApiDbContainer hRDemoAPIDb)
        {
            _hRDemoAPIDb = hRDemoAPIDb;
        }
        // GET api/Departments
        public IEnumerable<DepartmentResponse> Get(int count = default, int page = default, string name = default)
        {
             return _hRDemoAPIDb.Departments
                .Where(d => string.IsNullOrEmpty(name) || d.Name.Contains(name))
                .OrderBy(d => d.DepartmentID)
                .Paginate(count, page)
                .Include("Manager")
                .AsNoTracking()
                .ToList()
                .Select(d => d.MapQueryResult());
        }

        // GET api/Departments/5
        public HttpResponseMessage Get(int id)
        {
            DepartmentResponse department = _hRDemoAPIDb.Departments
                .Where(d => d.DepartmentID == id)
                .Include("Manager")
                .AsNoTracking()
                .ToList()
                .Select(d => d.MapQueryResult())
                .FirstOrDefault();

            if (department == null || department.DepartmentID != id)
            {
                return HttpUtilities.CreateResponseMessage(null, System.Net.HttpStatusCode.NotFound);
            }
            return department.CreateResponseMessage();
        }

        // POST api/Departments
        public HttpResponseMessage Post([FromBody]DepartmentRequest departmentRequest)
        {
            var validatedResponse = HttpUtilities.ValidateAdminRole();
            if (validatedResponse != null)
            {
                return validatedResponse;
            }
            Department newDepartment = departmentRequest.MapPostRequest();
            var savedDepartment = _hRDemoAPIDb.Departments.Add(newDepartment);
            _hRDemoAPIDb.SaveChanges();
            return savedDepartment.CreateResponseMessage(System.Net.HttpStatusCode.Created);
        }

        // PUT api/Departments/5
        public HttpResponseMessage Put(int id, [FromBody]DepartmentRequest departmentRequest)
        {
            var validatedResponse = HttpUtilities.ValidateManagerRole(id);
            if (validatedResponse != null)
            {
                return validatedResponse;
            }
            Department department = _hRDemoAPIDb.Departments.Find(id);
            if (department == null)
            {
                return HttpUtilities.CreateResponseMessage(null, System.Net.HttpStatusCode.NotFound);
            }
            Department newDepartment = departmentRequest.MapPutRequest(id);
            department.Name = newDepartment.Name;
            department.Description = newDepartment.Description;
            department.ManagerID = newDepartment.ManagerID;
            _hRDemoAPIDb.SaveChanges();
            return department.CreateResponseMessage();
        }

        // PATCH api/Departments/5
        public HttpResponseMessage Patch(int id, [FromBody] DepartmentRequest departmentRequest)
        {
            var validatedResponse = HttpUtilities.ValidateManagerRole(id);
            if (validatedResponse != null)
            {
                return validatedResponse;
            }
            Department department = _hRDemoAPIDb.Departments.Find(id);
            if (department == null)
            {
                return HttpUtilities.CreateResponseMessage(null, System.Net.HttpStatusCode.NotFound);
            }
            if (!string.IsNullOrEmpty(departmentRequest.Name)) 
            {
                department.Name = departmentRequest.Name;
            }
            if (!string.IsNullOrEmpty(departmentRequest.Description))
            {
                department.Description = departmentRequest.Description;
            }
            if (departmentRequest.ManagerID != default)
            {
                department.ManagerID = departmentRequest.ManagerID;
            }
            _hRDemoAPIDb.SaveChanges();
            return department.CreateResponseMessage();
        }

        // DELETE api/Departments/5
        public HttpResponseMessage Delete(int id)
        {
            var validatedResponse = HttpUtilities.ValidateAdminRole();
            if (validatedResponse != null)
            {
                return validatedResponse;
            }
            Department department = _hRDemoAPIDb.Departments.Find(id);
            if(department == null)
            {
                return HttpUtilities.CreateResponseMessage(null, System.Net.HttpStatusCode.NotFound);
            }
            _hRDemoAPIDb.Departments.Remove(department);
            _hRDemoAPIDb.SaveChanges();
            return HttpUtilities.CreateResponseMessage(null);
        }
    }
}
