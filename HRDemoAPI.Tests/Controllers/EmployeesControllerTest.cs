using HRDemoAPI.Controllers;
using HRDemoAPI.Data;
using HRDemoApp.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace HRDemoAPI.Tests.Controllers
{
    [TestClass]
    public class EmployeesControllerTest
    {
        [TestMethod]
        public void Get()
        {
            // Arrange
            EmployeesController controller = new EmployeesController(new HRDemoApiDbContainer());

            // Act
            IEnumerable<EmployeePublicResponse> result = controller.Get();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result.Count());
        }

        [TestMethod]
        public void GetById()
        {
            // Arrange
            EmployeesController controller = new EmployeesController(new HRDemoApiDbContainer());

            // Act
            HttpResponseMessage result = controller.Get(1);
            // Assert
            Assert.IsTrue(result.IsSuccessStatusCode);

            EmployeePublicResponse employeeResponse = JsonConvert.DeserializeObject<EmployeePublicResponse>(result.Content.ReadAsStringAsync().Result);
            Assert.IsNotNull(employeeResponse);
            Assert.AreEqual(1, employeeResponse.EmployeeID);
        }
    }
}
