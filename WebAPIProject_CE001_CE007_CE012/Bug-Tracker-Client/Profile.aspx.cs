using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json;
using Bug_Tracker_Service.Models;

namespace Bug_Tracker_Client
{
    public partial class Profile : System.Web.UI.Page
    {
        HttpClient client = new HttpClient();
        protected void Page_Load(object sender, EventArgs e)
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.BaseAddress = new Uri("https://localhost:44353/");
            //name.Text = (string)Session["p_id"];
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            UserRole uRole = UserRole.Any;
            switch (role.SelectedValue.ToString())
            {
                case ("admin"):
                    uRole = UserRole.Admin;
                    break;
                case ("dev"):
                    uRole = UserRole.Developer;
                    break;
                case ("tester"):
                    uRole = UserRole.Tester;
                    break;
            }

            Person _per = new Person()
            {
                PersonId = Int32.Parse(perId.Text.ToString()),
                Name = name.Text.ToString(),
                Email = email.Text.ToString(),
                Contact = contact.Text.ToString(),
                Password = password.Text.ToString(),
                CreaedBy = Int32.Parse(creater.Text.ToString()),
                Role = uRole
            };
            string url = "api/user/" + (string)Session["p_id"];
            var res = client.PutAsJsonAsync(url, _per);
            res.Wait();
            var res_data = res.Result;
            //var data_async = res_data.Content.ReadAsStringAsync();
            //data_async.Wait();
            //var data = data_async.Result;
            if (! res_data.IsSuccessStatusCode)
            {
                errorLabel.Text = "Problem";
                errorLabel.Visible = true;
            }
            else
            {
                errorLabel.Visible = false;
                successLabel.Visible = true;
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            UserRole uRole = UserRole.Any;
            switch (role.SelectedValue.ToString())
            {
                case ("admin"):
                    uRole = UserRole.Admin;
                    break;
                case ("dev"):
                    uRole = UserRole.Developer;
                    break;
                case ("tester"):
                    uRole = UserRole.Tester;
                    break;
            }

            string url = "api/user/" + (string)Session["p_id"];
            var res = client.DeleteAsync(url);
            res.Wait();
            var res_data = res.Result;
            //var data_async = res_data.Content.ReadAsStringAsync();
            //data_async.Wait();
            //var data = data_async.Result;
            if (res_data.ToString().Contains("Error"))
            {
                errorLabel.Text = "Problem";
                errorLabel.Visible = true;
            }
            else
            {
                errorLabel.Visible = false;
                successLabel.Visible = true;
                Response.Redirect("~/Login");
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            UserRole uRole = UserRole.Any;
            switch ((string)Session["p_role"])
            {
                case ("admin"):
                    uRole = UserRole.Admin;
                    break;
                case ("dev"):
                    uRole = UserRole.Developer;
                    break;
                case ("tester"):
                    uRole = UserRole.Tester;
                    break;
            }

            string url = "api/user";
            string idStr = ((string)Session["p_id"]);
            var res = client.GetAsync(url + "?id=" + idStr.ToString());
            res.Wait();
            var data = res.Result;
            Person _per = null;
            var dataasync = data.Content.ReadAsAsync<Person>();
            dataasync.Wait();
            _per = dataasync.Result;
            if(_per == null || _per.PersonId == -1)
            {
                errorLabel.Text = "Invalid Credentials";
                errorLabel.Visible = true;
            }

            perId.Text = _per.PersonId.ToString();
            email.Text = _per.Email.ToString();
            name.Text = _per.Name.ToString();
            switch (_per.Role)
            {
                case (UserRole.Admin):
                    role.SelectedValue = "admin";
                    break;
                case (UserRole.Developer):
                    role.SelectedValue = "dev";
                    break;
                case (UserRole.Tester):
                    role.SelectedValue = "tester";
                    break;
            }
            contact.Text = _per.Contact.ToString();
            creater.Text = _per.CreaedBy.ToString();
        }
    }
}