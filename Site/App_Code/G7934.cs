﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

public partial class Accoplishment
{
    public int idAccoplishmnet { get; set; }
    public int Duration { get; set; }
    public System.DateTime Date { get; set; }
    public int Person { get; set; }
    public int Sport { get; set; }

    public virtual Person Person1 { get; set; }
    public virtual Sport Sport1 { get; set; }
}

public partial class Person
{
    public Person()
    {
        this.Accoplishments = new HashSet<Accoplishment>();
    }

    public int idPerson { get; set; }
    public string Name { get; set; }

    public virtual ICollection<Accoplishment> Accoplishments { get; set; }
}

public partial class Sport
{
    public Sport()
    {
        this.Accoplishments = new HashSet<Accoplishment>();
    }

    public int idSport { get; set; }
    public string Name { get; set; }

    public virtual ICollection<Accoplishment> Accoplishments { get; set; }
}
