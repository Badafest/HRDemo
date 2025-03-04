using HRDemoAPI.DataCore.Models;
using HRDemoAPICore.Filters;
using HRDemoAPICore.Models;
using HRDemoAPICore.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRDemoAPICore.Controllers
{
    [ServiceFilter<HRDemoAuthorizeFilter>]
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentsController(HRDemoApiContext hrDemoApiDb) : ControllerBase
    {
        private readonly HRDemoApiContext _hRDemoAPIDb = hrDemoApiDb;

        [HttpGet]
        // GET api/Departments
        public IEnumerable<DepartmentResponse> Get(int count = default, int page = default, string? name = default)
        {
            return _hRDemoAPIDb.Departments
               .Where(d => string.IsNullOrEmpty(name) || d.Name.Contains(name))
               .OrderBy(d => d.DepartmentID)
               .Paginate(count, page, HttpContext)
               .Include("Manager")
               .AsNoTracking()
               .ToList()
               .Select(d => d.MapQueryResult());
        }

        [HttpGet("{id}")]

        // GET api/Departments/5
        public ObjectResult Get(int id)
        {
            DepartmentResponse? department = _hRDemoAPIDb.Departments
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

        [HttpPost]

        // POST api/Departments
        public ObjectResult Post([FromBody] DepartmentRequest departmentRequest)
        {
            var validatedResponse = HttpUtilities.ValidateAdminRole(HttpContext);
            if (validatedResponse != null)
            {
                return validatedResponse;
            }
            Department newDepartment = departmentRequest.MapPostRequest();
            var savedDepartment = _hRDemoAPIDb.Departments.Add(newDepartment);
            _hRDemoAPIDb.SaveChanges();
            return savedDepartment.CreateResponseMessage(System.Net.HttpStatusCode.Created);
        }

        [HttpPut("{id}")]

        // PUT api/Departments/5
        public ObjectResult Put(int id, [FromBody] DepartmentRequest departmentRequest)
        {
            var validatedResponse = HttpUtilities.ValidateManagerRole(HttpContext, id);
            if (validatedResponse != null)
            {
                return validatedResponse;
            }
            Department? department = _hRDemoAPIDb.Departments.Find(id);
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

        [HttpPatch("{id}")]

        // PATCH api/Departments/5
        public ObjectResult Patch(int id, [FromBody] DepartmentRequest departmentRequest)
        {
            var validatedResponse = HttpUtilities.ValidateManagerRole(HttpContext, id);
            if (validatedResponse != null)
            {
                return validatedResponse;
            }
            Department? department = _hRDemoAPIDb.Departments.Find(id);
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

        [HttpDelete("{id}")]

        // DELETE api/Departments/5
        public ObjectResult Delete(int id)
        {
            var validatedResponse = HttpUtilities.ValidateManagerRole(HttpContext, id);
            if (validatedResponse != null)
            {
                return validatedResponse;
            }
            Department? department = _hRDemoAPIDb.Departments.Find(id);
            if (department == null)
            {
                return HttpUtilities.CreateResponseMessage(null, System.Net.HttpStatusCode.NotFound);
            }
            _hRDemoAPIDb.Departments.Remove(department);
            _hRDemoAPIDb.SaveChanges();
            return HttpUtilities.CreateResponseMessage(null);
        }
    }
}
