using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Bug_Tracker_Service.Models;
using Bug_Tracker_Service.Models.AuthModels;

namespace Bug_Tracker_Service.Controllers
{
    public class AuthController : ApiController
    {
        public string getDBConnectionString()
        {
            //return ConfigurationManager.ConnectionStrings["BugTrackingDatabase"].ConnectionString;
            //return @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename =| DataDirectory |\App_Data\BugTrackingDatabase.mdf; Integrated Security = True";
            return @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = F:\desktop_files_repo\prog\012_sem6\SOC\project_web_api\Bug-Tracking-System\Bug-Tracking-System\Bug-Tracker-Service\App_Data\BugTrackingDatabase.mdf; Integrated Security = True";
            //return @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = D:\GITHUB_REPO\BugTracker-WebAPI\Bug-Tracking-System\Bug-Tracking-System\Bug-Tracker-Service\App_Data\BugTrackingDatabase.mdf; Integrated Security = True; Connect Timeout=30";
        }

        [HttpPost]
        public IHttpActionResult Login([FromBody] AuthModel am)
        {
            string _email = am.Email;
            string _password = am.Pwd;
            Person _per = null;
            try
            {
                var connectionString = getDBConnectionString();
                SqlConnection conn = new SqlConnection(connectionString);

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT * FROM Person WHERE Email = @email AND Password = @pwd";
                cmd.Parameters.AddWithValue("@email", _email);
                cmd.Parameters.AddWithValue("@pwd", _password);
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    _per = new Person()
                    {
                        PersonId = (int)rdr[0],
                        Name = (string)rdr[1],
                        Email = (string)rdr[2],
                        Contact = (string)rdr[3],
                        Password = (string)rdr[4],
                        CreaedBy = (int)rdr[5],
                        Role = (UserRole)rdr[6]
                    };
                }
                else
                {
                    _per = new Person()
                    {
                        PersonId = -1,
                        Name = "",
                        Email = "",
                        Contact = "",
                        Password = "",
                        CreaedBy = -1,
                        Role = UserRole.Any
                    };
                    return NotFound();
                    //return "Na padi";
                }
                conn.Close();

            }
            catch (Exception fex)
            {
                Console.WriteLine("Error occured while login :=> " + fex.ToString());
                return NotFound();
                //return "Catch ma" + fex.ToString();
            }
            return Ok(_per);
            //return "Yeah";
        }
    }
}
