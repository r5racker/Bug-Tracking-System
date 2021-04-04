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
using System.Data;

namespace Bug_Tracker_Client
{
    public partial class NewBugAlert : System.Web.UI.Page
    {
        HttpClient client = new HttpClient();
        protected void Page_Load(object sender, EventArgs e)
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.BaseAddress = new Uri("https://localhost:44353/");

            //DataSet categoriesDataSet;

            var url = "api/category";
            IEnumerable<BugCategory> bcatlist = null;
            //List<string> data = new List<string>();
            var res = client.GetAsync(url);
            res.Wait();
            var dataread = res.Result;
            if (dataread.IsSuccessStatusCode)
            {
                var data = dataread.Content.ReadAsAsync<IList<BugCategory>>();
                data.Wait();
                bcatlist = data.Result;
                //category.DataSource = bcatlist.ToList();
                //category.DataBind();
            }


            category.Items.Add(new ListItem("Select a item", ""));
            foreach (BugCategory cat in bcatlist.ToList<BugCategory>())
            {
                category.Items.Add(new ListItem(cat.Title, cat.CategoryId.ToString()));
            }
            category.AppendDataBoundItems = true;
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
            BugAlert bugAlert = new BugAlert();
            bugAlert.Title = bugTitle.Text;
            bugAlert.CategoryId = int.Parse(category.SelectedValue);
            bugAlert.Description = description.Text;
            bugAlert.CreatedBy = getPersonId();

            string url = "api/bugalert";
            var res = client.PostAsJsonAsync(url, bugAlert);
            res.Wait();
            var res_data = res.Result;

            if (!res_data.IsSuccessStatusCode)
            {
                errorLabel.Text = "Problem in creating new bug alert.";
                errorLabel.Visible = true;
            }

            Response.Redirect("TesterHome");
        }
    }
}