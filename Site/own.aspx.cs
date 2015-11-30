using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class own : System.Web.UI.Page
{
    // Entiteetin julistus.
    G7934Entities ctx;
    
    // Luokka datan hakemiseen entiteetistä.
    public class GridViewClassC
    {
        public string Nimi { get; set; }
        public DateTime Pvm { get; set; }
        public string Laji { get; set; }
        public int Kesto { get; set; }
        public int Id { get; set; }
    }
    // Luokka datan näyttämiseen.
    public class GridViewClassD
    {
        public string Nimi { get; set; }
        // Kun päivämäärä onkin string tyyppinen siitä voidaan leikata
        // tunnit, minuutit ja sekunnit pois silti käyttäen datasourcea
        public string Pvm { get; set; }
        public string Laji { get; set; }
        public int Kesto { get; set; }
        public int Id { get; set; }
    }

    // Sivun lataus funktio.
    protected void Page_Load(object sender, EventArgs e)
    {
        // Jos sivulle ollaan tultu katselemaan jonkun (muun) henkilön suorituksia...
        if (Request.QueryString["name"] != null)
        {
            // Otetaan nimi talteen osoiteriviltä
            string nameForWhere = Request.QueryString["name"];
            // Entiteetin uusi instanssi
            ctx = new G7934Entities();
            // Haetaan data entiteesitä GridViewClassC olioihin...
            var results = from c in ctx.Accoplishments
                          join a in ctx.People on c.Person equals a.idPerson
                          join b in ctx.Sports on c.Sport equals b.idSport
                          // ...joiden suorittaja on kyseinen henkilö.
                          where a.Name == nameForWhere
                          orderby c.Date descending
                          select new GridViewClassC
                          {
                              Nimi = a.Name,
                              Pvm = c.Date,
                              Laji = b.Name,
                              Kesto = c.Duration,
                              Id = c.idAccoplishmnet
                          };

            // Tehdään GridViewClassC luokan oliojoukosta lista joukon
            // GridViewClassD olioita jotta päivämäärä saadaan kivaan muotoon
            List<GridViewClassD> list = new List<GridViewClassD>();
            foreach (var item in results)
            {
                list.Add(new GridViewClassD()
                {
                    Nimi = item.Nimi,
                    Pvm = item.Pvm.ToString("dd.MM.yyyy"),
                    Laji = item.Laji,
                    Kesto = item.Kesto,
                    Id = item.Id
                });
            }

            // Datan bindaus
            gvData.DataSource = list;
            gvData.DataBind();
            // Piilotetaan ensimmäinen linkkikolumni
            gvData.Columns[0].Visible = false;

            // Kerrotaan käyttäjälle kenen tuloksia näytetään.
            lblUser.Text = "Näytetään henkilön " + nameForWhere + " tulokset";
        }
        // Jos taas sivulle on tultu yläpalkin linkin kautta ja käyttäjä on jo valittu
        else if (Request.Cookies["UserSettings"] != null)
        {
            // Otetaan keksistä käyttäjän nimi talteen
            string nameForWhere = Request.Cookies["UserSettings"]["Name"];
            // Luodaan uusi instanssi entiteetistä
            ctx = new G7934Entities();
            // Ja haetaan käyttäjän suoritukset entiteetistä.
            var results = from c in ctx.Accoplishments
                          join a in ctx.People on c.Person equals a.idPerson
                          join b in ctx.Sports on c.Sport equals b.idSport
                          where a.Name == nameForWhere
                          orderby c.Date descending
                          select new GridViewClassC
                          {
                              Nimi = a.Name,
                              Pvm = c.Date,
                              Laji = b.Name,
                              Kesto = c.Duration,
                              Id = c.idAccoplishmnet
                          };

            // Muutetaan taas oliot luokasta C luokkaan D jotta päivämäärät on kivoja.
            List<GridViewClassD> list = new List<GridViewClassD>();
            foreach (var item in results)
            {
                list.Add(new GridViewClassD()
                {
                    Nimi = item.Nimi,
                    Pvm = item.Pvm.ToString("dd.MM.yyyy"),
                    Laji = item.Laji,
                    Kesto = item.Kesto,
                    Id = item.Id
                });
            }

            // Asetetaan taas data näkyviin
            gvData.DataSource = list;
            gvData.DataBind();
            // Ja tällä kertaa piilotetaan kolumni 1 jossa näkyy nimet ilman linkkejä
            gvData.Columns[1].Visible = false;

            // Kerrotaan vielä käyttäjälle kuka hän on
            lblUser.Text = "Valittuna: " + nameForWhere;
            btnLogOut.Visible = true;
        }
        // Jos taas käyttäjää ei ole valittu lainkaan
        else
        {
            // Pyydetään käyttäjää valitsemaan itsentä alasvetovalikosta...
            lblUser.Text = "Valitse kuka olet";
            // ...joka asetetaan näkyviin.
            ddlUser.Visible = true;
            // Ja mikäli dataa valikkoon ei vielä ole haettu
            if(!IsPostBack)
            {
                // Luodaan uusi entiteetti
                ctx = new G7934Entities();
                // Haetaan sieltä ihmiset
                var results = from c in ctx.People
                              select c.Name;
                // Luodaan lista...
                List<string> list = new List<string>();
                // ...johon lisätään ensimmäiseksi tyhjä valinta...
                list.Add("");
                // ...ja sitten nimet entiteetin joukosta.
                foreach (var i in results)
                {
                    list.Add(i);
                }

                // Ja sitten data näkyviin.
                ddlUser.DataSource = list;
                ddlUser.DataBind();
            }
        }
    }

    // Kun käyttäjänvalinta alasvetovalikon valinta muuttuu...
    protected void ddlUser_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Luodaan uusi keksi...
        HttpCookie myCookie = new HttpCookie("UserSettings");
        // ...johon asetetaan käyttäjän nimi...
        myCookie["Name"] = ddlUser.SelectedItem.Text;
        // ...sekä asetetaan sen voimassaoloaika vuodeksi...
        myCookie.Expires = DateTime.Now.AddYears(1);
        // ...sitten lisätään se...
        Response.Cookies.Add(myCookie);
        // ...ja ohjataan käyttäjä uudestaan samalle sivulle, nyt hän päätyy
        // näkemään omat tuloksensa kun keksi on asetettu
        Response.Redirect(Request.RawUrl);
    }

    // Jos käyttäjä klikkaa kirjaudu ulos nappia...
    protected void btnLogOut_Click(object sender, EventArgs e)
    {
        // Luodaan keksi uudestaan...
        HttpCookie myCookie = new HttpCookie("UserSettings");
        // ...mutta asetetaan sen expiretime menneisyyteen
        // jolloin se kuolee heti.
        myCookie.Expires = DateTime.Now.AddDays(-1d);
        // Asetetaan keksi...
        Response.Cookies.Add(myCookie);
        // ...ja ohjataan käyttäjä uudestaan sivulle, nyt häneltä
        // vaaditaan taas sisäänkirjautuminen.
        Response.Redirect(Request.RawUrl);
    }
}