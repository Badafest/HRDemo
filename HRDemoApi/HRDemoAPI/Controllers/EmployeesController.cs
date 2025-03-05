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
    public class EmployeesController : ApiController
    {
        private readonly HRDemoApiDbContainer _hRDemoAPIDb;
        public EmployeesController(HRDemoApiDbContainer hRDemoAPIDb)
        {
            _hRDemoAPIDb = hRDemoAPIDb;
        }
        // GET api/employees
        public IEnumerable<EmployeePublicResponse> Get(int count = default, int page = default, string firstName = default, string lastName = default, string jobTitle = default, string phone = default, string email = default)
        {
            return _hRDemoAPIDb.Employees
                .Where(e => string.IsNullOrEmpty(firstName) || e.FirstName.Contains(firstName))
                .Where(e => string.IsNullOrEmpty(lastName) ||  e.LastName.Contains(lastName))
                .Where(e => string.IsNullOrEmpty(jobTitle) || (e.JobTitle == jobTitle))
                .Where(e => string.IsNullOrEmpty(phone) || e.Phone == phone)
                .Where(e => string.IsNullOrEmpty(email) || e.Email == email)
                .OrderBy(e => e.EmployeeID)
                .Paginate(count, page)
                .Include("Department")
                .AsNoTracking()
                .ToList()
                .Select(e => e.MapQueryResult());
        }

        // GET api/employees/5
        public HttpResponseMessage Get(int id, bool salary = false)
        {
            EmployeePublicResponse employee = _hRDemoAPIDb.Employees
                .Where(e => e.EmployeeID == id)
                .Include("Department.Manager")
                .AsNoTracking()
                .ToList()
                .Select(e => e.MapQueryResult(!salary))
                .FirstOrDefault();
            if (employee == null || employee.EmployeeID != id)
            {
                return HttpUtilities.CreateResponseMessage(null, System.Net.HttpStatusCode.NotFound);
            }
            if (salary)
            {
                var validatedResponse = HttpUtilities.ValidateManagerRole(employee.Department.DepartmentID);
                if (validatedResponse != null)
                {
                    return validatedResponse;
                }
            }
            return employee.CreateResponseMessage();
        }

        // POST api/employees
        public HttpResponseMessage Post([FromBody]EmployeeRequest employeeRequest)
        {
            var validatedResponse = HttpUtilities.ValidateManagerRole(employeeRequest.DepartmentID);
            if (validatedResponse != null)
            {
                return validatedResponse;
            }
            Employee newEmployee = employeeRequest.MapPostRequest();
            Employee savedEmployee = _hRDemoAPIDb.Employees.Add(newEmployee);
            _hRDemoAPIDb.SaveChanges();
            return savedEmployee.CreateResponseMessage();
        }

        // PUT api/employees/5
        public HttpResponseMessage Put(int id, [FromBody]EmployeeRequest employeeRequest)
        {
            Employee employee = _hRDemoAPIDb.Employees.Find(id);
            if (employee == null)
            {
                return HttpUtilities.CreateResponseMessage(null, System.Net.HttpStatusCode.NotFound);
            }
            var validatedResponse = HttpUtilities.ValidateManagerRole(new int?[] { employee.DepartmentID, employeeRequest.DepartmentID });
            if (validatedResponse != null)
            {
                return validatedResponse;
            }
            Employee newEmployee = employeeRequest.MapPutRequest(id);
            employee.FirstName = newEmployee.FirstName;
            employee.LastName = newEmployee.LastName;
            employee.Email = newEmployee.Email;
            employee.Phone = newEmployee.Phone;
            employee.JobTitle = newEmployee.JobTitle;
            employee.Address = newEmployee.Address;
            employee.Salary = newEmployee.Salary;
            employee.DepartmentID = newEmployee.DepartmentID;
            _hRDemoAPIDb.SaveChanges();
            return employee.CreateResponseMessage();
        }

        // PATCH api/Employees/5
        public HttpResponseMessage Patch(int id, [FromBody] EmployeeRequest employeeRequest)
        {
            Employee employee = _hRDemoAPIDb.Employees.Find(id);
            if (employee == null)
            {
                return HttpUtilities.CreateResponseMessage(null, System.Net.HttpStatusCode.NotFound);
            }
            var validatedResponse = HttpUtilities.ValidateManagerRole(new int?[] { employee.DepartmentID, employeeRequest.DepartmentID });
            if (validatedResponse != null)
            {
                return validatedResponse;
            }
            if (!string.IsNullOrEmpty(employeeRequest.FirstName))
            {
                employee.FirstName = employeeRequest.FirstName;
            }
            if (!string.IsNullOrEmpty(employeeRequest.LastName))
            {
                employee.LastName = employeeRequest.LastName;
            }
            if (!string.IsNullOrEmpty(employeeRequest.Phone))
            {
                employee.Phone = employeeRequest.Phone;
            }
            if (!string.IsNullOrEmpty(employeeRequest.Email))
            {
                employee.Email = employeeRequest.Email;
            }
            if (!string.IsNullOrEmpty(employeeRequest.JobTitle))
            {
                employee.JobTitle = employeeRequest.JobTitle;
            }
            if(employeeRequest.Salary != default)
            {
                employee.Salary = employeeRequest.Salary;
            }
            if(employeeRequest.Address!= null)
            {
                if (!string.IsNullOrEmpty(employeeRequest.Address.Line1))
                {
                    employee.Address.Line1 = employeeRequest.Address.Line1;
                }
                if (!string.IsNullOrEmpty(employeeRequest.Address.Line2))
                {
                    employee.Address.Line2 = employeeRequest.Address.Line2;
                }
                if (!string.IsNullOrEmpty(employeeRequest.Address.City))
                {
                    employee.Address.City = employeeRequest.Address.City;
                }
                if (!string.IsNullOrEmpty(employeeRequest.Address.State))
                {
                    employee.Address.State = employeeRequest.Address.State;
                }
                if (!string.IsNullOrEmpty(employeeRequest.Address.PostalCode))
                {
                    employee.Address.PostalCode = employeeRequest.Address.PostalCode;
                }
                if (!string.IsNullOrEmpty(employeeRequest.Address.Country))
                {
                    employee.Address.Country = employeeRequest.Address.Country;
                }
            }
            if (employeeRequest.DepartmentID != default)
            {
                employee.DepartmentID = employeeRequest.DepartmentID;
            }
            _hRDemoAPIDb.SaveChanges();
            return employee.CreateResponseMessage();
        }

        // DELETE api/employees/5
        public HttpResponseMessage Delete(int id)
        {
            Employee employee = _hRDemoAPIDb.Employees.Find(id);
            if (employee == null)
            {
                return HttpUtilities.CreateResponseMessage(null, System.Net.HttpStatusCode.NotFound);
            }
            var validatedResponse = HttpUtilities.ValidateManagerRole(employee.DepartmentID);
            if (validatedResponse != null)
            {
                return validatedResponse;
            }
            _hRDemoAPIDb.Employees.Remove(employee);
            _hRDemoAPIDb.SaveChanges();
            return HttpUtilities.CreateResponseMessage(null);
        }
    }
}
