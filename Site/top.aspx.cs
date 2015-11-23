using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class top : System.Web.UI.Page
{
    G7934Entities ctx;

    public class GridViewClassC
    {
        public string Nimi { get; set; }
        public int Kerrat { get; set; }
        public int Kesto { get; set; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        getData(true);   
    }

    protected void btnDuration_Click(object sender, EventArgs e)
    {
        getData(true);
    }

    public void getData(bool d)
    {
        ctx = new G7934Entities();
        List<GridViewClassC> results;
        
        if (d)
        {
            var result = from c in ctx.Accoplishments
                      join p in ctx.People on c.Person equals p.idPerson
                      group c by p.Name into cc
                      orderby cc.Sum(x => x.Duration) descending
                      select new GridViewClassC
                      {
                          Nimi = cc.Key,
                          Kerrat = cc.Sum(x => 1),
                          Kesto = cc.Sum(x => x.Duration)
                      };
            results = result.ToList();
        }
        else
        {
            var result = from c in ctx.Accoplishments
                          join p in ctx.People on c.Person equals p.idPerson
                          group c by p.Name into cc
                          orderby cc.Sum(x => 1) descending
                          select new GridViewClassC
                          {
                              Nimi = cc.Key,
                              Kerrat = cc.Sum(x => 1),
                              Kesto = cc.Sum(x => x.Duration)
                          };
            results = result.ToList();
        }
        
        gvData.DataSource = results;
        gvData.DataBind();
    }

    protected void btnTimes_Click(object sender, EventArgs e)
    {
        getData(false);
    }
}