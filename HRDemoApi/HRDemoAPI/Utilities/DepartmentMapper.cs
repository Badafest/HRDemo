using HRDemoAPI.Data;
using HRDemoAPI.Models;

namespace HRDemoAPI.Utilities
{
    public static class DepartmentMapper
    {
        public static DepartmentResponse MapQueryResult(this Department department)
        {
            return new DepartmentResponse
            {
                DepartmentID = department.DepartmentID,
                Name = department.Name,
                Description = department.Description,
                Manager = department.Manager == null ? null : new ManagerResponse
                {
                    FirstName = department.Manager.FirstName,
                    LastName = department.Manager.LastName,
                    Phone = department.Manager.Phone,
                    Email = department.Manager.Email,
                    JobTitle = department.Manager.JobTitle,
                    Status = department.Manager.Status,
                    Address = new EmployeeResponseAddress
                    {
                        Line1 = department.Manager.Address.Line1,
                        Line2 = department.Manager.Address.Line2,
                        City = department.Manager.Address.City,
                        PostalCode = department.Manager.Address.PostalCode,
                        State = department.Manager.Address.State,
                        Country = department.Manager.Address.Country
                    }
                }
            };
        }
        public static Department MapPostRequest(this DepartmentRequest departmentRequest)
        {
            Department department = MapRequest(departmentRequest);
            if (department.ManagerID == 0)
            {
                department.ManagerID = null;
            }
            return department;
        }

        public static Department MapPutRequest(this DepartmentRequest departmentRequest, int departmentId)
        {
            Department department = MapRequest(departmentRequest);
            if (department.ManagerID == 0)
            {
                department.ManagerID = null;
            }
            department.DepartmentID = departmentId;
            return department;
        }

        private static Department MapRequest(this DepartmentRequest departmentRequest)
        {
            return new Department()
            {
                Name = departmentRequest.Name,
                Description = departmentRequest.Description,
                ManagerID = departmentRequest.ManagerID
            };
        }
    }
}