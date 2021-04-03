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
    public partial class BugDetail : System.Web.UI.Page
    {
        HttpClient client = new HttpClient();
        protected void Page_Load(object sender, EventArgs e)
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.BaseAddress = new Uri("https://localhost:44353/");

            int bugID = int.Parse(Request.QueryString["bugId"]);
            if (bugID > 0)
            {
                string url = "api/bugalert/" + bugID.ToString();
                var res = client.GetAsync(url);
                res.Wait();
                var data = res.Result;
                BugAlert bugAlert = null;
                var dataasync = data.Content.ReadAsAsync<BugAlert>();
                dataasync.Wait();
                bugAlert = dataasync.Result;

                if(bugAlert == null || bugAlert.BugId == -1)
                {
                    errorLabel.Text = "Invalid Credentials";
                    errorLabel.Visible = true;
                    return;
                }

                bugId.Text = bugID.ToString();
                bugTitle.Text = bugAlert.Title;
                description.Text = bugAlert.Description;
                status.Text = bugAlert.Status.ToString();
                category.Text = bugAlert.CategoryId.ToString();
                resolutionDescription.Text = bugAlert.ResolutionDescription;
            }
        }
    }
}