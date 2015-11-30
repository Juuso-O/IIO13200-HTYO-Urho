using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;

public partial class add : System.Web.UI.Page
{
    // Julistetaan entiteetti
    G7934Entities ctx;
    // Rimpsu joka lisätään lajivalikkoon jos lajien lisäys käytössä
    private string addNewSport = "Lisää uusi laji";
    // Katsotaan onko lajien ja käyttäjien lisäys käytössä
    bool AllowNewUsersAndSports = Boolean.Parse(WebConfigurationManager.AppSettings["AllowNewUsersAndSports"]);

    // Sivun lataus
    protected void Page_Load(object sender, EventArgs e)
    {
        // Muutetaan kenttiä sen mukaan onko lajuen ja käyttäjien lisäys käytössä
        ChangeNameEntry(AllowNewUsersAndSports);
        // Jos sivu ei ole takaisinkutsu...
        if (!IsPostBack)
        {
            // Täytetään laji- ja päivämäärävalikot
            FillDateAndSports();
            // Piilotetaan viestikenttä
            lblMessage.Visible = false;
            // Jos lisäys onkin suorituksen muutos...
            if (Request.QueryString["id"] != null)
            {
                // Varmistetaan että Käyttäjänimi kenttä on tekstikenttä eikä alasvetovalikko
                ChangeNameEntry(true);
                // Estetään nimen muutos
                txtName.ReadOnly = true;
            }
        }
    }

    // Funktio jolla vaihdetaan lisättävän käyttäjän nimen ja lajin välillä
    protected void ChangeNameEntry(bool b)
    {
        // Jos lisäys ei käytössä..
        if (!b)
        {
            // ...piilotetaan käyttäjänimen teksikenttä ja näytetään alasvetovalikko...
            divEditableName.Visible = false;
            divUnEditableName.Visible = true;
        }
        // ...ja jos taas on...
        else
        {
            // ...niin toisinpäin
            divEditableName.Visible = true;
            divUnEditableName.Visible = false;
        }
    }

    // Täytetään laji ja päivämäärä valikot
    protected void FillDateAndSports()
    {
        // Uusi entiteetti instanssi
        ctx = new G7934Entities();
        
        // Haetaan lajit entiteetistä...
        var sports = from c in ctx.Sports
                     select c.Name;
        // Luodaan haetuista lajeista lista string alkioita
        List<string> sportList = sports.ToList();

        // Ja jos lajien lisäys on käytössä...
        if (AllowNewUsersAndSports)
        {
            // ...lisätään listan loppuun uusi laji vaihtoehto
            sportList.Add(addNewSport);
        }

        // Asetetaan lajilista lajivalikon datasourceksi...
        ddlSport.DataSource = sportList;
        // ...ja kiinnitetään data valikkoon.
        ddlSport.DataBind();

        // Luodaan lista päivämäärille
        List<String> dates = new List<string>();

        // Apumuuttujat jotai käytetään jos kyseessä suorituksen muokkaus
        string selectedDate = "";
        string selectedSport = "";
        string selectedName = "";
        string selectedDuration = "";
        // Jos kyseessä suorituksen muokkaus
        if (Request.QueryString["id"] != null)
        {
            // Haetaan suoritus
            int id = int.Parse(Request.QueryString["id"]);
            var result = from c in ctx.Accoplishments
                         join a in ctx.People on c.Person equals a.idPerson
                         join b in ctx.Sports on c.Sport equals b.idSport
                         // Suorituksen id pitää olla tietty _id
                         where c.idAccoplishmnet == id
                         // Tulokset läiskäytetään uuteen olioon
                         select new
                         {
                             // Halutut arvot halutuille nimille
                             name = a.Name,
                             date = c.Date,
                             sport = b.Name,
                             duration = c.Duration
                         };
            foreach (var i in result)
            {
                // Jos suorituksen päivämäärä ei muuten tulisi valikkoon...
                if (i.date.CompareTo(DateTime.Today.AddDays(-14)) < 0)
                {
                    // ...lisätään suorituksen päivämäärä päivämäärälistaan
                    dates.Add(i.date.ToString("dd.MM.yyyy"));
                }
                // Merkataan valittavat arvot
                selectedDate = i.date.ToString("dd.MM.yyyy");
                selectedSport = i.sport;
                selectedName = i.name;
                selectedDuration = i.duration.ToString();
            }
        }

        // Lisätään tämä päivämäärä listaan...
        DateTime date = DateTime.Today;
        dates.Add(date.ToString("dd.MM.yyyy"));
        for (int i = 0; i < 13; i++)
        {
            // ...ja sen jälkeen 13 päivää taaksepäin
            date = date.AddDays(-1);
            dates.Add(date.ToString("dd.MM.yyyy"));
        }
        // Lisätään 14 (ehkä 15 jos vanhan suorituksen muokkaus) päivämäärävalikon datasourceksi
        ddlDate.DataSource = dates;
        // Ja bindataan data.
        ddlDate.DataBind();

        // Jos käyttäjien lisäys ei ole sallittu
        if (!AllowNewUsersAndSports)
        {
            // Haetaan entiteetistä valmiit käyttäjät...
            var people = from c in ctx.People
                         select c.Name;
            // ...ja muutetaan ne listaksi ja laitetaan valikon datasourceksi
            ddlName.DataSource = people.ToList();
            // Ja bindataan data.
            ddlName.DataBind();
        }

        // Jos kyseessä suorituksen muokkaus jolloin selected arvot on muutettu...
        if (selectedDate != "" && selectedSport != "" && selectedName != "" && selectedDuration != "")
        {
            // ...asetetaan saadut arvot kenttiin.
            txtName.Text = selectedName;
            txtDuration.Text = selectedDuration;
            ddlSport.SelectedValue = selectedSport;
            ddlDate.SelectedValue = selectedDate;
        }
    }

    // Lisäysnäppäimen painallus
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        // Luodaan uusi entiteetin instanssi
        ctx = new G7934Entities();

        // Jos kyseessä suorituksen muokkaus...
        if (Request.QueryString["id"] != null)
        {
            // Haetaan suoritus _id:n perusteella
            int id = int.Parse(Request.QueryString["id"]);
            var result = from c in ctx.Accoplishments
                         // Tässä merkataan id
                         where c.idAccoplishmnet == id
                         select c;
            // Haetaan lajin _id minkä nimi on tekstikentässä
            var s = from c in ctx.Sports
                    where c.Name == ddlSport.SelectedValue
                    select c;
            // Luodaan apumuuttuja lajin _id:lle...
            int sportId = 0;
            foreach (var i in s)
            {
                // ...ja merkataan siihen arvo
                sportId = i.idSport;
            }
            foreach (var a in result)
            {
                // Merkataan suoritukseen uudet arvot
                a.Date = DateTime.Parse(ddlDate.SelectedValue);
                a.Duration = int.Parse(txtDuration.Text);
                a.Sport = sportId;
            }

            // Ja tallennetaan se..
            ctx.SaveChanges();

            // ...ja näytetään käyttäjälle ilmoitus
            lblMessage.Text = "Muutos tallennettu";
            lblMessage.Visible = true;
        }
        // Jos taas kyseessä on uusi suoritus
        else
        {
            // Luodaan apumuuttuja...
            string name = "";
            // ...ja jos uusien nimien lisäys on mahdollista...
            if (AllowNewUsersAndSports)
            {
                // ...haetaan nimi tekstikentästä...
                name = txtName.Text;
            }
            // ...jos taas lisäys ei ole käytössä
            else
            {
                // ...haetaan nimi alasvetovalikosta.
                name = ddlName.SelectedValue;
            }

            // Haetaan tallennettavan henkilön _id kenttä entiteetistä...
            var person = from c in ctx.People
                    where c.Name == name
                    select c.idPerson;
            int personId = 0;
            foreach (var i in person)
            {
                // ...ja merkataan se apumuuttujaan.
                personId = i;
            }
            // Haetaan henkilön suoritukset...
            var results = from c in ctx.Accoplishments
                         where c.Person == personId
                         select c;
            int counter = 0;
            foreach (var i in results)
            {
                // ...ja jos henkilöllä on jo suorituksia valitulle päivälle niin lasketaan ne.
                if (i.Date.ToString("dd.MM.yyyy").Equals(ddlDate.SelectedValue)) {
                    counter++;
                }
            }

            // Jos suorituksiä päivälle on 5...
            if (counter > 4)
            {
                // ...ilmoitetaan siitä käyttäjälle...
                lblMessage.Text = "Sinulla on tällä päivällä jo " + counter + " suoritusta, et voi lisätä enempää.";
                lblMessage.Visible = true;
                // ...ja piilotetaan lisäysnapit.
                btnAdd.Visible = false;
                btnConfirmedAdd.Visible = false;
            }
            // Jos taas suorituksia on mutta ei vielä viittä...
            else if (counter > 0)
            {
                // ...piilotetaan peruslisäysnappi ja näytetään sekondäärilisäysnappi...
                btnAdd.Visible = false;
                btnConfirmedAdd.Visible = true;
                // ...ja ilmoitetaan tästä käyttäjälle.
                lblMessage.Text = "Sinulla on tällä päivällä jo " + counter.ToString() + " suoritusta, lisätäänkö uusi?";
                lblMessage.Visible = true;
            }
            // Jos suorituksia ei vielä kyseiselle päivälle ole, lisätään suoritus
            else
            {
                AddNew();
            }
        }

        
    }

    // Sekondäärilisäysnappi
    protected void btnConfirmedAdd_Click(object sender, EventArgs e)
    {
        // Kun ollaan varmistettu suorituksen lisäys, lisätään se.
        ctx = new G7934Entities();
        AddNew();
    }

    // Suorituksen lisäysfunkkari
    private void AddNew()
    {
        // Luodaan uusi suoritusolio
        Accoplishment nA = new Accoplishment();

        // Käyttäjien lisäysmoodista riippuen haetaan lisättävän käyttäjän nimi oikeasta kentästä.
        string name = "";
        if (AllowNewUsersAndSports)
        {
            name = txtName.Text;
        }
        else
        {
            name = ddlName.SelectedValue;
        }
        // Ja haetaan käyttäjän id nimen perusteella.
        var p = from c in ctx.People
                select c;

        // Apumuuttuja henkilön etsimistä varten
        bool found = false;

        // Plärätään henkilöitä läpi...
        foreach (var i in p)
        {
            // ...ja jos nimi täsmää haettavaan...
            if (i.Name == name)
            {
                // ...merkataan että käyttäjä on löytynyt...
                found = true;
                // ...ja asetetaan uuden suorituksen henkilöksi kyseinen henkilö.
                nA.Person = i.idPerson;
                break;
            }
        }
        // Jos henkilöä ei löytynyt...
        if (!found)
        {
            // Luodaan uuse henkilö...
            Person pe = new Person();

            // ...asetetaan sille nimi tekstikentästä...
            pe.Name = txtName.Text;

            // ...lisätään se entiteettiin...
            ctx.People.Add(pe);

            // ...ja tallennetaan se sinne.
            ctx.SaveChanges();

            // Lopuksi merkataan henkilö uuden suorituksen henkilöksi.
            nA.Person = pe.idPerson;
        }

        // Haetaan lajit entiteetistä...
        var s = from c in ctx.Sports
                select c;
        foreach (var i in s)
        {
            // ...etsitään valittu laji...
            if (i.Name == ddlSport.SelectedValue)
            {
                // ...ja merkataan se uuteen suoritukseen
                nA.Sport = i.idSport;
            }
        }

        // Merkataan vielä suoritukselle päivämäärä ja kesto
        nA.Date = DateTime.Parse(ddlDate.SelectedValue);
        nA.Duration = int.Parse(txtDuration.Text);

        // Lisätään entiteettiin uusi suoritus
        ctx.Accoplishments.Add(nA);

        // Ja tallennetaan se.
        ctx.SaveChanges();

        // Asetetaan kentät tyhjiksi
        txtName.Text = "";
        txtDuration.Text = "";
        ddlDate.SelectedIndex = 0;
        ddlSport.SelectedIndex = 0;

        // Kerrotaan käyttäjälle tapahtuneesta...
        lblMessage.Text = "Suoritus tallennettu";
        lblMessage.Visible = true;
        // ...ja piilotetaan lisäysnappulat
        btnAdd.Visible = false;
        btnConfirmedAdd.Visible = false;
    }

    // Funkkari mikä ajetaan kun vaihdetaan lajikentän arvoa
    protected void ddlSport_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Jos valittu arvo on uuden lajin lisäys...
        if (ddlSport.SelectedValue.Equals(addNewSport))
        {
            // ...näytetään uuden lajin lisäyssälät
            trNewSport.Visible = true;
        }
        // Jos taas arvo on jotain muuta...
        else
        {
            // ...ei näytetä lisäyssälöjä
            trNewSport.Visible = false;
        }
    }

    // Uuden lajin lisäysnapin funktio
    protected void btnAddNewSport_Click(object sender, EventArgs e)
    {
        // Uusi entiteetin instanssi
        ctx = new G7934Entities();
        // Luodaan uusi lajin olio
        Sport s = new Sport();
        // Asetetaan sille nimi tekstikentästä
        s.Name = txtNewSport.Text;
        // Lisätään se entiteettiin
        ctx.Sports.Add(s);
        // Ja tallennetaan se
        ctx.SaveChanges();
        // Täytetään lajivalikko uudestaan
        FillDateAndSports();
        // Ja asetataan juuri luotu laji valituksi
        ddlSport.SelectedValue = s.Name;
        
        // Piilotetaan lajinlisäyssälät
        trNewSport.Visible = false;
    }
}