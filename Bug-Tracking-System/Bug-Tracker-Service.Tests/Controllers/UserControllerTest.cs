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
using System.Web.Http.Results;

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
            string id = "31";
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            IHttpActionResult actionResult = controller.Get(id);
            var contentResult = actionResult as OkNegotiatedContentResult<Person>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(id, contentResult.Content.PersonId.ToString());
        }
/*
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
        }*/

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
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            var response = controller.Post(per);

            // Assert
           
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }


        [TestMethod]
        public void Put()
        {
            UserController controller = new UserController();
            string successMsg = responseFactory.Generate(ApiResponseType.UserUpdate);
            string errorMsgPrefix = responseFactory.Generate(ApiResponseType.UserActionError);
            Person per;
            string id = "31";
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            IHttpActionResult actionResult = controller.Get(id);
            var contentResult = actionResult as OkNegotiatedContentResult<Person>;
            per = contentResult.Content;
            per.Name = "UpdatedName";
            var response = controller.Put(Int32.Parse(id), per);
            // Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }
/*
        [TestMethod]
        public void Login()
        {
            UserController controller = new UserController();
            string email = "demo@demo.com", password = "123456";
            Person per = controller.Login(email, password);
            Assert.IsNotNull(per);
            Assert.IsTrue(per.Email.StartsWith(email));
        }*/




        [TestMethod]
        public void Delete()
        {
            UserController controller = new UserController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            string successMsg = responseFactory.Generate(ApiResponseType.UserDelete);
            int userId = 39;
            var response = controller.DeletePerson(userId, UserRole.Admin);
            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }
    }
}
