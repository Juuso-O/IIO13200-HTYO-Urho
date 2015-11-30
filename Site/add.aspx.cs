using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;

public partial class add : System.Web.UI.Page
{
    G7934Entities ctx;
    private string addNewSport = "Lisää uusi laji";
    bool AllowNewUsersAndSports = Boolean.Parse(WebConfigurationManager.AppSettings["AllowNewUsersAndSports"]);

    protected void Page_Load(object sender, EventArgs e)
    {
        ChangeNameEntry(AllowNewUsersAndSports);
        if (!IsPostBack)
        {
            FillDateAndSports();
            lblMessage.Visible = false;
            if (Request.QueryString["id"] != null)
            {
                GetAccoplishment();
                ChangeNameEntry(true);
                txtName.ReadOnly = true;
            }
        }
    }

    protected void ChangeNameEntry(bool b)
    {
        if (!b)
        {
            divEditableName.Visible = false;
            divUnEditableName.Visible = true;
        }
        else
        {
            divEditableName.Visible = true;
            divUnEditableName.Visible = false;
        }
    }

    protected void GetAccoplishment()
    {
        ctx = new G7934Entities();
        int id = int.Parse(Request.QueryString["id"]);
        var result = from c in ctx.Accoplishments
                      join a in ctx.People on c.Person equals a.idPerson
                      join b in ctx.Sports on c.Sport equals b.idSport
                      where c.idAccoplishmnet == id
                      select new
                      {
                          name = a.Name,
                          date = c.Date,
                          sport = b.Name,
                          duration = c.Duration
                      };
        foreach(var a in result)
        {
            txtName.Text = a.name;
            ddlSport.SelectedValue = a.sport;
            ddlDate.SelectedValue = a.date.ToString();
            txtDuration.Text = a.duration.ToString();
        }
    }

    protected void FillDateAndSports()
    {
        // New entity of database
        ctx = new G7934Entities();
        
        // Collecting names from sports and setting them
        // as Sport dropDownLists datasource
        var sports = from c in ctx.Sports
                     select c.Name;
        List<string> sportList = sports.ToList();

        if (AllowNewUsersAndSports)
        {
            sportList.Add(addNewSport);
        }

        ddlSport.DataSource = sportList;
        ddlSport.DataBind();

        // Generating 14 dates back from today and setting
        // them as datasource to datelist
        List<String> dates = new List<string>();
        //List<DateTime> dates = new List<DateTime>();
        DateTime date = DateTime.Today;
        dates.Add(date.ToString("dd.MM.yyyy"));
        for (int i = 0; i < 13; i++)
        {
            date = date.AddDays(-1);
            dates.Add(date.ToString("dd.MM.yyyy"));
        }
        ddlDate.DataSource = dates;
        ddlDate.DataBind();

        if (!AllowNewUsersAndSports)
        {
            var people = from c in ctx.People
                         select c.Name;
            ddlName.DataSource = people.ToList();
            ddlName.DataBind();
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        // New entity of database
        ctx = new G7934Entities();

        //AllowNewUsersAndSports

        if (Request.QueryString["id"] != null)
        {
            int id = int.Parse(Request.QueryString["id"]);
            var result = from c in ctx.Accoplishments
                         where c.idAccoplishmnet == id
                         select c;
            var s = from c in ctx.Sports
                    where c.Name == ddlSport.SelectedValue
                    select c;
            int sportId = 0;
            foreach (var i in s)
            {
                sportId = i.idSport;
            }
            foreach (var a in result)
            {
                a.Date = DateTime.Parse(ddlDate.SelectedValue);
                a.Duration = int.Parse(txtDuration.Text);
                a.Sport = sportId;
            }

            ctx.SaveChanges();

            lblMessage.Text = "Muutos tallennettu";
            lblMessage.Visible = true;
        }
        else
        {
            string name = "";
            if (AllowNewUsersAndSports)
            {
                name = txtName.Text;
            }
            else
            {
                name = ddlName.SelectedValue;
            }
            var person = from c in ctx.People
                    where c.Name == name
                    select c.idPerson;
            int personId = 0;
            foreach (var i in person)
            {
                personId = i;
            }
            var results = from c in ctx.Accoplishments
                         where c.Person == personId
                         select c;
            int counter = 0;
            foreach (var i in results)
            {
                if (i.Date.ToString().Equals(ddlDate.SelectedValue)) {
                    counter++;
                }
            }

            if (counter > 4)
            {
                lblMessage.Text = "Sinulla on tällä päivällä jo " + counter + " suoritusta, et voi lisätä enempää.";
                lblMessage.Visible = true;
                btnAdd.Visible = false;
                btnConfirmedAdd.Visible = false;
            }
            else if (counter > 0)
            {
                btnAdd.Visible = false;
                btnConfirmedAdd.Visible = true;
                lblMessage.Text = "Sinulla on tällä päivällä jo " + counter.ToString() + " suoritusta, lisätäänkö uusi?";
                lblMessage.Visible = true;
            }
            else
            {
                AddNew();
            }
        }

        
    }

    protected void btnConfirmedAdd_Click(object sender, EventArgs e)
    {
        ctx = new G7934Entities();
        AddNew();
    }

    private void AddNew()
    {
        // New accomplishment (urheilusuoritus) to be added
        Accoplishment nA = new Accoplishment();

        string name = "";
        if (AllowNewUsersAndSports)
        {
            name = txtName.Text;
        }
        else
        {
            name = ddlName.SelectedValue;
        }
        var p = from c in ctx.People
                select c;
        bool found = false;
        foreach (var i in p)
        {
            if (i.Name == name)
            {
                found = true;
                nA.Person = i.idPerson;
            }
        }
        if (!found)
        {
            Person pe = new Person();
            pe.Name = txtName.Text;
            ctx.People.Add(pe);
            ctx.SaveChanges();
            nA.Person = pe.idPerson;
        }

        var s = from c in ctx.Sports
                select c;
        foreach (var i in s)
        {
            if (i.Name == ddlSport.SelectedValue)
            {
                nA.Sport = i.idSport;
            }
        }

        nA.Date = DateTime.Parse(ddlDate.SelectedValue);
        nA.Duration = int.Parse(txtDuration.Text);
        ctx.Accoplishments.Add(nA);

        ctx.SaveChanges();

        txtName.Text = "";
        txtDuration.Text = "";
        ddlDate.SelectedIndex = 0;
        ddlSport.SelectedIndex = 0;

        lblMessage.Text = "Suoritus tallennettu";
        lblMessage.Visible = true;
        btnAdd.Visible = false;
        btnConfirmedAdd.Visible = false;
    }

    protected void ddlSport_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSport.SelectedValue.Equals(addNewSport))
        {
            //txtNewSport.Visible = true;
            //btnAddNewSport.Visible = true;
            //lblNewSportName.Visible = true;
            trNewSport.Visible = true;
        }
        else
        {
            //txtNewSport.Visible = false;
            //btnAddNewSport.Visible = false;
            //lblNewSportName.Visible = false;
            trNewSport.Visible = false;
        }
    }

    protected void btnAddNewSport_Click(object sender, EventArgs e)
    {
        ctx = new G7934Entities();
        Sport s = new Sport();
        s.Name = txtNewSport.Text;
        ctx.Sports.Add(s);
        ctx.SaveChanges();
        FillDateAndSports();
        ddlSport.SelectedValue = s.Name;

        //txtNewSport.Visible = false;
        //btnAddNewSport.Visible = false;
        //lblNewSportName.Visible = false;

        trNewSport.Visible = false;
    }
}