using HRDemoAPICore.Controllers;
using HRDemoAPICore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;

namespace HRDemoAPI.TestsCore.Controllers
{
    [TestClass]
    public class EmployeesControllerTest: ControllerTestBase
    {
        [TestMethod]
        public void Get()
        {
            // Arrange
            EmployeesController controller = new(_dbContext);

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
            EmployeesController controller = new(_dbContext);

            // Act
            ObjectResult result = controller.Get(1);
            // Assert
            Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);

            EmployeePublicResponse? employeeResponse = result.Value as EmployeePublicResponse;
            Assert.IsNotNull(employeeResponse);
            Assert.AreEqual(1, employeeResponse?.EmployeeID);
        }
    }
}
