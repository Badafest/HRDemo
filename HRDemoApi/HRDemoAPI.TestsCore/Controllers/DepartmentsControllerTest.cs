using HRDemoAPICore.Controllers;
using HRDemoAPICore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;

namespace HRDemoAPI.TestsCore.Controllers
{
    [TestClass]
    public class DepartmentsControllerTest : ControllerTestBase
    {
        [TestMethod]
        public void Get()
        {
            // Arrange
            DepartmentsController controller = new(_dbContext);

            // Act
            IEnumerable<DepartmentResponse> result = controller.Get();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result.Count());
        }

        [TestMethod]
        public void GetById()
        {
            // Arrange
            DepartmentsController controller = new(_dbContext);

            // Act
            ObjectResult result = controller.Get(1);
            // Assert
            Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);

            DepartmentResponse? departmentResponse = result.Value as DepartmentResponse;
            Assert.IsNotNull(departmentResponse);
            Assert.AreEqual(1, departmentResponse?.DepartmentID);
        }
    }
}
