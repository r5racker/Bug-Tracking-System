using Bug_Tracker_Service.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Bug_Tracker_Service.Controllers
{
    public class CategoryController : ApiController
    {
        private string getDBConnectionString()
        {
            //return ConfigurationManager.ConnectionStrings["BugTrackingDatabase"].ConnectionString;
            //return @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename =| DataDirectory |\App_Data\BugTrackingDatabase.mdf; Integrated Security = True";
            /* return @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = F:\desktop_files_repo\prog\012_sem6\SOC\project_web_api\Bug-Tracking-System\Bug-Tracking-System\Bug-Tracker-Service\App_Data\BugTrackingDatabase.mdf; Integrated Security = True";*/
            return @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = D:\GITHUB_REPO\BugTracker-WebAPI\Bug-Tracking-System\Bug-Tracking-System\Bug-Tracker-Service\App_Data\BugTrackingDatabase.mdf; Integrated Security = True";
        }

        [HttpGet]
        public IHttpActionResult GetCategories()
        {
            DataSet bugCategories = new DataSet();
            List<BugCategory> bugCategoriesList = new List<BugCategory>();
            try
            {
                var connectionString = getDBConnectionString();
                SqlConnection conn = new SqlConnection(connectionString);
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = conn;
                sqlCmd.CommandText = "SELECT * from BugCategory";
                conn.Open();
                SqlDataAdapter categorySqlDataAdapter = new SqlDataAdapter(sqlCmd);
                categorySqlDataAdapter.Fill(bugCategories);
                bugCategories.Tables[0].TableName = "BugCategory";
                conn.Close();
                bugCategoriesList = (from DataRow dr in bugCategories.Tables[0].Rows
                                     select new BugCategory()
                                     {
                                         CategoryId = (int)dr["Id"],
                                         Title = (string)dr["Title"],
                                         Description = (string)dr["Description"],
                                         CreatedBy = (int)dr["CreatedBy"],
                                         AlertCount = !DBNull.Value.Equals(dr["AlertCount"]) ? (int)dr["AlertCount"] : 0,
                                         AlertCountUnresolved = !DBNull.Value.Equals(dr["AlertCountUnresolved"]) ? (int)dr["AlertCountUnresolved"] : 0
                                     }
                                 ).ToList();
            }
            catch (Exception fex)
            {
                Console.WriteLine("Error occured while retriving Bug Category Record :=> " + fex.ToString());
                return NotFound();
            }
            return Ok(bugCategoriesList);
        }
    }
}
