using Bug_Tracker_Service;
using Bug_Tracker_Service.Controllers;
using Bug_Tracker_Service.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace Bug_Tracker_Service.Tests.Controllers
{
    [TestClass]
    public class BugAlertControllerTest
    {
        ApiResponseFactory responseFactory = new ApiResponseFactory();

        [TestMethod]
        public void GetBugList()
        {
            // Arrange
            BugAlertController controller = new BugAlertController();
            int pid = 5;
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            IHttpActionResult actionResult = controller.GetBugList(BugAlertFilter.All, pid);
            var contentResult = actionResult as OkNegotiatedContentResult<List<BugAlert>>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.IsTrue(contentResult.Content.Count()>0);
        }
        [TestMethod]
        public void GetBug()
        {
            // Arrange
            BugAlertController controller = new BugAlertController();
            int id = 20;
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            IHttpActionResult actionResult = controller.GetBug(id);
            var contentResult = actionResult as OkNegotiatedContentResult<BugAlert>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(id, contentResult.Content.BugId);
        }

        [TestMethod]
        public void Post()
        {
            BugAlertController controller = new BugAlertController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            
            string successMsg = responseFactory.Generate(ApiResponseType.UserCreate);
            string errorMsgPrefix = responseFactory.Generate(ApiResponseType.UserActionError);
            BugAlert ba = new BugAlert()
            {
                CategoryId = 1,
                Description = "Testing bug post on" + DateTime.Now.ToString(),
                Title = "Testing",
                CreatedBy = 5
            };
            
            // Act
            var response = controller.Post(ba);

            // Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [TestMethod]
        public void Delete()
        {
            string successMsg = responseFactory.Generate(ApiResponseType.UserDelete);
            int bugId = 21;
            BugAlertController controller = new BugAlertController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            var response = controller.Delete(bugId);
            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        public void Put()
        {
            // Arrange
            BugAlertController controller = new BugAlertController();
            int id = 20;
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            IHttpActionResult actionResult = controller.GetBug(id);
            var contentResult = actionResult as OkNegotiatedContentResult<BugAlert>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(id, contentResult.Content.BugId);

            BugAlert ba = contentResult.Content;
            ba.Title = "Updated Title";
            // Act
            var response = controller.Put(ba.BugId,ba);

            // Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }


        /*[TestMethod]
        public void Claim()
        {
            BugAlertController controller = new BugAlertController();
            string successMsg = "Bug Alert Assignment Record added Successfully.";
            int bugId = 20, devId = 5, assignedBy = 5;

            string result = controller.Claim(bugId, devId, assignedBy);
            Assert.IsNotNull(result);
            Assert.AreEqual(successMsg, result);
        }

        [TestMethod]
        public void UnClaim()
        {
            BugAlertController controller = new BugAlertController();
            string successMsg = "Bug Alert Assignment Record Deleted Successfully.";
            int bugId = 20, devId = 5;

            string result = controller.Unclaim(bugId, devId);
            Assert.IsNotNull(result);
            Assert.AreEqual(successMsg, result);
        }
        [TestMethod]
        public void Categories()
        {
            // Arrange
            BugAlertController controller = new BugAlertController();
            // Act
            IEnumerable<BugCategory> result = controller.Categories();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count() > 0);
        }

        [TestMethod]
        public void Resolve()
        {
            BugAlertController controller = new BugAlertController();
            string successMsg = "Bug Alert status set to Resolved Successfully.";
            int bugId = 20;

            string result = controller.Resolve(bugId, "Testing");
            Assert.IsNotNull(result);
            Assert.AreEqual(successMsg, result);
        }*/
    }
}