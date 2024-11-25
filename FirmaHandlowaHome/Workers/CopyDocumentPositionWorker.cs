using FirmaHandlowaHome.Workers;
using Soneta.Business;
using Soneta.Handel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: Worker(typeof(CopyDocumentPositionWorker), typeof(DokumentHandlowy))]

namespace FirmaHandlowaHome.Workers
{
    public class CopyDocumentPositionWorker
    {
        [Context]
        public Session Session { get; set; }

        [Context]
        public Context Context { get; set; }

        [Action("Kopiuj pozycję",
            CommandStyle = Soneta.Commands.CommandStyle.Blue,
            Icon = ActionIcon.Accept,
            Target = ActionTarget.ToolbarWithText,
            Mode = ActionMode.SingleSession | ActionMode.OnlyForm | ActionMode.ConfirmFinished)]

        public void Copy()
        {
            INavigatorContext navigatorContext = Context[typeof(INavigatorContext)] as INavigatorContext;
            PozycjaDokHandlowego pozycja = navigatorContext.FocusedRow as PozycjaDokHandlowego;

            if (pozycja == null)
            {
                return;
            }

            DokumentHandlowy dokumentHandlowy = Context[typeof(DokumentHandlowy)] as DokumentHandlowy;

            using(ITransaction transaction = Session.Logout(true))
            {
                PozycjaDokHandlowego pozycjaKopia = new PozycjaDokHandlowego(dokumentHandlowy);
                Session.AddRow(pozycjaKopia);
                pozycjaKopia.Towar = pozycja.Towar;
                pozycjaKopia.Cena = pozycja.Cena;
                pozycjaKopia.Ilosc = pozycja.Ilosc;
                pozycjaKopia.Rabat = pozycja.Rabat;

                transaction.CommitUI();
            }
        }
    }
}
