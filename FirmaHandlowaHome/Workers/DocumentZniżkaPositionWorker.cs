using FirmaHandlowa.Workers;
using FirmaHandlowaHome.Workers;
using Soneta.Business;
using Soneta.Handel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly : Worker(typeof(DocumentZniżkaPositionWorker),typeof(DokumentHandlowy))]

namespace FirmaHandlowaHome.Workers
{
    public class DocumentZniżkaPositionWorker
    {
        [Context]
        public Session Session { get; set; }
        [Context]
        public DokumentHandlowy DokumentHandlowy { get; set; }
        [Context]
        public DocumentZnizkaPositionParams Params { get; set; }

        [Action("Dodaj Zniżkę",
          CommandStyle = Soneta.Commands.CommandStyle.LightGreen,
          Icon = ActionIcon.Asterix,
          Target = ActionTarget.ToolbarWithText,
          Mode = ActionMode.SingleSession | ActionMode.OnlyForm | ActionMode.ConfirmFinished)]

        public string DocumentZnizkaAction()
        {
            if (Params.Znizka <= 0)
            {
                throw new Exception("Podano złą wartość zniżki");
            }

            using (ITransaction transaction = Session.Logout(true))
            {
                foreach (PozycjaDokHandlowego pozycja in DokumentHandlowy.Pozycje)
                {
                    pozycja.Cena = pozycja.Cena * Params.Znizka;
                }
                transaction.CommitUI();
            }
            return $"Zmniejszono cenę wszystkich pozycji o {Params.Znizka}";
        }

    }
}
