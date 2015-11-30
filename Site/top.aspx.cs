using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class top : System.Web.UI.Page
{
    // Entiteetin julistus
    G7934Entities ctx;

    // Luokka datan näyttämistä varten
    public class GridViewClassC
    {
        public string Nimi { get; set; }
        public int Kerrat { get; set; }
        public int Kesto { get; set; }
    }

    // Kun sivu ladataan...
    protected void Page_Load(object sender, EventArgs e)
    {
        // ...ajetaan hae data funktiota arvolla 'true'
        getData(true);   
    }

    // Jos klikataan hae kokonaisajan mukaan nappia...
    protected void btnDuration_Click(object sender, EventArgs e)
    {
        // ...ajetaan hae data funktiota arvolla 'true'
        getData(true);
    }

    // Datan haku funktio
    public void getData(bool d)
    {
        // Luodaan uusi entiteetti
        ctx = new G7934Entities();
        // Julistetaan lista olioista
        List<GridViewClassC> results;
        
        // Jos funktiota on kutsuttu arvolla 'true'
        if (d)
        {
            // Haetaan data entiteetistä...
            var result = from c in ctx.Accoplishments
                      join p in ctx.People on c.Person equals p.idPerson
                      // ...summaten tuloksen nimien mukaan...
                      group c by p.Name into cc
                      // ...ja järjestäen se summatun kestokentän mukaan
                      orderby cc.Sum(x => x.Duration) descending
                      select new GridViewClassC
                      {
                          Nimi = cc.Key,
                          // Kerrat kenttä - yhteenlasketaan suoritusten määrä
                          Kerrat = cc.Sum(x => 1),
                          // Kesto - yhteenlasketaan kesto kentät.
                          Kesto = cc.Sum(x => x.Duration)
                      };
            // Asetetaan data listaan.
            results = result.ToList();
        }
        // Jos taas funktio kutsuttu arvolla 'false'
        else
        {
            // Haetaan taas dataa entiteestiä...
            var result = from c in ctx.Accoplishments
                          join p in ctx.People on c.Person equals p.idPerson
                          // ...summaten tulekset nimien mukaan...
                          group c by p.Name into cc
                          // ...ja järjestämälle ne summatun kertakentän mukaan
                          orderby cc.Sum(x => 1) descending
                          select new GridViewClassC
                          {
                              Nimi = cc.Key,
                              Kerrat = cc.Sum(x => 1),
                              Kesto = cc.Sum(x => x.Duration)
                          };
            // Data listaksi
            results = result.ToList();
        }
        
        // Ja data näkyviin.
        gvData.DataSource = results;
        gvData.DataBind();
    }

    // Jos klikataan hae kertojen mukaan painiketta
    protected void btnTimes_Click(object sender, EventArgs e)
    {
        // ...ajetaan hae data funktiota arvolla 'false'
        getData(false);
    }
}