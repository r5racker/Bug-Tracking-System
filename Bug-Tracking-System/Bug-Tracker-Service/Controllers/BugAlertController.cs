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
    public class BugAlertController : ApiController
    {
        private string getDBConnectionString()
        {
            //return ConfigurationManager.ConnectionStrings["BugTrackingDatabase"].ConnectionString;
            //return @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename =| DataDirectory |\App_Data\BugTrackingDatabase.mdf; Integrated Security = True";
            return @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = F:\desktop_files_repo\prog\012_sem6\SOC\project_web_api\Bug-Tracking-System\Bug-Tracking-System\Bug-Tracker-Service\App_Data\BugTrackingDatabase.mdf; Integrated Security = True";
            //return @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = D:\GITHUB_REPO\BugTracker-WebAPI\Bug-Tracking-System\Bug-Tracking-System\Bug-Tracker-Service\App_Data\BugTrackingDatabase.mdf; Integrated Security = True";
        }

        // GET: api/BugAlert
        [HttpGet]
        //[ActionName("getbuglist")]
        public IHttpActionResult GetBugList([FromUri]BugAlertFilter filter, [FromUri]int personId)
        {
            //return new string[] { "value1", "value2" };
            Console.WriteLine("Inside BugAlertController.Get(): id="+personId.ToString());
            DataSet bugAlerts = new DataSet();
            List<BugAlert> bugAlertsList = new List<BugAlert>();
            var connectionString = getDBConnectionString();
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.Connection = conn;
            var cmdText = "";
            if (filter == BugAlertFilter.All)
            {
                cmdText = "SELECT * from BugAlert";
                sqlCmd.CommandText = cmdText;
            }
            else if (filter == BugAlertFilter.AllByTester)
            {
                //cmdText = "SELECT BA.Id,BA.Title,BC.Title as CategoryName,BA.CategoryId,BA.Description,BA.CreatedBy,BA.Status,BA.ResolutionDescription from BugAlert as BA,BugCategory as BC where BA.CreatedBy=@createdBy and BA.CategoryId=BC.Id";
                cmdText = "SELECT * from BugAlert as BA,BugCategory as BC where BA.CreatedBy=@createdBy and BA.CategoryId=BC.Id";
                sqlCmd.CommandText = cmdText;
                sqlCmd.Parameters.AddWithValue("@createdBy", personId);
            }
            else if (filter == BugAlertFilter.AllByDeveloper)
            {
                //cmdText = "SELECT BA.Id,BA.Title,BC.Title as CategoryName,BA.CategoryId,BA.Description,BA.CreatedBy,BA.Status,BA.ResolutionDescription from BugAlert as BA,BugAlertAssignmentTable as AT,BugCategory as BC where BA.Id = AT.BugAlertId and AT.DeveloperId=@developerId and BA.CategoryId=BC.Id";
                cmdText = "SELECT * from BugAlert as BA,BugAlertAssignmentTable as AT,BugCategory as BC where BA.Id = AT.BugAlertId and AT.DeveloperId=@developerId and BA.CategoryId=BC.Id";
                sqlCmd.CommandText = cmdText;
                sqlCmd.Parameters.AddWithValue("@developerId", personId);
            }
            else if (filter == BugAlertFilter.UnresolvedByDeveloper)
            {
                //cmdText = "SELECT BA.Id,BA.Title,BA.Description,BA.ResolutionDescription,BC.Title as CategoryName,BA.CategoryId,BA.CreatedBy,BA.Status from BugAlert as BA,BugAlertAssignmentTable as AT,BugCategory as BC where BA.Id = AT.BugAlertId and AT.DeveloperId=@developerId and BA.status!=@status and BA.CategoryId=BC.Id";
                cmdText = "SELECT * from BugAlert as BA,BugAlertAssignmentTable as AT,BugCategory as BC where BA.Id = AT.BugAlertId and AT.DeveloperId=@developerId and BA.status!=@status and BA.CategoryId=BC.Id";
                sqlCmd.CommandText = cmdText;
                sqlCmd.Parameters.AddWithValue("@developerId", personId);
                sqlCmd.Parameters.AddWithValue("@status", BugAlertStatus.Resolved);
            }
            else if (filter == BugAlertFilter.ResolvedByDeveloper)
            {
                //cmdText = "SELECT BA.Id,BA.Title,BA.CategoryId,BA.Description,BA.ResolutionDescription,BC.Title as CategoryName,BA.CreatedBy,BA.Status from BugAlert as BA,BugAlertAssignmentTable as AT,BugCategory as BC where BA.Id = AT.BugAlertId and AT.DeveloperId=@developerId and BA.status=@status and BA.CategoryId=BC.Id";
                cmdText = "SELECT * from BugAlert as BA,BugAlertAssignmentTable as AT,BugCategory as BC where BA.Id = AT.BugAlertId and AT.DeveloperId=@developerId and BA.status=@status and BA.CategoryId=BC.Id";
                sqlCmd.CommandText = cmdText;
                sqlCmd.Parameters.AddWithValue("@developerId", personId);
                sqlCmd.Parameters.AddWithValue("@status", BugAlertStatus.Resolved);
            }
            else if (filter == BugAlertFilter.UnresolvedByTester)
            {
                cmdText = "SELECT * from BugAlert where CreatedBy=@createdBy and Status!=@status";
                sqlCmd.CommandText = cmdText;
                sqlCmd.Parameters.AddWithValue("@createdBy", personId);
                sqlCmd.Parameters.AddWithValue("@status", BugAlertStatus.Resolved);
            }
            else if (filter == BugAlertFilter.AllUnresolved)
            {
                cmdText = "SELECT * from BugAlert where Status!=@status1 and Status!=@status2";
                sqlCmd.CommandText = cmdText;
                sqlCmd.Parameters.AddWithValue("@status1", BugAlertStatus.Resolved);
                sqlCmd.Parameters.AddWithValue("@status2", BugAlertStatus.UnderResolution);
            }

            try
            {
                conn.Open();
                SqlDataAdapter categorySqlDataAdapter = new SqlDataAdapter(sqlCmd);
                categorySqlDataAdapter.Fill(bugAlerts);
                bugAlerts.Tables[0].TableName = "BugAlert";
                conn.Close();
                bugAlertsList = (from DataRow dr in bugAlerts.Tables[0].Rows
                                 select new BugAlert()
                                 {
                                     BugId = (int)dr["Id"],
                                     Title = (string)dr["Title"],
                                     Description = (string)dr["Description"],
                                     CreatedBy = (int)dr["CreatedBy"],
                                     CategoryId = (int)dr["CategoryId"],
                                     Status = (BugAlertStatus)dr["Status"],
                                     ResolutionDescription = !DBNull.Value.Equals(dr["ResolutionDescription"]) ? (string)dr["ResolutionDescription"] : " ",
                                     ReportPath = !DBNull.Value.Equals(dr["ReportFilePath"]) ? (string)dr["ReportFilePath"] : "",
                                     CreatedOn = (DateTime)dr["CreatedOn"],
                                     AssignedOn = !DBNull.Value.Equals(dr["AssignedOn"]) ? (DateTime?)dr["AssignedOn"] : null,
                                     ResolvedOn = !DBNull.Value.Equals(dr["ResolvedOn"]) ? (DateTime?)dr["ResolvedOn"] : null
                                 }
                                 ).ToList();
                //alternate 
                /*for (int i = 0; i < bugAlerts.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = bugAlerts.Tables[0].Rows[i];
                    BugAlert ba = new BugAlert()
                    {
                        BugId = (int)dr["Id"],
                        Title = (string)dr["Title"],
                        Description = (string)dr["Description"],
                        CreatedBy = (int)dr["CreatedBy"],
                        CategoryId = (int)dr["CategoryId"],
                        Status = (BugAlertStatus)dr["Status"],
                        ResolutionDescription = !DBNull.Value.Equals(dr["ResolutionDescription"]) ? (string)dr["ResolutionDescription"] : " ",
                        ReportPath = !DBNull.Value.Equals(dr["ReportFilePath"]) ?(string)dr["ReportFilePath"]:"",
                        CreatedOn = (DateTime)dr["CreatedOn"],
                        AssignedOn = !DBNull.Value.Equals(dr["AssignedOn"])?(DateTime?)dr["AssignedOn"]:null,
                        ResolvedOn = !DBNull.Value.Equals(dr["ResolvedOn"])?(DateTime?)dr["ResolvedOn"]:null
                    };
                    bugAlertsList.Add(ba);
                }*/
            }
            catch (Exception fex)
            {
                Console.WriteLine("Error occured while retriving Bug Alert Record :=> " + fex.ToString());
                return NotFound();
            }
            
            return Ok(bugAlertsList);
        }

        // GET: api/BugAlert/5
        [HttpGet]
        //[ActionName("getbug")]
        public IHttpActionResult GetBug(int id)
        {
            int bugId = id;
            BugAlert bugAlert = null;
            try
            {
                var connectionString = getDBConnectionString();
                SqlConnection conn = new SqlConnection(connectionString);
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = conn;
                sqlCmd.CommandText = "SELECT * from BugAlert where Id = @Id";
                sqlCmd.Parameters.AddWithValue("@Id", bugId);
                conn.Open();
                SqlDataReader reader = sqlCmd.ExecuteReader();
                if (reader.Read())
                {
                    bugAlert = new BugAlert()
                    {
                        BugId = (int)reader[0],
                        Title = (string)reader[1],
                        Description = (string)reader[2],
                        CreatedBy = (int)reader[3],
                        CategoryId = (int)reader[4],
                        Status = (BugAlertStatus)reader[5],
                        ResolutionDescription = !DBNull.Value.Equals(reader[6]) ? (string)reader[6] : " ",
                        ReportPath = !DBNull.Value.Equals(reader[7]) ? (string)reader[7] : " ",
                        CreatedOn = (DateTime)reader[8],
                        AssignedOn = !DBNull.Value.Equals(reader[9]) ? (DateTime?)reader[9] : null,
                        ResolvedOn = !DBNull.Value.Equals(reader[10]) ? (DateTime?)reader[10] : null
                    };
                }
                conn.Close();
            }
            catch (Exception fex)
            {
                Console.WriteLine("Error occured while creating Bug Alert Record :=> " + fex.ToString());
                return NotFound();
            }
            return Ok(bugAlert);
        }

        // PUT: api/BugAlert/5
        [HttpPut]
        public HttpResponseMessage Put(int id, [FromBody]BugAlert bugAlert)
        {
            string result = "";
            bugAlert.Status = BugAlertStatus.New;
            try
            {
                var connectionString = getDBConnectionString();
                SqlConnection conn = new SqlConnection(connectionString);
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = conn;
                sqlCmd.CommandText = "UPDATE BugAlert SET Title=@title, Description=@description, CreatedBy=@createdBy, CategoryId=@categoryId, Status= @status,CreatedOn=@createdOn where Id=@id";
                sqlCmd.Parameters.AddWithValue("@title", bugAlert.Title);
                sqlCmd.Parameters.AddWithValue("@description", bugAlert.Description);
                sqlCmd.Parameters.AddWithValue("@createdBy", bugAlert.CreatedBy);
                sqlCmd.Parameters.AddWithValue("@categoryId", bugAlert.CategoryId);
                sqlCmd.Parameters.AddWithValue("@status", bugAlert.Status);
                sqlCmd.Parameters.AddWithValue("@createdOn", bugAlert.CreatedOn);
                sqlCmd.Parameters.AddWithValue("@id", bugAlert.BugId);
                conn.Open();
                sqlCmd.ExecuteNonQuery();
                conn.Close();
                result = "Bug Alert Record Updated Successfully.";

            }
            catch (Exception fex)
            {
                result = "Error occured while updating Bug Alert Record :=> " + fex.ToString();
                return Request.CreateResponse(HttpStatusCode.NotFound, result);
            }
            //return result;
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        // DELETE: api/BugAlert/5
        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            int bugAlertId = id;
            string result = "";
            try
            {
                var connectionString = getDBConnectionString();
                SqlConnection conn = new SqlConnection(connectionString);
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = conn;
                sqlCmd.CommandText = "DELETE from BugAlert where Id=@id";
                sqlCmd.Parameters.AddWithValue("@id", bugAlertId);
                conn.Open();
                sqlCmd.ExecuteNonQuery();
                conn.Close();
                result = "Bug Alert Record Deleted Successfully.";
            }
            catch (Exception fex)
            {
                result = "Error occured while deleting Bug Alert Record :=> " + fex.ToString();
                return Request.CreateResponse(HttpStatusCode.NotFound, result);
            }
            //return result;
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        // POST: api/BugAlert
        [HttpPost]
        public HttpResponseMessage Post([FromBody] BugAlert bugAlert)
        {
            string result = "";
            try
            {
                var connectionString = getDBConnectionString();
                SqlConnection conn = new SqlConnection(connectionString);

                bugAlert.CreatedOn = DateTime.Now;
                bugAlert.Status = BugAlertStatus.New;
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = conn;
                sqlCmd.CommandText = "INSERT INTO BugAlert(Title, Description, CreatedBy, CategoryId, Status,CreatedOn) Values (@title, @description, @createdBy, @categoryId, @status,@createdOn)";
                sqlCmd.Parameters.AddWithValue("@title", bugAlert.Title);
                sqlCmd.Parameters.AddWithValue("@description", bugAlert.Description);
                sqlCmd.Parameters.AddWithValue("@createdBy", bugAlert.CreatedBy);
                sqlCmd.Parameters.AddWithValue("@categoryId", bugAlert.CategoryId);
                sqlCmd.Parameters.AddWithValue("@status", bugAlert.Status);
                sqlCmd.Parameters.AddWithValue("@createdOn", bugAlert.CreatedOn);
                conn.Open();
                sqlCmd.ExecuteNonQuery();
                conn.Close();
                result = "Bug Alert Record added Successfully.";

            }
            catch (Exception fex)
            {
                result = "Error occured while creating Bug Alert Record :=> " + fex.ToString();
                return Request.CreateResponse(HttpStatusCode.NotFound, result);
            }
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
