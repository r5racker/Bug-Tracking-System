using Bug_Tracker_Service.Controllers;
using Bug_Tracker_Service.Models;
using Bug_Tracker_Service.Models.AuthModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace Bug_Tracker_Service.Tests.Controllers
{
    [TestClass]
    public class AuthControllerTest
    {
        [TestMethod]
        public void Login()
        {
            // Arrange
            AuthController controller = new AuthController();
            string id = "31";
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            AuthModel am = new AuthModel() { 
            Email= "dev1@bts.com",
            Pwd = "123456789"
            };

            IHttpActionResult actionResult = controller.Login(am);
            var contentResult = actionResult as OkNegotiatedContentResult<Person>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(id, contentResult.Content.PersonId.ToString());
        }
    }
}
