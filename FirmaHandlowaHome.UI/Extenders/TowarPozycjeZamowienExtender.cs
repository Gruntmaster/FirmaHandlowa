using FirmaHandlowa.UI.Extenders;
using Soneta.Business;
using Soneta.Handel;
using Soneta.Towary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: Worker(typeof(TowarPozycjeZamowienExtender))]

namespace FirmaHandlowa.UI.Extenders
{
    public class TowarPozycjeZamowienExtender
    {
        [Context]
        public Session Session { get; set; }

        [Context]
        public Towar Towar { get; set; }

        public View ViewPozycjeZamowien
        {
            get
            {
                RowCondition rc = new FieldCondition.Equal("Towar", Towar);
                rc &= new FieldCondition.Equal("Dokument.Kategoria", KategoriaHandlowa.ZamówienieOdbiorcy);
                return Session.GetHandel().PozycjeDokHan.PrimaryKey[rc].CreateView();
            }
        }

    }
}
