﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class all : System.Web.UI.Page
{
    G7934Entities ctx;
  
    DateTime startDate;
    DateTime endDate;

    public class GridViewClassA
    {
        public string Nimi { get; set; }
        public DateTime Pvm { get; set; }
        //public string Pvm { get; set; }
        public string Laji { get; set; }
        public int Kesto { get; set; }
    }

    public class GridViewClassB
    {
        public string Nimi { get; set; }
        //public DateTime Pvm { get; set; }
        public string Pvm { get; set; }
        public string Laji { get; set; }
        public int Kesto { get; set; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //txtStart.Attributes.Add("onblur", this.Page.ClientScript.GetPostBackEventReference(this.btnTxtStartEventHandler, ""));
        txtStart.Attributes.Add("onfocus", this.Page.ClientScript.GetPostBackEventReference(this.btnTxtStartEventHandler, ""));
        txtEnd.Attributes.Add("onfocus", this.Page.ClientScript.GetPostBackEventReference(this.btnTxtEndEventHandler, ""));

        if (!IsPostBack)
        {
            ddlPopulate();
        }
    }

    private void ddlPopulate()
    {
        ctx = new G7934Entities();
        var results = from c in ctx.Accoplishments
                      join a in ctx.People on c.Person equals a.idPerson
                      join b in ctx.Sports on c.Sport equals b.idSport
                      orderby c.Date descending
                      select new GridViewClassA
                      {
                          Nimi = a.Name,
                          Pvm = c.Date,
                          Laji = b.Name,
                          Kesto = c.Duration
                      };

        List<GridViewClassB> list = new List<GridViewClassB>();
        foreach (var item in results)
        {
            list.Add(new GridViewClassB()
            {
                Nimi = item.Nimi,
                Pvm = item.Pvm.ToString("dd.MM.yyyy"),
                Laji = item.Laji,
                Kesto = item.Kesto
            });
        }

        //var resultList = results.ToList();
        //resultList.RemoveRange(15, resultList.Count - 15);
        list.RemoveRange(15, list.Count - 15);
        gvData.DataSource = list;
        gvData.DataBind();
    }

    private void ddlPopulateWithDates()
    {
        if (txtStart.Text != "")
        {
            startDate = DateTime.Parse(txtStart.Text);
        }
        else
        {
            startDate = DateTime.Parse("01.01.2000");
        }
        if (txtEnd.Text != "")
        {
            endDate = DateTime.Parse(txtEnd.Text);
        }
        else
        {
            endDate = DateTime.Parse("01.01.2100");
        }

        ctx = new G7934Entities();
        var results = from c in ctx.Accoplishments
                      join a in ctx.People on c.Person equals a.idPerson
                      join b in ctx.Sports on c.Sport equals b.idSport
                      where c.Date > startDate & c.Date < endDate
                      orderby c.Date descending
                      select new GridViewClassA
                      {
                          Nimi = a.Name,
                          Pvm = c.Date,
                          Laji = b.Name,
                          Kesto = c.Duration
                      };

        List<GridViewClassB> list = new List<GridViewClassB>();
        foreach (var item in results)
        {
            list.Add(new GridViewClassB()
            {
                Nimi = item.Nimi,
                Pvm = item.Pvm.ToString("dd.MM.yyyy"),
                Laji = item.Laji,
                Kesto = item.Kesto
            });
        }
        
        gvData.DataSource = list;
        gvData.DataBind();
    }

    protected void btnGet_Click(object sender, EventArgs e)
    {
        ddlPopulateWithDates();
    }

    protected void btnGetAll_Click(object sender, EventArgs e)
    {
        ddlPopulate();
    }

    protected void btnTxtStartEventHandler_Click(object sender, EventArgs e)
    {
        if (cldEnd.Visible)
        {
            cldEnd.Visible = false;
        }
        if (!cldStart.Visible)
        {
            cldStart.Visible = true;
        }
    }

    protected void btnTxtEndEventHandler_Click(object sender, EventArgs e)
    {
        if (cldStart.Visible)
        {
            cldStart.Visible = false;
        }
        if (!cldEnd.Visible)
        {
            cldEnd.Visible = true;
        }
    }

    protected void cldStart_SelectionChanged(object sender, EventArgs e)
    {
        txtStart.Text = cldStart.SelectedDate.Day.ToString() + "." +
                        cldStart.SelectedDate.Month.ToString() + "." +
                        cldStart.SelectedDate.Year.ToString();
        cldStart.Visible = false;
    }

    protected void cldEnd_SelectionChanged(object sender, EventArgs e)
    {
        txtEnd.Text = cldEnd.SelectedDate.Day.ToString() + "." +
                      cldEnd.SelectedDate.Month.ToString() + "." +
                      cldEnd.SelectedDate.Year.ToString();
        cldEnd.Visible = false;
    }
}