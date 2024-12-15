using FirmaHandlowaHome.Workers;
using Soneta.Business;
using Soneta.Handel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: Worker(typeof(CreateZOWorker),typeof(DokHandlowe))]

namespace FirmaHandlowaHome.Workers
{
    public class CreateZOWorker
    {
        [Context]
        public Session Session { get; set; }

        [Context]
        public DokumentHandlowy DokumentHandlowy { get; set; }

        [Context]
        public CreateZOParams Params { get; set; }

        [Action("Zrób nowy ZO",
          CommandStyle = Soneta.Commands.CommandStyle.Green,
          Icon = ActionIcon.Book,
          Target = ActionTarget.ToolbarWithText,
          Mode = ActionMode.SingleSession | ActionMode.OnlyForm | ActionMode.ConfirmFinished)]

        public object DocumentNewZOAction()
        {
            DefDokHandlowego defDokHandlowego = Session.GetHandel().DefDokHandlowych.WgSymbolu["ZO"];

            DokumentHandlowy dokumentHandlowy;
            using (ITransaction transaction = Session.Logout(true))
            {
                dokumentHandlowy = new DokumentHandlowy();
                dokumentHandlowy.Definicja = defDokHandlowego;
                dokumentHandlowy.Magazyn = Params.Magazyn;
                Session.AddRow(dokumentHandlowy);

                dokumentHandlowy.Kontrahent = Params.Kontrahent;

                PozycjaDokHandlowego pozycja = new PozycjaDokHandlowego(dokumentHandlowy);
                Session.AddRow(pozycja);
                pozycja.Towar = Params.Towar;
                pozycja.Ilosc = new Soneta.Towary.Quantity(10, Params.Towar.Jednostka.Kod);

                transaction.CommitUI();
            }
            return dokumentHandlowy;
        }

    }
}

