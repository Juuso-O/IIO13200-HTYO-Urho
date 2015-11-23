using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class own : System.Web.UI.Page
{
    G7934Entities ctx;

    public class GridViewClassA
    {
        public string Nimi { get; set; }
        public DateTime Pvm { get; set; }
        public string Laji { get; set; }
        public int Kesto { get; set; }
    }
    public class GridViewClassB
    {
        public string Nimi { get; set; }
        public DateTime Pvm { get; set; }
        public string Laji { get; set; }
        public int Kesto { get; set; }
        public int Id { get; set; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["name"] != null)
        {
            string nameForWhere = Request.QueryString["name"];
            ctx = new G7934Entities();
            var results = from c in ctx.Accoplishments
                          join a in ctx.People on c.Person equals a.idPerson
                          join b in ctx.Sports on c.Sport equals b.idSport
                          where a.Name == nameForWhere
                          select new GridViewClassB
                          {
                              Nimi = a.Name,
                              Pvm = c.Date,
                              Laji = b.Name,
                              Kesto = c.Duration,
                              Id = c.idAccoplishmnet
                          };
            
            gvData.DataSource = results.ToList();
            gvData.DataBind();
            gvData.Columns[0].Visible = false;

            lblUser.Text = "Näytetään henkilön " + nameForWhere + " tulokset";
        }
        else if (Session["name"] != null)
        {
            string nameForWhere = Session["name"].ToString();
            ctx = new G7934Entities();
            var results = from c in ctx.Accoplishments
                          join a in ctx.People on c.Person equals a.idPerson
                          join b in ctx.Sports on c.Sport equals b.idSport
                          where a.Name == nameForWhere
                          select new GridViewClassB
                          {
                              Nimi = a.Name,
                              Pvm = c.Date,
                              Laji = b.Name,
                              Kesto = c.Duration,
                              Id = c.idAccoplishmnet
                          };
            
            gvData.DataSource = results.ToList();
            gvData.DataBind();
            gvData.Columns[1].Visible = false;

            lblUser.Text = "Valittuna: " + nameForWhere;
            btnLogOut.Visible = true;
        }
        else
        {
            lblUser.Text = "Valitse kuka olet";
            ddlUser.Visible = true;
            if(!IsPostBack)
            {
                ctx = new G7934Entities();
                var results = from c in ctx.People
                              select c.Name;

                List<string> list = new List<string>();
                list.Add("");
                foreach (var i in results)
                {
                    list.Add(i);
                }
                ddlUser.DataSource = list;
                ddlUser.DataBind();
            }
        }
    }

    protected void ddlUser_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["name"] = ddlUser.SelectedItem.Text;
        Response.Redirect(Request.RawUrl);
    }

    protected void btnLogOut_Click(object sender, EventArgs e)
    {
        Session["name"] = null;
        Response.Redirect(Request.RawUrl);
    }
}