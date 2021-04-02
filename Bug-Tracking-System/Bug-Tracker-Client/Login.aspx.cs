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
using Bug_Tracker_Service.Models.AuthModels;

namespace Bug_Tracker_Client
{
    public partial class Login : System.Web.UI.Page
    {
        HttpClient client = new HttpClient();
        protected void Page_Load(object sender, EventArgs e)
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.BaseAddress = new Uri("https://localhost:44353/");
            errorLabel.Visible = false;
        }

        protected void btnLogin_Click(object sender, EventArgs e) {
            string url = "api/auth";
            var personModel = new AuthModel();
            personModel.Email = email.Text.ToString();
            personModel.Pwd = password.Text.ToString();
            var result = client.PostAsJsonAsync(url, personModel);
            result.Wait();
            var data = result.Result;
            if(!data.IsSuccessStatusCode)
            {
                errorLabel.Text = "Status code : unsuccess";
                errorLabel.Visible = true;
                return;
            }

            var model = data.Content.ReadAsAsync<Person>();
            model.Wait();
            Person _per = model.Result;
            if (_per == null || _per.PersonId == -1 )
            {
                errorLabel.Text = "Invalid Credentials";
                errorLabel.Visible = true;
            }
            else
            {
                errorLabel.Visible = false;
                Session["p_email"] = _per.Email.ToString();
                Session["p_id"] = _per.PersonId.ToString();
                Session["p_name"] = _per.Name.ToString();
                switch (_per.Role)
                {
                    case (UserRole.Admin):
                        Session["p_role"] = "admin";
                        break;
                    case (UserRole.Developer):
                        Session["p_role"] = "dev";
                        break;
                    case (UserRole.Tester):
                        Session["p_role"] = "tester";
                        break;
                }
                Response.Redirect("~/Profile");

            }
        }
    }
}