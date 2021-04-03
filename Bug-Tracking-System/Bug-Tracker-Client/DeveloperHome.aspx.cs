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
    public partial class DeveloperHome : System.Web.UI.Page
    {
        HttpClient client = new HttpClient();
        int personId, bugAlertId;

        protected void Page_Load(object sender, EventArgs e)
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.BaseAddress = new Uri("https://localhost:44353/");

            personId = getPersonId();
            ViewState["personId"] = personId;

            var url = "api/bugalert?filter=" + BugAlertFilter.UnresolvedByDeveloper + "&personId=" + personId.ToString();
            IEnumerable<BugAlert> unResByDev = null;
            //List<string> data = new List<string>();
            var res = client.GetAsync(url);
            res.Wait();
            var dataread = res.Result;
            if (dataread.IsSuccessStatusCode)
            {
                var data = dataread.Content.ReadAsAsync<IList<BugAlert>>();
                data.Wait();
                unResByDev = data.Result;

                if (unResByDev.ToList().Count > 0 && !IsPostBack)
                {
                    BugAlert ba = unResByDev.ToList()[0];
                    BugIdLable.Text = ba.BugId.ToString();
                    bugTitle.Text = ba.Title.ToString();
                    status.Text = Enum.GetName(typeof(BugAlertStatus), ba.Status);
                    description.Text = ba.Description.ToString();
                    category.Text = ba.CategoryId.ToString();
                    resolutionDescription.Text = ba.ResolutionDescription.ToString();
                }
                else
                {
                    /*show message*/
                    bugTitle.Text = "-";
                    mydisplay.Text = "Visit the Unresolved Bugs Page to claim some new bugs";
                    mydisplay.ForeColor = System.Drawing.Color.Red;
                }

            }


            
        }

        protected int getPersonId()
        {
            int pId = 0;
            if (Session["p_id"] != null)
            {
                pId = int.Parse((string)Session["p_id"]);
            }
            return pId;
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            if (BugIdLable.Text != "-")
            {
                //string rDescription = resolutionDescription.Text.ToString();
                //mydisplay.Text = rDescription.ToString() + " successfully done . ";
                personId = (int)ViewState["personId"];
                bugAlertId = int.Parse(BugIdLable.Text);

                StatusChangeModel sm = new StatusChangeModel();
                sm.id = bugAlertId;
                sm.developerId = personId;

                string url = "api/bugretreat";
                var res = client.PostAsJsonAsync(url, sm);
                res.Wait();
                var res_data = res.Result;

                if (!res_data.IsSuccessStatusCode)
                {
                    errorLabel.Text = "Problem in retreating bug alert.";
                    errorLabel.Visible = true;
                }

                Response.Redirect("DeveloperHome.aspx");
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (BugIdLable.Text != "-")
            {
                string rDescription = resolutionDescription.Text.ToString();
                mydisplay.Text = rDescription.ToString() + " successfully done . ";
                personId = (int)ViewState["personId"];
                bugAlertId = int.Parse(BugIdLable.Text);

                StatusChangeModel sm = new StatusChangeModel();
                sm.id = bugAlertId;
                sm.bugAlertResolutionDescription = resolutionDescription.Text;

                string url = "api/bugresolve";
                var res = client.PostAsJsonAsync(url, sm);
                res.Wait();
                var res_data = res.Result;

                if (!res_data.IsSuccessStatusCode)
                {
                    errorLabel.Text = "Problem in resolving bug alert.";
                    errorLabel.Visible = true;
                }

                Response.Redirect("DeveloperHome.aspx");
            }

        }
    }
}