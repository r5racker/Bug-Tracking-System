using Bug_Tracker_Service;
using Bug_Tracker_Service.Controllers;
using Bug_Tracker_Service.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Bug_Tracker_Service.Tests.Controllers
{
    [TestClass]
    public class BugAlertControllerTest
    {
        [TestMethod]
        public void Get()
        {
            // Arrange
            BugAlertController controller = new BugAlertController();
            int personId = 30;
            // Act
            IEnumerable<BugAlert> result = controller.Get(Models.BugAlertFilter.All, personId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count() > 0);
        }
        [TestMethod]
        public void GetById()
        {
            // Arrange
            BugAlertController controller = new BugAlertController();
            int Id = 14;
            // Act
            BugAlert result = controller.GetById(Id);

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Post()
        {
            BugAlertController controller = new BugAlertController();
            string successMsg = "Bug Alert Record added Successfully.";
            string errorMsgPrefix = "Error occured";
            BugAlert ba = new BugAlert() {
                CategoryId = 1,
                Description = "Testing bug post on" + DateTime.Now.ToString(),
                Title = "Testing",
                CreatedBy = 5
            };
            string result = controller.Post(ba);
            Assert.IsNotNull(result);
            Assert.AreEqual(successMsg, result);
        }

        [TestMethod]
        public void Delete()
        {
            BugAlertController controller = new BugAlertController();
            string successMsg = "Bug Alert Record Deleted Successfully.";
            int bugId = 19;
            string result = controller.Delete(bugId);
            Assert.IsNotNull(result);
            Assert.AreEqual(successMsg, result);
        }
        [TestMethod]
        public void Claim()
        {
            BugAlertController controller = new BugAlertController();
            string successMsg = "Bug Alert Assignment Record added Successfully.";
            int bugId = 20,devId=5,assignedBy=5;

            string result = controller.Claim(bugId,devId,assignedBy);
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

            string result = controller.Resolve(bugId,"Testing");
            Assert.IsNotNull(result);
            Assert.AreEqual(successMsg, result);
        }
    }
}