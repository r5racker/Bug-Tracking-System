using Bug_Tracker_Service.Controllers;
using Bug_Tracker_Service.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Bug_Tracker_Service.Tests.Controllers
{
    [TestClass]
    public class BugRetreatControllerTest
    {
        [TestMethod]
        public void Post()
        {
            BugRetreatController controller = new BugRetreatController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            int id = 20;
            StatusChangeModel sm = new StatusChangeModel
            {
                id = id,
                developerId = 5,
                assignedBy = 5,
                bugAlertResolutionDescription= "testing"
            };

            // Act
            var response = controller.Post(sm);

            // Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }
    }
}
