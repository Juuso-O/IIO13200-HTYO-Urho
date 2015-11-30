using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class all : System.Web.UI.Page
{
    // Entiteetin ja parin apumuuttujan julistukset
    G7934Entities ctx;
    DateTime startDate;
    DateTime endDate;

    // Luokat datan hakemiseen entiteetistä
    public class GridViewClassA
    {
        public string Nimi { get; set; }
        public DateTime Pvm { get; set; }
        public string Laji { get; set; }
        public int Kesto { get; set; }
    }
    // Ja datan näyttämiseen gridviewissä
    public class GridViewClassB
    {
        public string Nimi { get; set; }
        public string Pvm { get; set; }
        public string Laji { get; set; }
        public int Kesto { get; set; }
    }

    // Sivu ladattu funkkari
    protected void Page_Load(object sender, EventArgs e)
    {
        // Lisätään päivämäärä tekstikentille onfocus ominaisuudet ja liitetään ne piilotettujen nappien onclick eventteihin.
        txtStart.Attributes.Add("onfocus", this.Page.ClientScript.GetPostBackEventReference(this.btnTxtStartEventHandler, ""));
        txtEnd.Attributes.Add("onfocus", this.Page.ClientScript.GetPostBackEventReference(this.btnTxtEndEventHandler, ""));

        // Jos sivu ei ole takaisinkutsu
        if (!IsPostBack)
        {
            // Täytetään taulu
            ddlPopulate();
        }
    }

    // Taulun täyttö funkkari
    private void ddlPopulate()
    {
        // Uusi entiteetti
        ctx = new G7934Entities();
        // Haetaan KAIKKI suoritukset...
        var results = from c in ctx.Accoplishments
                      join a in ctx.People on c.Person equals a.idPerson
                      join b in ctx.Sports on c.Sport equals b.idSport
                      orderby c.Date descending
                      // ...ja pökitään ne GridViewClassA olioihin
                      select new GridViewClassA
                      {
                          Nimi = a.Name,
                          Pvm = c.Date,
                          Laji = b.Name,
                          Kesto = c.Duration
                      };

        // Luodaan lista GridViewClassB olioista
        List<GridViewClassB> list = new List<GridViewClassB>();
        // Plärätään entiteetistä saatu lista läpi
        foreach (var item in results)
        {
            // Ja lisätään kakkoslistaan olioit ykköslistasta
            list.Add(new GridViewClassB()
            {
                Nimi = item.Nimi,
                // DateTimestä leikataan tunnit, minuutit ja sekuntit pois
                Pvm = item.Pvm.ToString("dd.MM.yyyy"),
                Laji = item.Laji,
                Kesto = item.Kesto
            });
        }

        // Leikataan listasta muut kuin 15 ensimmäistä alkiota pois.
        list.RemoveRange(15, list.Count - 15);
        // Ja pistetään data näkymään
        gvData.DataSource = list;
        gvData.DataBind();
    }

    // Datan täyttö päivämäärillä
    private void ddlPopulateWithDates()
    {
        // Jos aloituspäivä on täytetty...
        if (txtStart.Text != "")
        {
            // ...otetaan data sieltä
            startDate = DateTime.Parse(txtStart.Text);
        }
        // Jos taas ei, käytetään oletuspäivämäärää
        else
        {
            startDate = DateTime.Parse("01.01.2000");
        }
        // Ja sama loppupäivälle.
        if (txtEnd.Text != "")
        {
            endDate = DateTime.Parse(txtEnd.Text);
        }
        else
        {
            endDate = DateTime.Parse("01.01.2100");
        }

        // Uusi entiteetti
        ctx = new G7934Entities();
        // Haetaan suoritukset jotka osuvat aikaväliin...
        var results = from c in ctx.Accoplishments
                      join a in ctx.People on c.Person equals a.idPerson
                      join b in ctx.Sports on c.Sport equals b.idSport
                      // ...mikä on tässä!
                      where c.Date > startDate & c.Date < endDate
                      orderby c.Date descending
                      select new GridViewClassA
                      {
                          Nimi = a.Name,
                          Pvm = c.Date,
                          Laji = b.Name,
                          Kesto = c.Duration
                      };

        // Tehdään sama kikkailu kuin ylempänä. Eli saadaan DateTimet silmille kivempaan muotoon
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
        
        // Datasourcen määritys ja datan bindaus.
        gvData.DataSource = list;
        gvData.DataBind();
    }

    // Hae Päivämäärällä napin funktio
    protected void btnGet_Click(object sender, EventArgs e)
    {
        // Haetaan data päivämäärillä
        ddlPopulateWithDates();
    }

    // Hae viimeisimmät napin funktio
    protected void btnGetAll_Click(object sender, EventArgs e)
    {
        // Haetaan 15 viimeisintä suoritusta.
        ddlPopulate();
    }

    // Piilotetun napin funktio jolla saadaan DatePickerinä toimiva alkupäivämäärän
    // kalenteri näkyviin ja pois näkyvistä
    protected void btnTxtStartEventHandler_Click(object sender, EventArgs e)
    {
        // Jos tekstikenttää klikataan ja kalenteri näkyy, se piilotetaan
        if (cldEnd.Visible)
        {
            cldEnd.Visible = false;
        }
        // Ja toisinpäin
        if (!cldStart.Visible)
        {
            cldStart.Visible = true;
        }
    }

    // Sama funktio kuin ylempi mutta loppupäivä kentälle
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

    // Kun alkupäivämäärä kalenterista valitaan arvo...
    protected void cldStart_SelectionChanged(object sender, EventArgs e)
    {
        // ...asetetaan tekstikenttään sen arvo
        txtStart.Text = cldStart.SelectedDate.Day.ToString() + "." +
                        cldStart.SelectedDate.Month.ToString() + "." +
                        cldStart.SelectedDate.Year.ToString();
        // ...ja piilotetaan kalenteri
        cldStart.Visible = false;
    }

    // Ja sama loppupääivämäärälle
    protected void cldEnd_SelectionChanged(object sender, EventArgs e)
    {
        txtEnd.Text = cldEnd.SelectedDate.Day.ToString() + "." +
                      cldEnd.SelectedDate.Month.ToString() + "." +
                      cldEnd.SelectedDate.Year.ToString();
        cldEnd.Visible = false;
    }
}