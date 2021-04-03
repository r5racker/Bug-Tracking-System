using Bug_Tracker_Service.Controllers;
using Bug_Tracker_Service.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace Bug_Tracker_Service.Tests.Controllers
{
    [TestClass]
    public class CategoryControllerTest
    {
        [TestMethod]
        public void GetCategories()
        {
            // Arrange
            CategoryController controller = new CategoryController();
            
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            IHttpActionResult actionResult = controller.GetCategories();
            var contentResult = actionResult as OkNegotiatedContentResult<List<BugCategory>>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.IsTrue(contentResult.Content.Count > 0);
        }
    }
}
