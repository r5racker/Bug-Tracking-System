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
    public partial class Registration : System.Web.UI.Page
    {
		HttpClient client = new HttpClient();
		protected void Page_Load(object sender, EventArgs e)
        {
            if ((string)Session["p_role"] != "admin")
            {
                notAdminErrorLabel.Visible = true;
                regPanel.Visible = false;
            }
            else
            {
                notAdminErrorLabel.Visible = false;
                regPanel.Visible = true;
            }
            errorLabel.Visible = false;
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			client.BaseAddress = new Uri("https://localhost:44353/");
		}

		protected void btnRegister_Click(object sender, EventArgs e)
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
				PersonId = -1,
				Name = name.Text.ToString(),
				Email = email.Text.ToString(),
				Contact = contact.Text.ToString(),
				Password = password.Text.ToString(),
				CreaedBy = Convert.ToInt32((string)Session["p_id"]),
				Role = uRole
			};


			string url = "api/user";
			var res = client.PostAsJsonAsync(url, _per);
			res.Wait();
			var res_data = res.Result;
			//var data_async = res_data.Content.ReadAsStringAsync();
			//data_async.Wait();
			//var data = data_async.Result;
			if (!res_data.IsSuccessStatusCode)
			{
				errorLabel.Text = "Problem";
				errorLabel.Visible = true;
			}
			else
			{
				errorLabel.Visible = false;
				successLabel.Visible = true;
				btnNewReg.Visible = true;
			}
		}

		protected void btnNewReg_Click(object sender, EventArgs e)
		{
			Response.Redirect("~/Registration");
		}

		protected void btnGoProfile_Click(object sender, EventArgs e)
		{
			Response.Redirect("~/Profile");
		}
	}
}