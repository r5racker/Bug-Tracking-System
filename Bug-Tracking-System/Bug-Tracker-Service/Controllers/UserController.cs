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
    public class UserController : ApiController
    {
        private ApiResponseFactory responseFactory = new ApiResponseFactory();
        private string getDBConnectionString()
        {
            //return ConfigurationManager.ConnectionStrings["BugTrackingDatabase"].ConnectionString;
            //return @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename =| DataDirectory |\App_Data\BugTrackingDatabase.mdf; Integrated Security = True";
            return @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = F:\desktop_files_repo\prog\012_sem6\SOC\project_web_api\Bug-Tracking-System\Bug-Tracking-System\Bug-Tracker-Service\App_Data\BugTrackingDatabase.mdf; Integrated Security = True";
        }

        // GET: api/User?role=0
        public IEnumerable<Person> Get([FromUri]UserRole role)
        {
            UserRole _role = role;
            DataSet ds = new DataSet();
            List<Person> userList = new List<Person>(); 
            try
            {
                var connectionString = getDBConnectionString();
                SqlConnection conn = new SqlConnection(connectionString);

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                if (_role == UserRole.Any)
                {
                    cmd.CommandText = "SELECT * FROM Person";
                }
                else
                {
                    cmd.CommandText = "SELECT * FROM Person WHERE Role = @role";
                    cmd.Parameters.AddWithValue("@role", _role);
                }

                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                conn.Close();
                userList = (from DataRow dr in ds.Tables[0].Rows
                                 select new Person()
                                 {
                                     PersonId = (int)dr["Id"],
                                     Name = (string)dr["Name"],
                                     Email = (string)dr["Email"],
                                     Contact = (string)dr["ContactNo"],
                                     Password = "",
                                     CreaedBy = (int)dr["CreatedBy"],
                                     Role = (UserRole)dr["Role"]
                                 }
                                ).ToList();
            }
            catch (Exception fex)
            {
                Console.WriteLine("Error occured while retreiving all users :=> " + fex.ToString());
            }
            return userList;
        }

        // GET: api/User/5?role=0
        public Person Get(int id, [FromUri]UserRole role=UserRole.Any)
        {
            UserRole _role = role;
            int _id = id;
            Person _per = null;
            try
            {
                var connectionString = getDBConnectionString();
                SqlConnection conn = new SqlConnection(connectionString);

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT * FROM Person WHERE Id = @id";
                cmd.Parameters.AddWithValue("@id", _id);
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    _per = new Person()
                    {
                        PersonId = (int)rdr["Id"],
                        Name = (string)rdr["Name"],
                        Email = (string)rdr["Email"],
                        Contact = (string)rdr["ContactNo"],
                        Password = (string)rdr["Password"],
                        CreaedBy = (int)rdr["CreatedBy"],
                        Role = (UserRole)rdr["Role"]
                    };
                }
                conn.Close();

            }
            catch (Exception fex)
            {
                Console.WriteLine("Error occured while retreiving user by id :=> " + fex.ToString());

            }
            return _per;
        }

        // POST: api/User
        public string Post([FromBody]Person _person)
        {
            string result = "";
            try
            {
                var connectionString = getDBConnectionString();
                SqlConnection conn = new SqlConnection(connectionString);

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "INSERT INTO Person (Name, Email, ContactNo, Password, CreatedBy, Role) Values (@name, @email, @cnt, @pwd, @create, @role)";
                cmd.Parameters.AddWithValue("@name", _person.Name);
                cmd.Parameters.AddWithValue("@email", _person.Email);
                cmd.Parameters.AddWithValue("@cnt", _person.Contact);
                cmd.Parameters.AddWithValue("@pwd", _person.Password);
                cmd.Parameters.AddWithValue("@create", _person.CreaedBy);
                cmd.Parameters.AddWithValue("@role", _person.Role);

                SqlCommand cmdRetrieve = new SqlCommand();
                cmdRetrieve.Connection = conn;
                cmdRetrieve.CommandText = "SELECT Id FROM Person WHERE Email = @email";
                cmdRetrieve.Parameters.AddWithValue("@email", _person.Email);

                SqlCommand cmdRolebasedEntry = new SqlCommand();
                cmdRolebasedEntry.Connection = conn;
                string roleTablePara = "";
                switch (_person.Role)
                {
                    case (UserRole.Developer):
                        roleTablePara = "Developer";
                        break;
                    case (UserRole.Tester):
                        roleTablePara = "Tester";
                        break;
                    case (UserRole.Admin):
                        roleTablePara = "Admin";
                        break;

                    default:
                        break;
                }
                cmdRolebasedEntry.CommandText = "INSERT INTO " + roleTablePara + "(PersonId) Values (@personId)";


                conn.Open();
                cmd.ExecuteNonQuery();
                _person.PersonId = (int)cmdRetrieve.ExecuteScalar();
                cmdRolebasedEntry.Parameters.AddWithValue("@personId", _person.PersonId);
                cmdRolebasedEntry.ExecuteNonQuery();
                conn.Close();
                result = responseFactory.Generate(ApiResponseType.UserCreate);

            }
            catch (Exception fex)
            {
                Console.WriteLine(fex.ToString());
                result = responseFactory.Generate(ApiResponseType.UserActionError);
            }
            return result;
        }

        // PUT: api/User/5
        public string Put(int id, [FromBody]Person _person)
        {
            if (_person.PersonId <= 0)
            {
                _person.PersonId = id;
            }
            string result = "";
            try
            {
                var connectionString = getDBConnectionString();
                SqlConnection conn = new SqlConnection(connectionString);

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "UPDATE Person SET Name = @name, Email = @email, ContactNo = @cnt, Password = @pwd, CreatedBy =@create, Role = @role WHERE Id = @id";
                cmd.Parameters.AddWithValue("@id", _person.PersonId);
                cmd.Parameters.AddWithValue("@name", _person.Name);
                cmd.Parameters.AddWithValue("@email", _person.Email);
                cmd.Parameters.AddWithValue("@cnt", _person.Contact);
                cmd.Parameters.AddWithValue("@pwd", _person.Password);
                cmd.Parameters.AddWithValue("@create", _person.CreaedBy);
                cmd.Parameters.AddWithValue("@role", _person.Role);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                result = responseFactory.Generate(ApiResponseType.UserUpdate);

            }
            catch (Exception fex)
            {
                result = "Error occured while updating user :=> " + fex.ToString();
            }
            return result;
        }

        // DELETE: api/User/5
        public string Delete(int id, UserRole _role)
        {
            int _personId = id;
            string result = "";
            try
            {
                var connectionString = getDBConnectionString();
                SqlConnection conn = new SqlConnection(connectionString);

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "DELETE Person WHERE Id = @id";
                cmd.Parameters.AddWithValue("@id", _personId);

                //deleting entry from role based table
                SqlCommand cmdRolebasedEntry = new SqlCommand();
                cmdRolebasedEntry.Connection = conn;
                string roleTablePara = "";
                switch (_role)
                {
                    case (UserRole.Developer):
                        roleTablePara = "Developer";
                        break;
                    case (UserRole.Tester):
                        roleTablePara = "Tester";
                        break;
                    case (UserRole.Admin):
                        roleTablePara = "Admin";
                        break;

                    default:
                        break;
                }
                cmdRolebasedEntry.CommandText = "Delete " + roleTablePara + " WHERE PersonId=@personId";


                conn.Open();
                cmd.ExecuteNonQuery();
                //deleting from role 
                cmdRolebasedEntry.Parameters.AddWithValue("@personId", _personId);
                cmdRolebasedEntry.ExecuteNonQuery();
                conn.Close();
                result = responseFactory.Generate(ApiResponseType.UserDelete);

            }
            catch (Exception fex)
            {
                Console.WriteLine(fex.ToString());
                result = responseFactory.Generate(ApiResponseType.UserActionError);
            }
            return result;
        }
        [HttpPost]
        public Person Login([FromBody] string _email, [FromBody] string _password)
        {
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
                }
                conn.Close();

            }
            catch (Exception fex)
            {
                Console.WriteLine("Error occured while login :=> " + fex.ToString());
            }
            return _per;
        }
    }
}
