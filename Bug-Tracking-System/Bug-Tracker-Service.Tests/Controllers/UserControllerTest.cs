using Bug_Tracker_Service.Controllers;
using Bug_Tracker_Service.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
namespace Bug_Tracker_Service.Tests.Controllers
{
    [TestClass]
    public class UserControllerTest
    {
        ApiResponseFactory responseFactory = new ApiResponseFactory();
        [TestMethod]
        public void Get()
        {
            // Arrange
            UserController controller = new UserController();
            // Act
            IEnumerable<Person> result = controller.Get(UserRole.Any);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count() > 0);
        }

        [TestMethod]
        public void GetById()
        {
            // Arrange
            UserController controller = new UserController();
            // Act
            int userId = 5;
            Person result = controller.Get(userId,UserRole.Any);

            // Assert
            Console.WriteLine(result.Name);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.PersonId,userId);
        }

        [TestMethod]
        public void Post()
        {
            UserController controller = new UserController();
            string successMsg = responseFactory.Generate(ApiResponseType.UserCreate);
            string errorMsgPrefix = responseFactory.Generate(ApiResponseType.UserActionError);
            Person per = new Person()
            {
                Name = "demo",
                Role = UserRole.Admin,
                Email = "demo1@demo.com",
                Contact = "1234567890",
                CreaedBy = 5,
                Password = "123456"
            };
            string result = controller.Post(per);
            Assert.IsNotNull(result);
            Assert.AreEqual(successMsg, result);
        }


        [TestMethod]
        public void Put()
        {
            UserController controller = new UserController();
            int userId = 32;
            string successMsg = responseFactory.Generate(ApiResponseType.UserUpdate);
            string errorMsgPrefix = responseFactory.Generate(ApiResponseType.UserActionError);
            Person per = controller.Get(userId, UserRole.Any);
            per.Name = "UpdatedName";
            string result = controller.Put(userId, per);
            Assert.IsNotNull(result);
            Assert.AreEqual(successMsg, result);
        }

        [TestMethod]
        public void Login()
        {
            UserController controller = new UserController();
            string email = "demo@demo.com", password = "123456";
            Person per = controller.Login(email, password);
            Assert.IsNotNull(per);
            Assert.IsTrue(per.Email.StartsWith(email));
        }




        [TestMethod]
        public void Delete()
        {
            UserController controller = new UserController();
            string successMsg = responseFactory.Generate(ApiResponseType.UserDelete);
            int userId = 38;
            string result = controller.Delete(userId, UserRole.Admin);
            Assert.IsNotNull(result);
            Assert.AreEqual(successMsg, result);
        }
    }
}
