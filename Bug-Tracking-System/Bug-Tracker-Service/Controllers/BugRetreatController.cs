using Bug_Tracker_Service.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Bug_Tracker_Service.Controllers
{
    public class BugRetreatController : ApiController
    {
        private string getDBConnectionString()
        {
            //return ConfigurationManager.ConnectionStrings["BugTrackingDatabase"].ConnectionString;
            //return @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename =| DataDirectory |\App_Data\BugTrackingDatabase.mdf; Integrated Security = True";
            /* return @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = F:\desktop_files_repo\prog\012_sem6\SOC\project_web_api\Bug-Tracking-System\Bug-Tracking-System\Bug-Tracker-Service\App_Data\BugTrackingDatabase.mdf; Integrated Security = True";*/
            return @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = D:\GITHUB_REPO\BugTracker-WebAPI\Bug-Tracking-System\Bug-Tracking-System\Bug-Tracker-Service\App_Data\BugTrackingDatabase.mdf; Integrated Security = True";
        }

        [HttpPost]
        public HttpResponseMessage Post([FromBody] StatusChangeModel sm)
        {
            int bugId = sm.id;
            int developerId = sm.developerId;
            string result = "";
            try
            {
                var connectionString = getDBConnectionString();
                SqlConnection conn = new SqlConnection(connectionString);
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = conn;
                sqlCmd.CommandText = "DELETE from BugAlertAssignmentTable where BugAlertId=@bugAlertId and DeveloperId=@developerId";
                sqlCmd.Parameters.AddWithValue("@bugAlertId", bugId);
                sqlCmd.Parameters.AddWithValue("@developerId", developerId);

                SqlCommand sqlCmd2 = new SqlCommand();
                sqlCmd2.Connection = conn;
                sqlCmd2.CommandText = "UPDATE BugAlert SET status=@status where Id=@id";
                sqlCmd2.Parameters.AddWithValue("@status", BugAlertStatus.Abandoned);
                sqlCmd2.Parameters.AddWithValue("@id", bugId);

                conn.Open();
                sqlCmd.ExecuteNonQuery();
                sqlCmd2.ExecuteNonQuery();
                conn.Close();
                result = "Bug Alert Assignment Record Deleted Successfully.";
            }
            catch (Exception fex)
            {
                result = "Error occured while deleting Bug Alert Assignment Record :=> " + fex.ToString();
                return Request.CreateResponse(HttpStatusCode.NotFound, result);
            }
            //return result;
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
