using HRDemoAPI.Controllers;
using HRDemoAPI.Data;
using HRDemoAPI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace HRDemoAPI.Tests.Controllers
{
    [TestClass]
    public class DepartmentsControllerTest
    {
        [TestMethod]
        public void Get()
        {
            // Arrange
            DepartmentsController controller = new DepartmentsController(new HRDemoApiDbContainer());

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
            DepartmentsController controller = new DepartmentsController(new HRDemoApiDbContainer());

            // Act
            HttpResponseMessage result = controller.Get(1);
            // Assert
            Assert.IsTrue(result.IsSuccessStatusCode);

            DepartmentResponse departmentResponse = JsonConvert.DeserializeObject<DepartmentResponse>(result.Content.ReadAsStringAsync().Result);
            Assert.IsNotNull(departmentResponse);
            Assert.AreEqual(1, departmentResponse.DepartmentID);
        }
    }
}
