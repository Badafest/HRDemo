using HRDemoAPI.Data;
using HRDemoAPI.Models;
using Newtonsoft.Json;
using System;

namespace HRDemoAPI.Utilities
{
    public static class EmployeeMapper
    {
        public static EmployeePublicResponse MapQueryResult(this Employee employee, bool isPublic = true)
        {
            var publicResponse = new EmployeePublicResponse
            {
                EmployeeID = employee.EmployeeID,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                Phone = employee.Phone,
                JobTitle = employee.JobTitle,
                Status = employee.Status,
                Address = new EmployeeResponseAddress
                {
                    Line1 = employee.Address.Line1,
                    Line2 = employee.Address.Line2,
                    City = employee.Address.City,
                    State = employee.Address.State,
                    PostalCode = employee.Address.PostalCode,
                    Country = employee.Address.Country
                },
                Department = employee.Department?.MapQueryResult()
            };

            if (isPublic)
            {
                return publicResponse;
            }

            var employeeResponse = JsonConvert.DeserializeObject<EmployeeResponse>(JsonConvert.SerializeObject(publicResponse));
            employeeResponse.Salary = employee.Salary;
            return employeeResponse;
        }
        public static Employee MapPostRequest(this EmployeeRequest employeeRequest)
        {
            Employee employee = MapRequest(employeeRequest);
            if(employee.DepartmentID == default)
            {
                employee.DepartmentID = null;
            }
            employee.Status = EmployeeStatus.Pending;
            employee.DateOfHire = DateTimeOffset.MinValue;
            return employee;
        }

        public static Employee MapPutRequest(this EmployeeRequest employeeRequest, int employeeId)
        {
            Employee employee = MapRequest(employeeRequest);
            if (employee.DepartmentID == default)
            {
                employee.DepartmentID = null;
            }
            employee.EmployeeID = employeeId;
            return employee;
        }

        private static Employee MapRequest(EmployeeRequest employeeRequest)
        {
            return new Employee()
            {
                FirstName = employeeRequest.FirstName,
                LastName = employeeRequest.LastName,
                Email = employeeRequest.Email,
                Phone = employeeRequest.Phone,
                JobTitle = employeeRequest.JobTitle,
                Salary = employeeRequest.Salary,
                DepartmentID = employeeRequest.DepartmentID,
                Address = new EmployeeAddress()
                {
                    Line1 = employeeRequest.Address.Line1,
                    Line2 = employeeRequest.Address.Line2 ?? "",
                    City = employeeRequest.Address.City,
                    State = employeeRequest.Address.State,
                    PostalCode = employeeRequest.Address.PostalCode,
                    Country = employeeRequest.Address.Country,
                }
            };
        }
    }
}