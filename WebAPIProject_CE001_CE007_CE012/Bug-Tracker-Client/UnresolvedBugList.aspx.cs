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
    public partial class UnresolvedBugList : System.Web.UI.Page
    {
        HttpClient client = new HttpClient();
        string bugId;
        int personId;

        protected void Page_Load(object sender, EventArgs e)
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.BaseAddress = new Uri("https://localhost:44353/");

            if (!IsPostBack)
            {
                personId = getPersonId();
                ViewState["personId"] = personId;

                var url = "api/bugalert?filter=" + BugAlertFilter.AllUnresolved + "&personId=" + personId.ToString();
                IEnumerable<BugAlert> allBugByTester = null;
                //List<string> data = new List<string>();
                var res = client.GetAsync(url);
                res.Wait();
                var dataread = res.Result;
                if (dataread.IsSuccessStatusCode)
                {
                    var data = dataread.Content.ReadAsAsync<IList<BugAlert>>();
                    data.Wait();
                    allBugByTester = data.Result;
                    GridView1.DataSource = allBugByTester.ToList();
                    GridView1.DataBind();
                }
            }
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GridView1.SelectedRow != null)
            {
                bugId = GridView1.SelectedRow.Cells[1].Text;
                ViewState["bugId"] = bugId;
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

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (ViewState["bugId"] != null)
            {
                bugId = ViewState["bugId"].ToString();
                Response.Redirect("BugDetail.aspx?bugId=" + bugId);
            }
            else
            {
                DisplayLabel.Text = "Please select a bug to View";
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            if (ViewState["bugId"] != null)
            {
                bugId = ViewState["bugId"].ToString();
                personId = getPersonId();

                StatusChangeModel sm = new StatusChangeModel();
                sm.id = int.Parse(bugId);
                sm.developerId = personId;
                sm.assignedBy = personId;

                string url = "api/bugclaim";
                var res = client.PostAsJsonAsync(url, sm);
                res.Wait();
                var res_data = res.Result;

                if (!res_data.IsSuccessStatusCode)
                {
                    errorLabel.Text = "Problem in resolving bug alert.";
                    errorLabel.Visible = true;
                }

                Response.Redirect("DeveloperHome");
            }
            else
            {
                DisplayLabel.Text = "Please select a bug to Claim";
            }
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            personId = getPersonId();
            ViewState["personId"] = personId;

            var url = "api/bugalert?filter=" + BugAlertFilter.ResolvedByDeveloper + "&personId=" + personId.ToString();
            IEnumerable<BugAlert> allBugByTester = null;
            //List<string> data = new List<string>();
            var res = client.GetAsync(url);
            res.Wait();
            var dataread = res.Result;
            if (dataread.IsSuccessStatusCode)
            {
                var data = dataread.Content.ReadAsAsync<IList<BugAlert>>();
                data.Wait();
                allBugByTester = data.Result;
                GridView1.DataSource = allBugByTester.ToList();
                GridView1.DataBind();
            }
            TableHeading.Text = "History of Resolved Bug Alerts";
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            personId = getPersonId();
            ViewState["personId"] = personId;

            var url = "api/bugalert?filter=" + BugAlertFilter.AllUnresolved + "&personId=" + personId.ToString();
            IEnumerable<BugAlert> allBugByTester = null;
            //List<string> data = new List<string>();
            var res = client.GetAsync(url);
            res.Wait();
            var dataread = res.Result;
            if (dataread.IsSuccessStatusCode)
            {
                var data = dataread.Content.ReadAsAsync<IList<BugAlert>>();
                data.Wait();
                allBugByTester = data.Result;
                GridView1.DataSource = allBugByTester.ToList();
                GridView1.DataBind();
            }
            TableHeading.Text = "Unresolved Bug Alerts";
        }
    }
}