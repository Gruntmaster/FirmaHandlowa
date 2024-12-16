using FirmaHandlowa.Workers;
using Soneta.Business;
using Soneta.Handel;
using Soneta.KadryPlace.Kadry.Workers;
using Soneta.Towary;
using Soneta.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: Worker(typeof(TowarListWorker), typeof(Towary))]

namespace FirmaHandlowa.Workers
{
    public class TowarListWorker
    {
        [Context]
        public Session Session { get; set; }

        [Context]
        public Context Context { get; set; }

        [Context]
        public TowarListParams Params { get; set; }

        [Context]
        public Towar[] Towary { get; set; }

        [Action("Zrób nowy ZO",
            CommandStyle = Soneta.Commands.CommandStyle.Blue,
            Icon = ActionIcon.Accept,
            Target = ActionTarget.ToolbarWithText,
            Mode = ActionMode.SingleSession | ActionMode.ConfirmFinished)]

        public object TowarListAction()
        {

            // Pobieranie zaznaczonych wierszy z INavigatorContext

            //INavigatorContext navigatorContext = Context[typeof(INavigatorContext), false] as INavigatorContext;
            //Towar[] selectedTowary = navigatorContext.SelectedRows as Towar[];


            DokumentHandlowy dokumentHandlowy;
            using (ITransaction transaction = Session.Logout(true))
            {
                dokumentHandlowy = new DokumentHandlowy();
                dokumentHandlowy.Definicja = Session.GetHandel().DefDokHandlowych.WgSymbolu["OO"];
                Session.AddRow(dokumentHandlowy);

                dokumentHandlowy.Magazyn = Params.Magazyn;
                dokumentHandlowy.Kontrahent = Params.Kontrahent;

                foreach (Towar towar in Towary)
                {
                    PozycjaDokHandlowego pozycja = new PozycjaDokHandlowego(dokumentHandlowy);
                    Session.AddRow(pozycja);
                    pozycja.Towar = towar;
                    pozycja.Ilosc = new Quantity(1, towar.Jednostka.Kod);
                    pozycja.Cena = new Currency(12m, "PLN");
                }

                transaction.CommitUI();
            }
            return dokumentHandlowy;
        }
    }
}
