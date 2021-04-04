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
    public partial class TesterHome : System.Web.UI.Page
    {
        HttpClient client = new HttpClient();
        int personId;
        string bugId;

        protected void Page_Load(object sender, EventArgs e)
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.BaseAddress = new Uri("https://localhost:44353/");

            if (!IsPostBack)
            {
                personId = getPersonId();
                ViewState["personId"] = personId;

                var url = "api/bugalert?filter=" + (int)BugAlertFilter.AllByTester + "&personId=" + personId.ToString();
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
                else
                {
                    //errorLabel.Text = "Problem";
                    errorLabel.Text = url;
                    errorLabel.Visible = true;
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

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["bugId"] = GridView1.SelectedRow.Cells[1].Text;
        }



        protected void Button2_Click(object sender, EventArgs e)
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

        protected void Button3_Click(object sender, EventArgs e)
        {
            if (ViewState["bugId"] != null)
            {
                bugId = ViewState["bugId"].ToString();

                string url = "api/bugalert/" + bugId;
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
                    //successLabel.Visible = true;
                    Response.Redirect("~/TesterHome");
                }

                //Response.Redirect("TesterHome");
            }
            else
            {
                DisplayLabel.Text = "Please select a bug to Delete";
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("NewBugAlert.aspx");
        }
    }
}